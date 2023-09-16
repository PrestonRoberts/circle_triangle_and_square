using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameGrid : MonoBehaviour
{
    // Game Manager
    public GameManager gameManager;

    // Game Canvas
    public GameObject gameCanvas;

    // Level select panel
    public GameObject levelSelectPanel;
    public LevelSelect levelSelect;

    // Grid Dimensions
    public int gridSize = 900;
    public int gridWidth;
    public int gridHeight;

    // Grid 
    public GridLayoutGroup gridLayoutGroup;
    public RectTransform gridRectTransform;
    private Level currentLevel;

    // Grid Structure
    public GridSquare[,] grid;

    // Grid Squares
    public GameObject gridSquarePrefab;

    // Player circles
    private GameObject bluePlayerCircle;
    private GameObject redPlayerCircle;
    private GameObject greenPlayerCircle;
    private GameObject yellowPlayerCircle;
    private GameObject whitePlayerCircle;

    // Player circle prefab
    public GameObject playerCirclePrefab;

    // Checkpoint flags
    private GameObject blueCheckpointFlag;
    private GameObject redCheckpointFlag;
    private GameObject greenCheckpointFlag;
    private GameObject yellowCheckpointFlag;

    // Checkpoint flag prefab
    public GameObject checkpointFlagPrefab;

    // Controls
    public GameObject controlsPanel;
    public GameObject startStopButton;
    public GameObject clearButton;

    // Start stop button sprites
    public Sprite startButtonSprite;
    public Sprite stopButtonSprite;

    // Simulation
    private bool isSimulationRunning;
    private bool initialGeneration = true;
    private bool simulationFailed = false;
    private int BPM = 120;

    // Level completed
    public GameObject levelCompletedPanel;

    // Level failed
    public GameObject levelFailedPanel;

    // Level select button
    public GameObject levelSelectButton;

    // Audio source
    private AudioSource audioSource;

    void Start()
    {
        levelCompletedPanel.SetActive(false);
        levelFailedPanel.SetActive(false);

        // Get audio source
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadGridData(Level currentLevel)
    {
        // Clear Board
        ClearBoard();

        BPM = 120;

        // Set up board
        isSimulationRunning = false;
        initialGeneration = true;
        SetButtonToStart();

        // Set current level
        this.currentLevel = currentLevel;

        // Set grid width and height
        gridWidth = currentLevel.gridWidth;
        gridHeight = currentLevel.gridHeight;

        // Enable stuff
        controlsPanel.SetActive(true);
        clearButton.GetComponent<Button>().interactable = true;
        levelSelectButton.GetComponent<Button>().interactable = true;

        // Disable stuff
        levelCompletedPanel.SetActive(false);
        levelFailedPanel.SetActive(false);

        CreateGrid();
    }

    // Reset board
    public void ResetBoard()
    {
        ClearBoard();

        // Play sound
        audioSource.Play();

        initialGeneration = true;

        CreateGrid();
    }

    // Clear board
    void ClearBoard()
    {
        // Clear grid
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Clear player circles
        bluePlayerCircle = null;
        redPlayerCircle = null;
        greenPlayerCircle = null;
        yellowPlayerCircle = null;
        whitePlayerCircle = null;

        // Clear checkpoint flags
        blueCheckpointFlag = null;
        redCheckpointFlag = null;
        greenCheckpointFlag = null;
        yellowCheckpointFlag = null;
    }

    void CreateGrid()
    {
        // Create grid
        switch (gridWidth)
        {
            case 3:
                gridSize = 900;
                break;
            case 4:
                gridSize = 900;
                break;
            case 5:
                gridSize = 900;
                break;
            case 6:
                gridSize = 900;
                break;
            case 7:
                gridSize = 896;
                break;
            case 8:
                gridSize = 896;
                break;
            case 9:
                gridSize = 900;
                break;
            case 10:
                gridSize = 900;
                break;
        }

        // Set grid size
        gridRectTransform.sizeDelta = new Vector2(gridSize, gridSize);

        int squareSize = gridSize / gridWidth;
        gridLayoutGroup.cellSize = new Vector2(squareSize, squareSize);

        if (initialGeneration)
        {
            // Create data structure
            grid = new GridSquare[gridHeight, gridWidth];

            // Create grid squares
            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    // Create grid square
                    GameObject gridSquare = Instantiate(gridSquarePrefab);

                    // Get the grid square script
                    GridSquare gridSquareScript = gridSquare.GetComponent<GridSquare>();

                    // Set parent
                    gridSquare.transform.SetParent(transform);

                    // Set grid square name
                    gridSquare.name = "GridSquare_" + j + "_" + i;

                    // Set grid square grid
                    grid[i, j] = gridSquareScript;

                    // Set grid square to 1 scale
                    gridSquare.transform.localScale = Vector3.one;

                    // Reset square to base and no arrow
                    gridSquareScript.ResetSquare();
                }
            }
        }

        // Set grid square colors
        SetGridSquareColors();

        // Set grid starting points
        SetGridStartingPoints();

        // Set grid checkpoints
        SetGridCheckpoints();

        if (!initialGeneration)
        {
            // Enable grid squares, clear player, load grid
            foreach (GridSquare gridSquare in grid)
            {
                gridSquare.arrowDisabled = false;
                gridSquare.rotationDisabled = false;
                gridSquare.ClearSquare();
                gridSquare.LoadSquare();
            }
        }

        controlsPanel.SetActive(true);
        initialGeneration = false;
    }

    void SetGridSquareColors()
    {
        // Update blue grid type
        if (currentLevel.blueColor.isUsed)
        {
            // length of grid
            int gridLength = grid.GetLength(0);

            // length of each row
            int rowLength = grid.GetLength(1);

            grid[currentLevel.blueColor.end_y, currentLevel.blueColor.end_x].UpdateGridType(GridSquare.GridType.Blue);
        }

        // Update red grid type
        if (currentLevel.redColor.isUsed)
        {
            grid[currentLevel.redColor.end_y, currentLevel.redColor.end_x].UpdateGridType(GridSquare.GridType.Red);
        }

        // Update green grid type
        if (currentLevel.greenColor.isUsed)
        {
            grid[currentLevel.greenColor.end_y, currentLevel.greenColor.end_x].UpdateGridType(GridSquare.GridType.Green);
        }

        // Update yellow grid type
        if (currentLevel.yellowColor.isUsed)
        {
            grid[currentLevel.yellowColor.end_y, currentLevel.yellowColor.end_x].UpdateGridType(GridSquare.GridType.Yellow);
        }

        // Update white grid type
        if (currentLevel.whiteColor.isUsed)
        {
            grid[currentLevel.whiteColor.end_y, currentLevel.whiteColor.end_x].UpdateGridType(GridSquare.GridType.White);
        }
    }

    // Set grid starting points
    void SetGridStartingPoints()
    {
        // Create blue player circle if used
        if (currentLevel.blueColor.isUsed)
        {
            bluePlayerCircle = CreatePlayerCircle(PlayerCircle.CircleColor.Blue, "BluePlayerCircle", currentLevel.blueColor.start_x, currentLevel.blueColor.start_y);
        }

        // Create red player circle if used
        if (currentLevel.redColor.isUsed)
        {
            redPlayerCircle = CreatePlayerCircle(PlayerCircle.CircleColor.Red, "RedPlayerCircle", currentLevel.redColor.start_x, currentLevel.redColor.start_y);
        }

        // Create green player circle if used
        if (currentLevel.greenColor.isUsed)
        {
            greenPlayerCircle = CreatePlayerCircle(PlayerCircle.CircleColor.Green, "GreenPlayerCircle", currentLevel.greenColor.start_x, currentLevel.greenColor.start_y);
        }

        // Create yellow player circle if used
        if (currentLevel.yellowColor.isUsed)
        {
            yellowPlayerCircle = CreatePlayerCircle(PlayerCircle.CircleColor.Yellow, "YellowPlayerCircle", currentLevel.yellowColor.start_x, currentLevel.yellowColor.start_y);
        }

        // Create white player circle if used
        if (currentLevel.whiteColor.isUsed)
        {
            whitePlayerCircle = CreatePlayerCircle(PlayerCircle.CircleColor.White, "WhitePlayerCircle", currentLevel.whiteColor.start_x, currentLevel.whiteColor.start_y);
        }
    }

    private GameObject CreatePlayerCircle(PlayerCircle.CircleColor color, string circleName, int startX, int startY)
    {
        GameObject playerCircle;
        if (initialGeneration)
        {
            playerCircle = Instantiate(playerCirclePrefab);
            playerCircle.name = circleName;
            playerCircle.GetComponent<PlayerCircle>().ChangePlayerColor(color);
        }
        else
        {
            playerCircle = GameObject.Find(circleName);
        }

        playerCircle.transform.SetParent(grid[startY, startX].transform);

        playerCircle.transform.localScale = Vector3.one;
        playerCircle.GetComponent<PlayerCircle>().ResetTransform();

        grid[startY, startX].GetComponent<GridSquare>().SetStartingSquare();

        playerCircle.GetComponent<PlayerCircle>().locationX = startX;
        playerCircle.GetComponent<PlayerCircle>().locationY = startY;

        return playerCircle;
    }

    // Set grid checkpoints
    void SetGridCheckpoints()
    {
        // Create checkpoint flags if used
        if (currentLevel.blueColor.isUsed)
        {
            blueCheckpointFlag = CreateCheckpointFlag("BlueCheckpointFlag", CheckpointFlag.Flagcolor.Blue, currentLevel.blueColor.checkpoint_x, currentLevel.blueColor.checkpoint_y);
        }

        if (currentLevel.redColor.isUsed)
        {
            redCheckpointFlag = CreateCheckpointFlag("RedCheckpointFlag", CheckpointFlag.Flagcolor.Red, currentLevel.redColor.checkpoint_x, currentLevel.redColor.checkpoint_y);
        }

        if (currentLevel.greenColor.isUsed)
        {
            greenCheckpointFlag = CreateCheckpointFlag("GreenCheckpointFlag", CheckpointFlag.Flagcolor.Green, currentLevel.greenColor.checkpoint_x, currentLevel.greenColor.checkpoint_y);
        }

        if (currentLevel.yellowColor.isUsed)
        {
            yellowCheckpointFlag = CreateCheckpointFlag("YellowCheckpointFlag", CheckpointFlag.Flagcolor.Yellow, currentLevel.yellowColor.checkpoint_x, currentLevel.yellowColor.checkpoint_y);
        }
    }

    private GameObject CreateCheckpointFlag(string flagName, CheckpointFlag.Flagcolor flagColor, int checkpointX, int checkpointY)
    {
        GameObject checkpointFlag;

        if (initialGeneration)
        {
            checkpointFlag = Instantiate(checkpointFlagPrefab);

        }
        else
        {
            checkpointFlag = GameObject.Find(flagName);
        }
        checkpointFlag.name = flagName;
        checkpointFlag.transform.SetParent(grid[checkpointY, checkpointX].transform);
        checkpointFlag.transform.localScale = Vector3.one;

        CheckpointFlag checkpointFlagComponent = checkpointFlag.GetComponent<CheckpointFlag>();
        checkpointFlagComponent.ChangeFlagColor(flagColor);
        checkpointFlagComponent.locationX = checkpointX;
        checkpointFlagComponent.locationY = checkpointY;
        checkpointFlagComponent.ReleaseFlag();

        return checkpointFlag;
    }

    void SetButtonToStart()
    {
        // Set button to start button sprite
        Button button = startStopButton.GetComponent<Button>();
        button.image.sprite = startButtonSprite;
    }

    void SetButtonToStop()
    {
        // Set button to stop button sprite
        Button button = startStopButton.GetComponent<Button>();
        button.image.sprite = stopButtonSprite;
    }

    private IEnumerator Beats()
    {
        while (true)
        {
            if (isSimulationRunning)
            {
                Simulation();
            }
            yield return new WaitForSeconds(60f / BPM);
        }
    }

    // Simulation
    private bool firstBeat = true;
    private void Simulation()
    {
        if (simulationFailed) return;

        if (!firstBeat)
        {
            if (currentLevel.greenColor.isUsed)
            {
                GridSquare greenSquare = MovePlayerCircle(greenPlayerCircle);

                // Set danger on green square
                greenSquare.SetDanger();
            }

            // Move all colors is they are used
            if (currentLevel.blueColor.isUsed)
            {
                MovePlayerCircle(bluePlayerCircle);
            }

            if (currentLevel.yellowColor.isUsed)
            {
                GridSquare yellowSquare = MovePlayerCircle(yellowPlayerCircle);

                // Flip the arrow of the yellow square
                if (yellowSquare.arrowType == GridSquare.ArrowType.North)
                {
                    yellowSquare.SetArrowType(GridSquare.ArrowType.South);
                }
                else if (yellowSquare.arrowType == GridSquare.ArrowType.East)
                {
                    yellowSquare.SetArrowType(GridSquare.ArrowType.West);
                }
                else if (yellowSquare.arrowType == GridSquare.ArrowType.South)
                {
                    yellowSquare.SetArrowType(GridSquare.ArrowType.North);
                }
                else if (yellowSquare.arrowType == GridSquare.ArrowType.West)
                {
                    yellowSquare.SetArrowType(GridSquare.ArrowType.East);
                }
            }

            if (currentLevel.whiteColor.isUsed)
            {
                GridSquare whiteSquare = MovePlayerCircle(whitePlayerCircle);

                // Remove the arrow of the white square
                whiteSquare.SetArrowType(GridSquare.ArrowType.None);
            }
        }

        // Move red
        if (currentLevel.redColor.isUsed)
        {
            MovePlayerCircle(redPlayerCircle);
        }

        if (simulationFailed)
        {
            SimulationFailed();
            StartCoroutine(WaitForSimulationEnd());
            return;
        }

        // Create dictionary of grid squares
        Dictionary<GridSquare, int> gridSquares = new Dictionary<GridSquare, int>();

        // Check for win
        bool win = true;

        // Get all player grid squares if they are used
        if (currentLevel.blueColor.isUsed)
        {
            int x = bluePlayerCircle.GetComponent<PlayerCircle>().locationX;
            int y = bluePlayerCircle.GetComponent<PlayerCircle>().locationY;

            // Check if checkpoint flag on same square
            CheckpointFlag blueFlag = blueCheckpointFlag.GetComponent<CheckpointFlag>();
            if (blueFlag.locationX == x && blueFlag.locationY == y)
            {
                // Capture flag
                blueFlag.CaptureFlag();
            }

            GridSquare blueSquare = grid[y, x];

            gridSquares.Add(blueSquare, 0);

            // Check if blue square is blue and flag is captured
            if (blueSquare.gridType != GridSquare.GridType.Blue || !blueFlag.isCaptured)
            {
                win = false;
            }
        }

        if (currentLevel.redColor.isUsed)
        {
            int x = redPlayerCircle.GetComponent<PlayerCircle>().locationX;
            int y = redPlayerCircle.GetComponent<PlayerCircle>().locationY;

            // Check if checkpoint flag on same square
            CheckpointFlag redFlag = redCheckpointFlag.GetComponent<CheckpointFlag>();
            if (redFlag.locationX == x && redFlag.locationY == y)
            {
                // Capture flag
                redFlag.CaptureFlag();
            }

            GridSquare redSquare = grid[y, x];

            // Check if red square is in grid squares
            if (gridSquares.ContainsKey(redSquare))
            {
                simulationFailed = true;
                redSquare.SetFailSquare();
            }
            else
            {
                gridSquares.Add(redSquare, 0);
            }

            // Check if red square is red and flag is captured
            if (redSquare.gridType != GridSquare.GridType.Red || !redFlag.isCaptured)
            {
                win = false;
            }
        }

        if (currentLevel.greenColor.isUsed)
        {
            int x = greenPlayerCircle.GetComponent<PlayerCircle>().locationX;
            int y = greenPlayerCircle.GetComponent<PlayerCircle>().locationY;

            // Check if checkpoint flag on same square
            CheckpointFlag greenFlag = greenCheckpointFlag.GetComponent<CheckpointFlag>();
            if (greenFlag.locationX == x && greenFlag.locationY == y)
            {
                // Capture flag
                greenFlag.CaptureFlag();
            }

            GridSquare greenSquare = grid[y, x];

            // Check if green square is in grid squares
            if (gridSquares.ContainsKey(greenSquare))
            {
                simulationFailed = true;
                greenSquare.SetFailSquare();
            }
            else
            {
                gridSquares.Add(greenSquare, 0);
            }

            // Check if green square is green and flag is captured
            if (greenSquare.gridType != GridSquare.GridType.Green || !greenFlag.isCaptured)
            {
                win = false;
            }
        }

        if (currentLevel.yellowColor.isUsed)
        {
            int x = yellowPlayerCircle.GetComponent<PlayerCircle>().locationX;
            int y = yellowPlayerCircle.GetComponent<PlayerCircle>().locationY;

            // Check if checkpoint flag on same square
            CheckpointFlag yellowFlag = yellowCheckpointFlag.GetComponent<CheckpointFlag>();
            if (yellowFlag.locationX == x && yellowFlag.locationY == y)
            {
                // Capture flag
                yellowFlag.CaptureFlag();
            }

            GridSquare yellowSquare = grid[y, x];

            // Check if yellow square is in grid squares
            if (gridSquares.ContainsKey(yellowSquare))
            {
                simulationFailed = true;
                yellowSquare.SetFailSquare();
            }
            else
            {
                gridSquares.Add(yellowSquare, 0);
            }

            // Check if yellow square is yellow and flag is captured
            if (yellowSquare.gridType != GridSquare.GridType.Yellow || !yellowFlag.isCaptured)
            {
                win = false;
            }
        }

        if (currentLevel.whiteColor.isUsed)
        {
            int x = whitePlayerCircle.GetComponent<PlayerCircle>().locationX;
            int y = whitePlayerCircle.GetComponent<PlayerCircle>().locationY;

            GridSquare whiteSquare = grid[y, x];

            // Check if white square is in grid squares
            if (gridSquares.ContainsKey(whiteSquare))
            {
                simulationFailed = true;
                whiteSquare.SetFailSquare();
            }
            else
            {
                gridSquares.Add(whiteSquare, 0);
            }

            // Check if white square is white
            if (whiteSquare.gridType != GridSquare.GridType.White)
            {
                win = false;
            }
        }

        // Check if win
        if (win)
        {
            SimulationFailed();
            StartCoroutine(WaitForSimulationSuccess());
            return;
        }

        // Check if simulation failed
        if (simulationFailed)
        {
            SimulationFailed();
            StartCoroutine(WaitForSimulationEnd());
            return;
        }

        firstBeat = !firstBeat;
    }

    private GridSquare MovePlayerCircle(GameObject player)
    {
        // Get player circle
        PlayerCircle playerCircle = player.GetComponent<PlayerCircle>();

        // Get the current grid square
        GridSquare currentGridSquare = grid[playerCircle.locationY, playerCircle.locationX];

        // Get the next grid square
        GridSquare.ArrowType arrowType = currentGridSquare.arrowType;

        // Check if arrow type is none
        if (arrowType == GridSquare.ArrowType.None)
        {
            // Player moves in same direction
            PlayerCircle.MovementDirection movementDirection = playerCircle.currentDirection;

            if (movementDirection == PlayerCircle.MovementDirection.North)
            {
                playerCircle.locationY--;
            }
            else if (movementDirection == PlayerCircle.MovementDirection.East)
            {
                playerCircle.locationX++;
            }
            else if (movementDirection == PlayerCircle.MovementDirection.South)
            {
                playerCircle.locationY++;
            }
            else if (movementDirection == PlayerCircle.MovementDirection.West)
            {
                playerCircle.locationX--;
            }
        }

        else
        {
            // Player moves in direction of arrow
            if (arrowType == GridSquare.ArrowType.North)
            {
                playerCircle.locationY--;
                playerCircle.currentDirection = PlayerCircle.MovementDirection.North;
            }
            else if (arrowType == GridSquare.ArrowType.East)
            {
                playerCircle.locationX++;
                playerCircle.currentDirection = PlayerCircle.MovementDirection.East;
            }
            else if (arrowType == GridSquare.ArrowType.South)
            {
                playerCircle.locationY++;
                playerCircle.currentDirection = PlayerCircle.MovementDirection.South;
            }
            else if (arrowType == GridSquare.ArrowType.West)
            {
                playerCircle.locationX--;
                playerCircle.currentDirection = PlayerCircle.MovementDirection.West;
            }
        }

        // Check if location is in grid
        if (!CheckIfLocationInGrid(playerCircle.locationX, playerCircle.locationY))
        {
            currentGridSquare.SetFailSquare();
            SimulationFailed();
            return currentGridSquare;
        }

        GridSquare newGridSquare = grid[playerCircle.locationY, playerCircle.locationX];

        // Circle sound
        AudioManager.Instance.PlayCircleSound(playerCircle.circleColor);

        // Set parent of circle to new grid square
        player.transform.SetParent(newGridSquare.transform);

        // Reset transform
        playerCircle.ResetTransform();

        // If new grid square is danger then fail
        if (newGridSquare.isDanger)
        {
            newGridSquare.SetFailSquare();
            SimulationFailed();
            return currentGridSquare;
        }

        return currentGridSquare;
    }

    void SimulationFailed()
    {
        simulationFailed = true;
        isSimulationRunning = false;
        controlsPanel.SetActive(false);
    }

    private bool CheckIfLocationInGrid(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return false;
        }

        return true;
    }

    // Toggle button pressed
    public void ToggleSimulation()
    {
        // Toggle simulation
        isSimulationRunning = !isSimulationRunning;

        // Start simulation
        if (isSimulationRunning)
        {
            StartSimulation();
        }

        // Stop simulation
        else
        {
            StopSimulation();
            // Create board
            CreateGrid();
        }
    }

    // Start simulation
    void StartSimulation()
    {
        isSimulationRunning = true;
        firstBeat = true;

        SetButtonToStop();

        // Disable some buttons
        clearButton.GetComponent<Button>().interactable = false;
        levelSelectButton.GetComponent<Button>().interactable = false;

        // Disable grid squares
        foreach (GridSquare gridSquare in grid)
        {
            gridSquare.arrowDisabled = true;
            gridSquare.rotationDisabled = true;
        }

        StartCoroutine(Beats());
    }

    // Wait 1 second
    private IEnumerator WaitForSimulationEnd()
    {
        yield return new WaitForSeconds(1);
        StopSimulation();
        levelFailedPanel.SetActive(true);
    }

    // Stop simulation
    void StopSimulation()
    {
        StopAllCoroutines();

        // Enable some buttons
        clearButton.GetComponent<Button>().interactable = true;
        levelSelectButton.GetComponent<Button>().interactable = true;

        isSimulationRunning = false;
        simulationFailed = false;

        SetButtonToStart();
    }

    // Wait 1 second
    private IEnumerator WaitForSimulationSuccess()
    {
        yield return new WaitForSeconds(1);
        SimulationSuccess();
    }

    // Simulation success
    void SimulationSuccess()
    {
        StopAllCoroutines();

        isSimulationRunning = false;
        simulationFailed = false;

        // Mark level as completed
        currentLevel.levelCompleted = true;

        // Disable start stop button
        controlsPanel.SetActive(false);

        // Enable level completed panel
        levelCompletedPanel.SetActive(true);
    }

    // Load level select screen
    public void LoadLevelSelect()
    {
        StopAllCoroutines();

        // Disable game canvas
        gameCanvas.SetActive(false);

        // Enable level select canvas
        levelSelect.UpdateLevelPage();
        levelSelectPanel.SetActive(true);

        // Disable level completed panel
        levelCompletedPanel.SetActive(false);
    }

    // Replay level
    public void ReplayLevel()
    {
        StopSimulation();

        // Disable level completed panel
        levelCompletedPanel.SetActive(false);

        // Disable level failed panel
        levelFailedPanel.SetActive(false);

        // Create board
        CreateGrid();
    }

    // Next level
    public void NextLevel()
    {
        StopAllCoroutines();

        // Disable level completed panel
        levelCompletedPanel.SetActive(false);

        // Load next level
        gameManager.LoadNextLevel();
    }
}

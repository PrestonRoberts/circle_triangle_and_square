using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Canvases
    public GameObject gameCanvas;
    public GameObject levelSelectCanvas;
    public TMP_Text levelName;

    // Level text colors
    public Color tutorialColor = new Color(0.5f, 0.5f, 0.5f);
    public Color level2Color = new Color(0.5f, 0.5f, 0.5f);
    public Color level3Color = new Color(0.5f, 0.5f, 0.5f);
    public Color level4Color = new Color(0.5f, 0.5f, 0.5f);
    public Color level5Color = new Color(0.5f, 0.5f, 0.5f);

    // Game Grid
    public GameGrid gameGrid;

    // List of levels
    public int totalLevelPages = 5;
    public Level[] tutorialLevels;
    public Level[] levels1;
    public Level[] levels2;
    public Level[] levels3;
    public Level[] levels4;

    // Current level
    public int currentLevelSetIndex = 0;
    public int currentLevelIndex = 0;
    public Level currentLevel;

    // Check if last level
    public bool isLastLevel;
    public GameObject NextLevelButton;

    void Start()
    {
        totalLevelPages = 5;
        currentLevelSetIndex = 0;
        currentLevelIndex = 0;

        // Load level select canvas
        levelSelectCanvas.SetActive(true);
        gameCanvas.SetActive(false);
    }

    public void LoadNextLevel()
    {
        // Get next level
        GetNextLevel();

        // Load level
        LoadLevel();
    }

    void GetNextLevel()
    {
        // Get next level
        currentLevelIndex++;
        if (currentLevelIndex >= GetCurrentLevelSet().Length)
        {
            // Go to next level set
            currentLevelSetIndex++;
            currentLevelIndex = 0;
        }

        // Get current level
        Level[] currentLevelSet = GetCurrentLevelSet();
        currentLevel = currentLevelSet[currentLevelIndex];
    }

    public Level[] GetCurrentLevelSet()
    {
        switch (currentLevelSetIndex)
        {
            case 0:
                return tutorialLevels;
            case 1:
                return levels1;
            case 2:
                return levels2;
            case 3:
                return levels3;
            case 4:
                return levels4;
            default:
                return tutorialLevels;
        }
    }

    public void LoadLevel()
    {
        // Get current level
        currentLevel = GetCurrentLevelSet()[currentLevelIndex];

        // Disable level select canvas
        levelSelectCanvas.SetActive(false);

        // Set level name
        SetLevelName();

        // Enable game canvas
        gameCanvas.SetActive(true);

        // Load level
        gameGrid.LoadGridData(currentLevel);

        // Check if last level
        isLastLevel = currentLevelSetIndex == totalLevelPages - 1 && currentLevelIndex == GetCurrentLevelSet().Length - 1;
        NextLevelButton.SetActive(!isLastLevel);
    }

    void SetLevelName()
    {
        if (currentLevel.levelName != "Level Name")
        {
            // Set level name
            levelName.text = currentLevel.levelName;
        }

        else
        {
            levelName.text = "Level " + currentLevelSetIndex + "-" + (currentLevelIndex + 1);
        }

        // Set level name color
        if (currentLevelSetIndex == 0)
        {
            levelName.color = tutorialColor;
        }
        else if (currentLevelSetIndex == 1)
        {
            levelName.color = level2Color;
        }
        else if (currentLevelSetIndex == 2)
        {
            levelName.color = level3Color;
        }
        else if (currentLevelSetIndex == 3)
        {
            levelName.color = level4Color;
        }
        else if (currentLevelSetIndex == 4)
        {
            levelName.color = level5Color;
        }
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ResetAllLevelProgress()
    {
        // Reset all level progress
        for (int i = 0; i < tutorialLevels.Length; i++)
        {
            tutorialLevels[i].levelCompleted = false;
        }

        for (int i = 0; i < levels1.Length; i++)
        {
            levels1[i].levelCompleted = false;
        }

        for (int i = 0; i < levels2.Length; i++)
        {
            levels2[i].levelCompleted = false;
        }

        for (int i = 0; i < levels3.Length; i++)
        {
            levels3[i].levelCompleted = false;
        }

        for (int i = 0; i < levels4.Length; i++)
        {
            levels4[i].levelCompleted = false;
        }
    }
}

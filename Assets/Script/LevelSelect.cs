using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    public GameManager gameManager;

    // Arrows
    public GameObject leftArrow;
    public GameObject rightArrow;

    private int currentLevelPage = 0;

    public GameObject levelPrefab;
    public TMP_Text levelPageText;

    // Level page sprites
    public Sprite baseSprite;
    public Sprite blue;
    public Sprite red;
    public Sprite green;
    public Sprite yellow;
    public Sprite white;

    // Level text colors
    public Color tutorialColor = new Color(0.5f, 0.5f, 0.5f);
    public Color level2Color = new Color(0.5f, 0.5f, 0.5f);
    public Color level3Color = new Color(0.5f, 0.5f, 0.5f);
    public Color level4Color = new Color(0.5f, 0.5f, 0.5f);
    public Color level5Color = new Color(0.5f, 0.5f, 0.5f);

    // Audio Source
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        currentLevelPage = 0;

        // Load level page
        LoadLevelPage();

        // Get audio source
        audioSource = GetComponent<AudioSource>();
    }

    public void UpdateLevelPage()
    {
        currentLevelPage = gameManager.currentLevelSetIndex;

        LoadLevelPage();
    }

    public void LoadLevelPage()
    {
        // Destroy all level prefabs
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Set level page text
        ChangeLevelPageText();

        // Set arrows
        leftArrow.SetActive(true);
        if (currentLevelPage == 0)
        {
            leftArrow.SetActive(false);
        }

        rightArrow.SetActive(true);
        if (currentLevelPage == gameManager.totalLevelPages - 1)
        {
            rightArrow.SetActive(false);
        }

        // Get list of levels
        Level[] levels = GetLevels(currentLevelPage);

        // Set up level prefabs
        for (int i = 0; i < levels.Length; i++)
        {
            // Set up level prefab
            SetUpLevelPrefab(levels[i], i);
        }
    }

    void SetUpLevelPrefab(Level level, int levelNumber)
    {
        // Instantiate level prefab
        GameObject levelPrefabInstance = Instantiate(levelPrefab, transform);

        // If level completed set sprite to color of page, else set to base
        if (!level.levelCompleted)
        {
            levelPrefabInstance.GetComponent<Image>().sprite = baseSprite;
        }
        else
        {
            switch (currentLevelPage)
            {
                case 0:
                    levelPrefabInstance.GetComponent<Image>().sprite = blue;
                    break;
                case 1:
                    levelPrefabInstance.GetComponent<Image>().sprite = red;
                    break;
                case 2:
                    levelPrefabInstance.GetComponent<Image>().sprite = green;
                    break;
                case 3:
                    levelPrefabInstance.GetComponent<Image>().sprite = yellow;
                    break;
                case 4:
                    levelPrefabInstance.GetComponent<Image>().sprite = white;
                    break;
            }
        }

        // Set level prefab position
        levelPrefabInstance.transform.localPosition = new Vector3(0, 0, 0);

        // Set level prefab scale
        levelPrefabInstance.transform.localScale = new Vector3(1, 1, 1);

        // Set level prefab name
        levelPrefabInstance.name = "Level " + (levelNumber + 1);

        // Set level prefab text
        levelPrefabInstance.GetComponentInChildren<TMP_Text>().text = (levelNumber + 1).ToString();

        // Set level prefab button
        levelPrefabInstance.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelNumber));
    }

    void LoadLevel(int levelNumber)
    {
        // Set current level
        gameManager.currentLevelSetIndex = currentLevelPage;
        gameManager.currentLevelIndex = levelNumber;

        // Load level
        gameManager.LoadLevel();
    }

    Level[] GetLevels(int levelPage)
    {
        // Get list of levels
        switch (levelPage)
        {
            case 0:
                return gameManager.tutorialLevels;
            case 1:
                return gameManager.levels1;
            case 2:
                return gameManager.levels2;
            case 3:
                return gameManager.levels3;
            case 4:
                return gameManager.levels4;
            default:
                return null;
        }
    }

    void ChangeLevelPageText()
    {
        if (currentLevelPage == 0)
        {
            levelPageText.color = tutorialColor;
            levelPageText.text = "Tutorial";
            return;
        }
        else if (currentLevelPage == 1)
        {
            levelPageText.color = level2Color;
        }
        else if (currentLevelPage == 2)
        {
            levelPageText.color = level3Color;
        }
        else if (currentLevelPage == 3)
        {
            levelPageText.color = level4Color;
        }
        else if (currentLevelPage == 4)
        {
            levelPageText.color = level5Color;
        }

        levelPageText.text = "Level " + currentLevelPage;
    }

    public void RightArrow()
    {
        // Load next level page
        currentLevelPage++;
        LoadLevelPage();

        // Play sound
        audioSource.Play();
    }

    public void LeftArrow()
    {
        // Load previous level page
        currentLevelPage--;
        LoadLevelPage();

        // Play sound
        audioSource.Play();
    }
}

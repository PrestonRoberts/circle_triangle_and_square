using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GridSquare : MonoBehaviour, IPointerDownHandler
{
    // Blank grids
    public Sprite baseGrid;
    public Sprite blueGrid;
    public Sprite redGrid;
    public Sprite greenGrid;
    public Sprite yellowGrid;
    public Sprite whiteGrid;

    // Arrow grids
    public Sprite baseArrowGrid;
    public Sprite blueArrowGrid;
    public Sprite redArrowGrid;
    public Sprite greenArrowGrid;
    public Sprite yellowArrowGrid;
    public Sprite whiteArrowGrid;

    // Current image component
    [SerializeField] private GameObject gridImage;
    public Image imageComponent;

    // Grid Data
    public enum GridType
    {
        Base,
        Blue,
        Red,
        Green,
        Yellow,
        White
    }

    public enum ArrowType
    {
        None,
        North,
        East,
        South,
        West
    }

    // Grid type
    public GridType gridType;
    public ArrowType arrowType = ArrowType.None;
    private ArrowType savedArrowType = ArrowType.None;
    private bool isStartingSquare = false;

    // Disable changes
    public bool arrowDisabled = false;
    public bool rotationDisabled = false;

    // Green Danger
    public Sprite greenDanger;
    public bool isDanger = false;

    // Fail Square prefab
    public GameObject failSquarePrefab;

    // Pointer click handler
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !arrowDisabled)
        {
            ToggleArrow();
        }
        else if (eventData.button == PointerEventData.InputButton.Right && !rotationDisabled)
        {
            RotateSquare();
        }
    }

    public void ResetSquare()
    {
        // Set grid type to base
        gridType = GridType.Base;

        // Set arrow type to none
        arrowType = ArrowType.None;
    }

    // Toggle arrow for editor
    public void ToggleArrow()
    {
        if (isStartingSquare) return;
        if (arrowType == ArrowType.None)
        {
            arrowType = ArrowType.North;
            savedArrowType = arrowType;

            // Play place arrow sound
            AudioManager.Instance.PlaceArrowSound();
        }
        else
        {
            arrowType = ArrowType.None;
            savedArrowType = arrowType;

            // Play remove arrow sound
            AudioManager.Instance.RemoveArrowSound();
        }

        UpdateArrowType();
    }

    // Update arrow type
    void UpdateArrowType()
    {
        // Update arrow type based on grid type and if arrow is none
        switch (gridType)
        {
            case GridType.Base:
                if (arrowType == ArrowType.None)
                {
                    imageComponent.sprite = baseGrid;
                }
                else
                {
                    imageComponent.sprite = baseArrowGrid;
                }
                break;
            case GridType.Blue:
                if (arrowType == ArrowType.None)
                {
                    imageComponent.sprite = blueGrid;
                }
                else
                {
                    imageComponent.sprite = blueArrowGrid;
                }
                break;
            case GridType.Red:
                if (arrowType == ArrowType.None)
                {
                    imageComponent.sprite = redGrid;
                }
                else
                {
                    imageComponent.sprite = redArrowGrid;
                }
                break;
            case GridType.Green:
                if (arrowType == ArrowType.None)
                {
                    imageComponent.sprite = greenGrid;
                }
                else
                {
                    imageComponent.sprite = greenArrowGrid;
                }
                break;
            case GridType.Yellow:
                if (arrowType == ArrowType.None)
                {
                    imageComponent.sprite = yellowGrid;
                }
                else
                {
                    imageComponent.sprite = yellowArrowGrid;
                }
                break;
            case GridType.White:
                if (arrowType == ArrowType.None)
                {
                    imageComponent.sprite = whiteGrid;
                }
                else
                {
                    imageComponent.sprite = whiteArrowGrid;
                }
                break;
        }

        SetSquareRotation();
    }

    // Rotate square
    void RotateSquare()
    {
        if (arrowType == ArrowType.None) return;

        switch (arrowType)
        {
            case ArrowType.North:
                arrowType = ArrowType.East;
                break;
            case ArrowType.East:
                arrowType = ArrowType.South;
                break;
            case ArrowType.South:
                arrowType = ArrowType.West;
                break;
            case ArrowType.West:
                arrowType = ArrowType.North;
                break;
        }

        // Play rotate sound
        AudioManager.Instance.RotateSound();

        savedArrowType = arrowType;

        SetSquareRotation();
    }

    void SetSquareRotation()
    {
        if (arrowType == ArrowType.North)
        {
            gridImage.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (arrowType == ArrowType.East)
        {
            gridImage.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (arrowType == ArrowType.South)
        {
            gridImage.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (arrowType == ArrowType.West)
        {
            gridImage.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    // Set starting square
    public void SetStartingSquare()
    {
        isStartingSquare = true;

        // Show arrow
        if (arrowType == ArrowType.None && savedArrowType == ArrowType.None)
        {
            arrowType = ArrowType.North;
            savedArrowType = arrowType;
        }

        UpdateArrowType();
    }

    // Set arrow type
    public void SetArrowType(ArrowType newArrowType)
    {
        arrowType = newArrowType;
        UpdateArrowType();
    }

    // Load square
    public void LoadSquare()
    {
        arrowType = savedArrowType;
        UpdateGridType(gridType);
        UpdateArrowType();
    }

    public void UpdateGridType(GridType newGridType)
    {
        gridType = newGridType;

        switch (gridType)
        {
            case GridType.Base:
                imageComponent.sprite = baseGrid;
                break;
            case GridType.Blue:
                imageComponent.sprite = blueGrid;
                break;
            case GridType.Red:
                imageComponent.sprite = redGrid;
                break;
            case GridType.Green:
                imageComponent.sprite = greenGrid;
                break;
            case GridType.Yellow:
                imageComponent.sprite = yellowGrid;
                break;
            case GridType.White:
                imageComponent.sprite = whiteGrid;
                break;
        }
    }

    // Set danger
    public void SetDanger()
    {
        isDanger = true;

        // Set danger image
        imageComponent.sprite = greenDanger;

        // Reset rotation
        gridImage.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Set fail square
    public void SetFailSquare()
    {
        // Instantiate fail square
        GameObject failSquare = Instantiate(failSquarePrefab, transform.position, Quaternion.identity);

        // Set name
        failSquare.name = "Fail Square";

        // Set parent
        failSquare.transform.SetParent(transform);

        // Set scale
        failSquare.transform.localScale = new Vector3(1, 1, 1);

        // Set position
        failSquare.transform.localPosition = new Vector3(0, 0, 0);

        // Get rect transform
        RectTransform rectTransform = failSquare.GetComponent<RectTransform>();
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    // Remove danger and 
    public void ClearSquare()
    {
        isDanger = false;

        // Remove fail square
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Fail Square")
            {
                Destroy(child.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCircle : MonoBehaviour
{
    // Player colors
    public Sprite blue;
    public Sprite red;
    public Sprite green;
    public Sprite yellow;
    public Sprite white;

    public enum CircleColor
    {
        Blue,
        Red,
        Green,
        Yellow,
        White
    }

    public enum MovementDirection
    {
        North,
        East,
        South,
        West
    }

    public int locationX;
    public int locationY;
    public MovementDirection currentDirection;

    // Image component
    public Image imageComponent;
    public RectTransform rectTransform;

    // Circle color
    public CircleColor circleColor;

    public void ChangePlayerColor(CircleColor newCircleColor)
    {
        switch (newCircleColor)
        {
            case CircleColor.Blue:
                imageComponent.sprite = blue;
                circleColor = CircleColor.Blue;
                break;
            case CircleColor.Red:
                imageComponent.sprite = red;
                circleColor = CircleColor.Red;
                break;
            case CircleColor.Green:
                imageComponent.sprite = green;
                circleColor = CircleColor.Green;
                break;
            case CircleColor.Yellow:
                imageComponent.sprite = yellow;
                circleColor = CircleColor.Yellow;
                break;
            case CircleColor.White:
                imageComponent.sprite = white;
                circleColor = CircleColor.White;
                break;
        }
    }

    public void ResetTransform()
    {
        // Reset rect transform position
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

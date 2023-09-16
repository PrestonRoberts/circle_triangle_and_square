using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointFlag : MonoBehaviour
{
    // Flag colors
    public Sprite blue;
    public Sprite red;
    public Sprite green;
    public Sprite yellow;

    public enum Flagcolor
    {
        Blue,
        Red,
        Green,
        Yellow
    }

    public int locationX;
    public int locationY;

    public bool isCaptured = false;

    // Image component
    public Image imageComponent;

    public void ChangeFlagColor(Flagcolor flagColor)
    {
        switch (flagColor)
        {
            case Flagcolor.Blue:
                imageComponent.sprite = blue;
                break;
            case Flagcolor.Red:
                imageComponent.sprite = red;
                break;
            case Flagcolor.Green:
                imageComponent.sprite = green;
                break;
            case Flagcolor.Yellow:
                imageComponent.sprite = yellow;
                break;
        }
    }

    public void CaptureFlag()
    {
        isCaptured = true;
        imageComponent.enabled = false;
    }

    public void ReleaseFlag()
    {
        isCaptured = false;
        imageComponent.enabled = true;
    }
}

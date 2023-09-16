using UnityEngine;
using System;

[System.Serializable]
[CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/Level")]
public class Level : ScriptableObject
{
    [Serializable]
    public class PlayerColor
    {
        public bool isUsed;
        public int start_x;
        public int start_y;
        public int checkpoint_x;
        public int checkpoint_y;
        public int end_x;
        public int end_y;

        // Constructor to set default values
        public PlayerColor(bool isWhite = false)
        {
            // Set default values here
            isUsed = false;
            start_x = 0;
            start_y = 0;

            if (isWhite)
            {
                checkpoint_x = -1;
                checkpoint_y = -1;
            }
            else
            {
                checkpoint_x = 0;
                checkpoint_y = 0;
            }

            end_x = 0;
            end_y = 0;
        }
    }

    public PlayerColor blueColor = new PlayerColor();
    public PlayerColor redColor = new PlayerColor();
    public PlayerColor greenColor = new PlayerColor();
    public PlayerColor yellowColor = new PlayerColor();
    public PlayerColor whiteColor = new PlayerColor(true);

    // Grid Dimensions
    public int gridWidth;
    public int gridHeight;

    // Level name
    public string levelName = "Level Name";

    // Level completed
    public bool levelCompleted = false;
}

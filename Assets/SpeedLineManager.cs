using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLineManager : MonoBehaviour
{
    public Sprite[] speedLineSprites;
    public SpriteRenderer visibleSpeedLine;
    private int currentLineIndex = -1;

    public void ShowSpeedLine()
    {
        int randomNum = -1;
        do
        {
            randomNum = Random.Range(0, speedLineSprites.Length);
        } while (randomNum == currentLineIndex);

        currentLineIndex = randomNum;

        visibleSpeedLine.sprite = speedLineSprites[currentLineIndex];
    }

    public void HideSpeedLines()
    {
        visibleSpeedLine.sprite = null;
        currentLineIndex = -1;
    }
}

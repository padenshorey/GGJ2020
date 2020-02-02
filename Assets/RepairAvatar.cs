using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairAvatar : MonoBehaviour
{
    public Image myImage;
    public Animator animator;
    public bool isPlayer2 = false;

    public void SetupSprite(Sprite sprite, bool player2)
    {
        myImage.sprite = sprite;
        isPlayer2 = player2;
    }

}

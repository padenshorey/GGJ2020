using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIcon : MonoBehaviour
{
    public Image image;

    public Sprite buttonA;
    public Sprite buttonB;
    public Sprite buttonY;
    public Sprite buttonX;
    public Sprite buttonRB;
    public Sprite buttonLB;

    public Sprite buttonUp;
    public Sprite buttonDown;
    public Sprite buttonLeft;
    public Sprite buttonRight;

    public void SetImage(string button)
    {
        switch(button)
        {
            case "A_":
                image.sprite = buttonA;
                break;
            case "B_":
                image.sprite = buttonB;
                break;
            case "Y_":
                image.sprite = buttonY;
                break;
            case "X_":
                image.sprite = buttonX;
                break;
            case "LB_":
                image.sprite = buttonLB;
                break;
            case "RB_":
                image.sprite = buttonRB;
                break;
            case "Up":
                image.sprite = buttonUp;
                break;
            case "Down":
                image.sprite = buttonDown;
                break;
            case "Left":
                image.sprite = buttonLeft;
                break;
            case "Right":
                image.sprite = buttonRight;
                break;
        }
    }
}

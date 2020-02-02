using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.StartGame();
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject.FindGameObjectWithTag("DroneSprite").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.FindGameObjectWithTag("DividerLine").GetComponent<SpriteRenderer>().enabled = true;
    }

    public void PlaySound()
    {
        //todo - playsound
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.StartGame();
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void PlaySound()
    {
        //todo - playsound
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{

    public AudioManager audioManager;

    public void StartGame()
    {
        GameManager.instance.StartGame();
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject.FindGameObjectWithTag("DroneSprite").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.FindGameObjectWithTag("DividerLine").GetComponent<SpriteRenderer>().enabled = true;

        GameObject[] allCheckmarks = GameObject.FindGameObjectsWithTag("ReadyCheckmark");
        for (int i = 0; i < allCheckmarks.Length; i++)
        {
            allCheckmarks[i].GetComponent<SpriteRenderer>().enabled = false;

        }
    }

    void Start() {
        audioManager = audioManager = FindObjectOfType<AudioManager>();
    }
    public void PlaySound()
    {
        audioManager.PlaySound("beeps");
    }
}

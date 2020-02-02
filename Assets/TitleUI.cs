using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public void ShowPressStart()
    {
       
        GameObject.FindGameObjectWithTag("PressStartUI").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.FindGameObjectWithTag("PressStartUI").GetComponent<Animator>().enabled = true;
    }

    public void PlaySound()
    {
        //todo - playsound
    }
}

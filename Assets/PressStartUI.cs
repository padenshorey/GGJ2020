using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStartUI : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartCircleAnim()
    {
        GameObject.FindGameObjectWithTag("PressStartCircle").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.FindGameObjectWithTag("PressStartCircle").GetComponent<Animator>().enabled = true;
    }

    
}

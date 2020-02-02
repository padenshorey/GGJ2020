using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
public class Countdown : MonoBehaviour
{
    public int timeLeft = 3; //Seconds Overall
    public Text countdown; //UI Text Object
    public bool beginCountdown = false;
    void Start()
    {
        StartCoroutine("LoseTime");
        Time.timeScale = 1; 
    }
    void Update()
    {
        
        
        this.GetComponent<Text>().text = ("" + timeLeft); 

        if(timeLeft == 0) this.GetComponent<Text>().text = ("GO!");
        else if (timeLeft > 3) this.GetComponent<Text>().text = ("3");
        else if (timeLeft < 0)
        {
            this.GetComponent<Text>().text = ("");
            GameManager.instance.CheckGameStart();
            //GameManager.instance.StartAfterCountdown();
        }
           

    }
    //Simple Coroutine
    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
}
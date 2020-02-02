using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOut : MonoBehaviour
{
    public void CheckStartGame()
    {
        GameManager.instance.CheckGameStart();
    }
}

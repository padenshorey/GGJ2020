using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameObject redTeam;
    public GameObject blueTeam;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerWinBanner(int team)
    {
        if (team == 1) redTeam.SetActive(false);
        else blueTeam.SetActive(false);

        animator.SetTrigger("Win");
    }
}

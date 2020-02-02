using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public GameObject introScene;

    public GameObject blueRepaired;
    public GameObject redRepared;

    public GameObject blueToTimeFix;
    public GameObject redToTimeFix;

    private AudioManager audioManager;

    void Start () {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void SpawnCutscene(int team, Enums.Cutscene cutscene)
    {
        GameObject currentCutscene;

        audioManager.PlaySound("wooshes");

        switch (cutscene)
        {
            case Enums.Cutscene.Intro:
                currentCutscene = Instantiate(introScene);
                Destroy(currentCutscene, 3f);
                break;
            case Enums.Cutscene.Repair:
                
                audioManager.PlaySound("goodScreams");

                if(team == 1)
                {
                    currentCutscene = Instantiate(blueRepaired);
                }
                else
                {
                    currentCutscene = Instantiate(redRepared);
                }
                Destroy(currentCutscene, 2.5f);
               
                break;
            case Enums.Cutscene.Broken:
                audioManager.PlaySound("startRepair");
                audioManager.PlaySound("dies");
                audioManager.PlaySound("badScreams");

                if (team == 1)
                {
                    currentCutscene = Instantiate(blueToTimeFix);
                }
                else
                {
                    currentCutscene = Instantiate(redToTimeFix);
                }
                Destroy(currentCutscene, 2.5f);
                //StartCoroutine(IndicatePlayers());
                break;
            default:
                break;
        }
    }

    IEnumerator IndicatePlayers()
    {
        yield return new WaitForSeconds(2.75f);
        foreach(PlayerController pc in GameManager.instance.team1)
        {
            pc.canvasPlayer.GetComponent<RepairAvatar>().animator.SetTrigger("ImHere");
        }

        foreach (PlayerController pc in GameManager.instance.team2)
        {
            pc.canvasPlayer.GetComponent<RepairAvatar>().animator.SetTrigger("ImHere");
        }
    }
}

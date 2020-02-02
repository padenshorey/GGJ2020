using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
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
                break;
            default:
                break;
        }
    }
}

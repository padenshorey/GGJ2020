using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    private float maxY = -0.18f;
    private float minY = -1.667f;

    public GameObject team1Icon;
    public GameObject team2Icon;

    public void ResetDrone()
    {
        team1Icon.transform.position = new Vector3(team1Icon.transform.position.x, minY, team1Icon.transform.position.z);
        team2Icon.transform.position = new Vector3(team2Icon.transform.position.x, minY, team2Icon.transform.position.z);
    }

    public void SetPosition(int team, int currentRound, int maxRounds)
    {
        float percentageOfRaceComplete = (float)currentRound / (float)maxRounds;
        float newYPosition = minY + (percentageOfRaceComplete * Mathf.Abs(minY - maxY));
        if (team == 1)
        {
            team1Icon.transform.localPosition = new Vector3(team1Icon.transform.localPosition.x, newYPosition, team1Icon.transform.localPosition.z);
        }
        else
        {
            team2Icon.transform.localPosition = new Vector3(team1Icon.transform.localPosition.x, newYPosition, team1Icon.transform.localPosition.z);
        }
    }
}

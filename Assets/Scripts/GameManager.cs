using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public PlayerController playerPrefab;
    private AudioManager audioManager;

    private const int maxPlayers = 4;

	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}

	void Start () {
        audioManager = FindObjectOfType<AudioManager>();
    }
	
	void Update () {
        SetupPlayers();
	}

    void StartGame()
    {

    }

    public void EndGame()
    {

    }

    private void SetupPlayers() {
        for (int i = 1; i < maxPlayers + 1; i++)
        {
            if (Input.GetButtonDown("Start_" + i))
            {
                SpawnPlayer(i);
            }
        }
    }

    private void SpawnPlayer(int controllerId) {
        Debug.Log("Spawn Player " + controllerId);
        //PlayerController player = Instantiate(playerPrefab);
        //player.controllerId = controllerId;
    }
}

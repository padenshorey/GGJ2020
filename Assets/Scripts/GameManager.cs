using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public PlayerController playerPrefab;
    private AudioManager audioManager;

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
        if(Input.GetButtonDown("Start_1"))
        {
            SpawnPlayer(1);
        }
        else if (Input.GetButtonDown("Start_2"))
        {
            SpawnPlayer(2);
        }
        else if (Input.GetButtonDown("Start_3"))
        {
            SpawnPlayer(3);
        }
        else if (Input.GetButtonDown("Start_4"))
        {
            SpawnPlayer(4);
        }
    }

    private void SpawnPlayer(int controllerId) {
        PlayerController player = Instantiate(playerPrefab);
        player.controllerId = controllerId;
    }
}

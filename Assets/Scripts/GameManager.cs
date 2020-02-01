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
    private bool _gameInProgress = false;
    public bool GameInProgress { get { return _gameInProgress; } }

    private List<PlayerController> team1 = new List<PlayerController>();
    private List<PlayerController> team2 = new List<PlayerController>();
    private int PlayerCount { get { return team1.Count + team2.Count; } }

    private Game _currentGame;

    [SerializeField]
    private RoundData[] _roundData = null;
    public RoundData[] RoundData { get { return _roundData; } }

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

    // GAME LOOP CODE
    void StartGame()
    {
        Debug.Log("STARTING GAME");
        _currentGame = new Game(_roundData.Length);
    }

    public void EndGame()
    {

    }

    public void CheckGameStart()
    {
        if (PlayerCount < 2) return;

        bool startGame = true;
        
        foreach (PlayerController pc in team1)
        {
            if (!pc.ReadyToPlay) startGame = false;
        }

        foreach (PlayerController pc in team2)
        {
            if (!pc.ReadyToPlay) startGame = false;
        }

        if (startGame) StartGame();
    }

    //PLAYER CODE
    private void SetupPlayers() {
        for (int i = 1; i < maxPlayers + 1; i++)
        {
            if (Input.GetButtonDown("Start_" + i) && !PlayerHasSpawned(i))
            {
                SpawnPlayer(i);
            }
        }
    }

    private bool PlayerHasSpawned(int id)
    {
        foreach(PlayerController pc in team1)
        {
            if (pc.PlayerID == id) return true; 
        }

        foreach (PlayerController pc in team2)
        {
            if (pc.PlayerID == id) return true;
        }

        return false;
    }

    private void SpawnPlayer(int controllerId) {
        Debug.Log("Spawn Player " + controllerId);
        PlayerController player = Instantiate(playerPrefab, transform);
        player.SetupPlayer(controllerId, AssignTeam(player));
    }

    public List<XboxController> GetTeamControllers(int teamId)
    {
        List<XboxController> teamControllers = new List<XboxController>();
        if (teamId == 1)
        {
            foreach(PlayerController pc in team1)
            {
                teamControllers.Add(pc.Controller);
            }
        }
        else
        {
            foreach (PlayerController pc in team2)
            {
                teamControllers.Add(pc.Controller);
            }
        }

        return teamControllers;
    }

    private int AssignTeam(PlayerController pc)
    {
        int totalPlayers = team1.Count + team2.Count;
        if(totalPlayers % 2 == 0)
        {
            team1.Add(pc);
            return 1;
        }
        else
        {
            team2.Add(pc);
            return 2;
        }
    }
}

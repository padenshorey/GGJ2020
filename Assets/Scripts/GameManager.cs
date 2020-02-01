using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public PlayerController playerPrefab;
    private AudioManager audioManager;

    private const float TIME_UNTIL_GAME_RESET = 3;
    private const int MAX_PLAYERS = 4;
    private bool _gameInProgress = false;
    public bool GameInProgress { get { return _gameInProgress; } }

    private List<PlayerController> team1 = new List<PlayerController>();
    private List<PlayerController> team2 = new List<PlayerController>();
    public int PlayerCount { get { return team1.Count + team2.Count; } }
    public int Team1Count { get { return team1.Count; } }
    public int Team2Count { get { return team2.Count; } }
    public Canvas GameCanvas;
    private Game _currentGame;

    [SerializeField]
    private RoundData[] _roundData = null;
    public RoundData[] RoundData { get { return _roundData; } }

    [SerializeField]
    private SprintData[] _sprintData = null;
    public SprintData[] SprintData { get { return _sprintData; } }

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
        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(TIME_UNTIL_GAME_RESET);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CheckGameStart()
    {
        //if (PlayerCount < 2) return; //PUT THIS BACK IN

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
        for (int i = 1; i < MAX_PLAYERS + 1; i++)
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

    public bool CheckAllButtonsUp(XboxController controller)
    {
        if(Input.GetButtonDown(controller.a) ||
           Input.GetButtonDown(controller.b) ||
           Input.GetButtonDown(controller.x) ||
           Input.GetButtonDown(controller.y) ||
           Input.GetButtonDown(controller.rb) ||
           Input.GetButtonDown(controller.lb) ||
           Input.GetAxisRaw(controller.joyRightHori) > 0.1f ||
           Input.GetAxisRaw(controller.joyRightHori) < -0.1f ||
           Input.GetAxisRaw(controller.joyLeftClick) > 0.1f ||
           Input.GetAxisRaw(controller.joyLeftClick) < -0.1f)
        {
            return false;
        }

        return true;
    }
}

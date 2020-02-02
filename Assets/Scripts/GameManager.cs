using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public PlayerController playerPrefab;
    public PlayerController[] players;
    private AudioManager audioManager;
    public AudioManager AudioManager { get { return audioManager; } }
    private CutsceneManager cutsceneManager;
    public CutsceneManager CutsceneManager { get { return cutsceneManager; } }

    private const float TIME_UNTIL_GAME_RESET = 3;
    private const int MAX_PLAYERS = 4;
    private bool _gameInProgress = false;
    public bool GameInProgress { get { return _gameInProgress; } }

    public List<PlayerController> team1 = new List<PlayerController>();
    public List<PlayerController> team2 = new List<PlayerController>();
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

    public InstructionCardFrame[] instructionCardFramesTeam1;
    public InstructionCardFrame[] instructionCardFramesTeam2;

    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}

	void Start () {
        audioManager = FindObjectOfType<AudioManager>();
        cutsceneManager = FindObjectOfType<CutsceneManager>();
    }
	
	void Update () {
        SetupPlayers();
    }

    // GAME LOOP CODE
    public void StartGame()
    {
        //Debug.Log("STARTING GAME");
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
        //if (PlayerCount < 2) return;

        bool startGame = true;
        
        foreach (PlayerController pc in team1)
        {
            if (!pc.ReadyToPlay) startGame = false;
        }

        foreach (PlayerController pc in team2)
        {
            if (!pc.ReadyToPlay) startGame = false;
        }

        if(startGame)
        {
            GameObject pressReadyUI = GameObject.FindGameObjectWithTag("PressStartUI");
            if (pressReadyUI == null)
                Debug.Log("Missing Main Menu UI");
            else
                pressReadyUI.GetComponent<Animator>().Play("hide-pressStart");
            bool isAnimPlaying = pressReadyUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hide-pressStart");

            if (isAnimPlaying)
             {
                Debug.Log("Anim done!");
                GameObject countdownObject = GameObject.FindGameObjectWithTag("Countdown");
                countdownObject.GetComponent<Animator>().enabled = true;
                countdownObject.GetComponent<SpriteRenderer>().enabled = true;
            

            }
           

        }


        //if (startGame) StartGame();
    }

    //PLAYER CODE
    private void SetupPlayers() {
        for (int i = 1; i < MAX_PLAYERS + 1; i++)
        {
            if (Input.GetButtonDown("A_" + i) && !PlayerHasSpawned(i))
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
        if (GameManager.instance.GameInProgress)
        {
            //Debug.Log("You cannot join while the game in in progress");
            return;
        }


        //Debug.Log("Spawn Player with controller " + controllerId);

        GameObject playerLocation = GameObject.FindGameObjectWithTag("Player" + (PlayerCount + 1) + "Location");
        PlayerController player = Instantiate(players[PlayerCount], playerLocation.transform);
        player.transform.localPosition = Vector3.zero;
        player.SetPlayerObject(players[PlayerCount].gameObject);

        playerLocation.GetComponent<SpriteRenderer>().enabled = false;
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

    public void SetInstructionCardSelectedState(InstructionCard ic, int team, PlayerController player)
    {
        bool selectedState = false;
        if(team == 1)
        {
            foreach(PlayerController pc in team1)
            {
                if(pc.ControllerId != player.ControllerId)
                {
                    if(pc.currentSelectedRow == ic.currentInstructionCard.rowId &&
                        pc.currentSelectedColumn == ic.currentInstructionCard.columnId)
                    {
                        selectedState = true;
                    }
                }
            }
        }
        else
        {
            foreach (PlayerController pc in team2)
            {
                if (pc.ControllerId != player.ControllerId)
                {
                    if (pc.currentSelectedRow == ic.currentInstructionCard.rowId &&
                        pc.currentSelectedColumn == ic.currentInstructionCard.columnId)
                    {
                        selectedState = true;
                    }
                }
            }
        }

        ic.isSelected = selectedState;
    }

    public void AssignPlayersToInstructionFrame(int team, List<InstructionCard> instructions)
    {
        if (team == 1)
        {
            foreach (PlayerController pc in team1)
            {
                InstructionCardFrame icf = GetValidInstructionFrame(instructions);
                icf.instructionCard.isSelected = true;
                pc.currentlySelectedInstructionCard = icf.instructionCard;
                pc.currentSelectedColumn = icf.columnId;
                pc.currentSelectedRow = icf.rowId;
                pc.canvasPlayer.transform.position = icf.transform.position;
                pc.canvasPlayer.enabled = true;
                pc.canMoveCanvasPlayer = true;
                pc.AssignCurrentInstructionCards(instructions);
            }
        }
        else
        {
            foreach (PlayerController pc in team2)
            {
                InstructionCardFrame icf = GetValidInstructionFrame(instructions);
                icf.instructionCard.isSelected = true;
                pc.currentlySelectedInstructionCard = icf.instructionCard;
                pc.currentSelectedColumn = icf.columnId;
                pc.currentSelectedRow = icf.rowId;
                pc.canvasPlayer.transform.position = icf.transform.position;
                pc.canvasPlayer.enabled = true;
                pc.canMoveCanvasPlayer = true;
                pc.AssignCurrentInstructionCards(instructions);
            }
        }
    }

    public InstructionCardFrame GetValidInstructionFrame(List<InstructionCard> instructions)
    {
        return instructions[Random.Range(0, instructions.Count)].currentInstructionCard;
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

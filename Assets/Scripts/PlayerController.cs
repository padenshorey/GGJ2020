using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private XboxController _controller;
    public XboxController Controller { get { return _controller; } }
    private int _controllerId;
    public int ControllerId { get { return _controllerId; } }
    private int _playerId;
    public int PlayerID { get { return _playerId; } }
    private SpriteRenderer _sprite;

    private bool _readyToPlay;
    public bool ReadyToPlay { get { return _readyToPlay; } set { _readyToPlay = value; } }

    private int _teamId;

    private GameObject _playerObject;
    public GameObject PlayerObject { get { return _playerObject; } }

    public bool canMoveCanvasPlayer = false;
    public int currentSelectedRow = 0;
    public int currentSelectedColumn = 0;

    public GameObject canvasPlayer;

    private List<InstructionCard> currentInstructionCards;
    public InstructionCard currentlySelectedInstructionCard;

    void Awake()
    {
        _sprite = transform.GetComponentInChildren<SpriteRenderer>();
    }

	void Update ()
    {
        ControllerInput();
    }

    public void AssignCurrentInstructionCards(List<InstructionCard> cards)
    {
        currentInstructionCards = cards;
    }

    public void SetPlayerObject(GameObject player)
    {
        _playerObject = player;
    }

    public void SetupPlayer(int id, int teamId)
    {
        transform.gameObject.name = "Player" + id;
        _controllerId = _playerId = id;
        _teamId = teamId;

        //GameObject myCanvasPlayer = new GameObject();

        GameObject myCanvasPlayerPrefab = Resources.Load("RepairAvatar") as GameObject;
        GameObject myCanvasPlayer = GameObject.Instantiate(myCanvasPlayerPrefab, GameManager.instance.GameCanvas.transform.GetChild(teamId == 1 ? 0 : 1));
        myCanvasPlayer.name = "CanvasPlayer" + id + "_Team" + teamId;
        myCanvasPlayer.GetComponent<RepairAvatar>().SetupSprite(_sprite.sprite, teamId == 1 ? GameManager.instance.team1.Count > 1 : GameManager.instance.team2.Count > 1);
        myCanvasPlayer.SetActive(false);

        canvasPlayer = myCanvasPlayer;

        SetupControls(_controllerId);
    }

    private void SetupControls(int id)
    {
        _controller = new XboxController(id);
    }

    private void ControllerInput()
    {
        //ready up
        if (Input.GetButtonDown(_controller.start) &&
            !_readyToPlay &&
            !GameManager.instance.GameInProgress)
        {
            ReadyUp();
        }else if(canMoveCanvasPlayer)
        {
            if(Input.GetAxis(_controller.joyRightVert) > 0.75f)
            {
                //try to move up
                MoveCanvasPlayerDown();
            }
            else if (Input.GetAxis(_controller.joyRightVert) < -0.75f)
            {
                //try to move down
                MoveCanvasPlayerUp(); 
            }

            if (Input.GetAxis(_controller.joyRightHori) > 0.75f)
            {
                //try to move right
                MoveCanvasPlayerRight();
            }
            else if (Input.GetAxis(_controller.joyRightHori) < -0.75f)
            {
                //try to move left
                MoveCanvasPlayerLeft();
            }
        }
    }

    private void MoveToNewInstructionCard(InstructionCard ic)
    {
        canMoveCanvasPlayer = false;
        if (currentlySelectedInstructionCard)
        {
            GameManager.instance.SetInstructionCardSelectedState(currentlySelectedInstructionCard, _teamId, this);
        }

        currentlySelectedInstructionCard = ic;
        currentlySelectedInstructionCard.isSelected = true;

        canvasPlayer.transform.position = new Vector3(ic.currentInstructionCard.transform.position.x - (canvasPlayer.GetComponent<RepairAvatar>().isPlayer2 ? -1.1f : 1.1f), ic.currentInstructionCard.transform.position.y, ic.currentInstructionCard.transform.position.z);
        currentSelectedRow = ic.currentInstructionCard.rowId;
        currentSelectedColumn = ic.currentInstructionCard.columnId;
        canvasPlayer.GetComponent<RepairAvatar>().animator.SetTrigger("NewCard");
        StartCoroutine(ResetMoveCanvasPlayer());
    }

    IEnumerator ResetMoveCanvasPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        canMoveCanvasPlayer = true;
    }

    private void MoveCanvasPlayerLeft()
    {
        int smallestRowDifference = 99999;
        InstructionCard icToMoveTo = null;
        foreach (InstructionCard ic in currentInstructionCards)
        {
            if (!ic.IsComplete && ic.currentInstructionCard.columnId < currentSelectedColumn && Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId) < smallestRowDifference)
            {
                icToMoveTo = ic;
                smallestRowDifference = Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId);
            }
        }

        if (icToMoveTo)
        {
            MoveToNewInstructionCard(icToMoveTo);
        }
    }

    private void MoveCanvasPlayerRight()
    {
        int smallestRowDifference = 99999;
        InstructionCard icToMoveTo = null;
        foreach (InstructionCard ic in currentInstructionCards)
        {
            if (!ic.IsComplete && ic.currentInstructionCard.columnId > currentSelectedColumn && Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId) < smallestRowDifference)
            {
                icToMoveTo = ic;
                smallestRowDifference = Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId);
            }
        }

        if (icToMoveTo)
        {
            MoveToNewInstructionCard(icToMoveTo);
        }
    }

    private void MoveCanvasPlayerUp(bool checkOtherColumn = false)
    {
        int smallestRowDifference = 99999;
        InstructionCard icToMoveTo = null;
        foreach(InstructionCard ic in currentInstructionCards)
        {
            if(!ic.IsComplete && ic.currentInstructionCard.rowId < currentSelectedRow && Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId) < smallestRowDifference)
            {
                if(checkOtherColumn)
                {
                    icToMoveTo = ic;
                    smallestRowDifference = Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId);
                }
                else if(ic.currentInstructionCard.columnId == currentSelectedColumn)
                {
                    icToMoveTo = ic;
                    smallestRowDifference = Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId);
                }
            }
        }

        if(icToMoveTo)
        {
            MoveToNewInstructionCard(icToMoveTo);
        }else
        {
            if(!checkOtherColumn) MoveCanvasPlayerUp(true);
        }
    }

    private void MoveCanvasPlayerDown(bool checkOtherColumn = false)
    {
        int smallestRowDifference = 99999;
        InstructionCard icToMoveTo = null;
        foreach (InstructionCard ic in currentInstructionCards)
        {
            if (!ic.IsComplete && ic.currentInstructionCard.rowId > currentSelectedRow && Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId) < smallestRowDifference)
            {
                if (checkOtherColumn)
                {
                    icToMoveTo = ic;
                    smallestRowDifference = Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId);
                }
                else if (ic.currentInstructionCard.columnId == currentSelectedColumn)
                {
                    icToMoveTo = ic;
                    smallestRowDifference = Mathf.Abs(currentSelectedRow - ic.currentInstructionCard.rowId);
                }
            }
        }

        if (icToMoveTo)
        {
            MoveToNewInstructionCard(icToMoveTo);
        }
        else
        {
            if (!checkOtherColumn) MoveCanvasPlayerUp(true);
        }
    }

    private void ReadyUp()
    {
        _readyToPlay = true;
        //indicate ready status
        //transform.Rotate(new Vector3(0, 0, 1f), 45);

        SpriteRenderer[] playerSprites = transform.gameObject.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer mysprite in playerSprites)
        {
            if(mysprite.gameObject.tag.Equals( "active-sprite"))
            {
                mysprite.transform.gameObject.tag = "inactive-sprite";
                mysprite.transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            else if( mysprite.gameObject.tag.Equals("inactive-sprite"))
            {
                mysprite.transform.gameObject.tag = "active-sprite";
                mysprite.transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
        }

        GameManager.instance.CheckGameStart();


    }

       
    
}

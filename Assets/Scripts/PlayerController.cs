using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private XboxController _controller;
    public XboxController Controller { get { return _controller; } }
    private int _controllerId;
    private int _playerId;
    public int PlayerID { get { return _playerId; } }
    private SpriteRenderer _sprite;

    private bool _readyToPlay;
    public bool ReadyToPlay { get { return _readyToPlay; } set { _readyToPlay = value; } }

    private int _teamId;
	
    void Awake()
    {
        /*_sprite = GetComponent<SpriteRenderer>();
        if (!_sprite) Debug.LogError("Player " + gameObject.name + " does not have a Sprite Renderer");*/
    }

	void Update ()
    {
        ControllerInput();
    }

    public void SetupPlayer(int id, int teamId)
    {
        transform.gameObject.name = "Player" + id;
        _controllerId = _playerId = id;
        _teamId = teamId;

        SetupControls(_controllerId);
        //assign random color based on team 
       // _sprite.color = teamId == 1 ? new Color(Random.Range(0.5f, 1f), Random.Range(0f, 0.3f), Random.Range(0f, 0.3f)) : new Color(Random.Range(0f, 0.3f), Random.Range(0f, 0.3f), Random.Range(0.5f, 1f));
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

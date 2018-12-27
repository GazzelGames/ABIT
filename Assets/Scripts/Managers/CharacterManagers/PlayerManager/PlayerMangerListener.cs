using System.Collections;
using System.Collections.Generic;
using GameState;
using UnityEngine;

public class PlayerMangerListener : MonoBehaviour
{
    public delegate void PlayerState();

    public delegate void GameState();
    public static event GameState GameStopped;
    public static event GameState GameResumed;
    public static event GameState PlayerDead;

    public StateOfGame stateOf = StateOfGame.GameListening;
    public StateOfGame StateOf
    {
        get { return stateOf; }
        set
        {
            switch (value)
            {
                case StateOfGame.GameListening:
                    {
                        GameResumed();
                        enabled = true;
                        stateOf = value;
                        break;
                    }
                case StateOfGame.PlayerShopping:
                    {
                        GameStopped();
                        enabled = false;
                        stateOf = value;
                        break;
                    }
                case StateOfGame.PlayerTalking:
                    {
                        enabled = false;
                        GameStopped();
                        stateOf = value;
                        break;
                    }
                case StateOfGame.GameTransition:
                    {
                        enabled = false;
                        GameStopped();
                        stateOf = value;
                        break;
                    }
                case StateOfGame.GamePaused:
                    {
                        enabled = false;
                        pauseCanvas.SetActive(true);
                        GameStopped();
                        stateOf = value;
                        break;
                    }
                case StateOfGame.PlayerDead:
                    {
                        PlayerDead();
                        GameStopped();
                        enabled = false;
                        stateOf = value;
                        break;
                    }
            }
        }
    }

    public static PlayerMangerListener instance;
    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool PlayerInvincible { get; set; }
    public bool InScene { get; set; } 
    public bool HasControl { get; set; }
    public bool HasSword { get; set; }
    public Vector3 LastDoor { get; set; }

    void Awake()
    {
        gameObject.transform.position = GameObject.Find("BeginningDialogue").transform.position;
        pauseCanvas = GameObject.Find("AllCanvases").transform.GetChild(4).gameObject;      // you cannont find an object if it is already disabled but you can find its parent and get the object through that
        PlayerInvincible = InScene = false;
        HasSword = true;
        HasControl = true;
        CreateSingleton();
    }

    GameObject pauseCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && stateOf==StateOfGame.GameListening)
        {
            StateOf = StateOfGame.GamePaused;
        }
        /*else if (Input.GetKeyDown(KeyCode.Tab))
        {
            //CanvasManager.instance.AnimationDone = false;
            //mapEvent();
        }*/
    }
}

namespace GameState {
    public enum StateOfGame {PlayerDead, PlayerTalking, PlayerShopping, GameListening,GamePaused,GameTransition }
}


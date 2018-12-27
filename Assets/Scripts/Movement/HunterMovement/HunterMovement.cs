using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class HunterMovement : MonoBehaviour {

    private MovementController2 moveCon;
    private HunterListener hunterListener;
    private GameObject player;

    public Vector3[] positions;
    //this is the starting point from the 
    Vector3 startingPoint = new Vector3(66.5f,32f,0f);

    private bool canSubscribe;
    private bool waitForPlayer;
    //hunter should have a small patrol around his 

    private void Awake()
    {       
        transform.position = startingPoint;
        canSubscribe = true; 
        hunterListener = new HunterListener();
        moveCon = new MovementController2();
        waitForPlayer = false;
        moveCon.Initialize(gameObject, GetComponent<Animator>());
    }

    private void Start()
    {
        player = PlayerController.instance.gameObject;
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy == false && canSubscribe == false)
        {
            hunterListener.Unsubscribe();
            canSubscribe = true;            
        }
    }

    //this needs to be used for movement
    //I need a patrol set up for him
    private void Update()
    {
        moveCon.SetIdleStance();
        moveCon.MoveNPC();
        if (waitForPlayer)
        {
            CheckPlayerDistance();
            if (index <= 3)
            {
                MoveToPoint();
            }
        }
    }

    void CheckPlayerDistance()
    {
        if((player.transform.position-transform.position).magnitude < 1)
        {
            
        }
    }

    int index;
    void MoveToPoint()
    {
        float magnitude = (transform.position - positions[index]).magnitude;
        if (magnitude < 0.3f)
        {
            //if state is true
            if (index<=3)
            {
                index++;
            }
            moveCon.Movement = positions[index];
        }
    }

}

class HunterListener{
    HunterMovement hunterMovement;
    Collider2D collider2D;

    public void SetReferences(HunterMovement hunterMovement, Collider2D collider2D)
    {
        this.hunterMovement = hunterMovement;
        this.collider2D = collider2D;
    }

    //I need a method of doing this 
    public void Subscribe()
    {
        PlayerMangerListener.GameStopped += GameStopped;
        PlayerMangerListener.GameResumed += GameResumed;
    }

    public void Unsubscribe()
    {
        PlayerMangerListener.GameStopped -= GameStopped;
        PlayerMangerListener.GameResumed -= GameResumed;
    }

    void GameStopped()
    {
        hunterMovement.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        hunterMovement.enabled = true;
        collider2D.enabled = true;
    }
}



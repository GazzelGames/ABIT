using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlsMovement : MonoBehaviour {

    Vector3 startingPoint;
    Vector3 destinationPoint;
    public float distanceFromPoint;
    public float waitTime;
    public float range;
    public bool moving;

    MovementController2 moveCon;
    ShopGirlListener girlListener;
    bool canSubscribe;

    private void Awake()
    {
        range = 4;
        distanceFromPoint = 0;
        waitTime = 0;
        startingPoint = transform.position;
        moveCon = new MovementController2();
        girlListener = new ShopGirlListener();
        canSubscribe = true;
        girlListener.SetReferences(GetComponent<GirlsMovement>(), GetComponent<BoxCollider2D>());
        moveCon.Initialize(gameObject, GetComponent<Animator>());
        moveCon.movementSpeed = 4;
        //i need to get the starting position
    }

    private void OnEnable()
    {
        AssignDirection();
        GetComponent<Animator>().enabled = true;
        if (canSubscribe)
        {
            girlListener.Subscribe();
        }
    }

    private void OnDisable()
    {
        GetComponent<Animator>().enabled = false;
        if (gameObject.activeInHierarchy == false)
        {
            girlListener.Unsubscribe();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            moveCon.MoveNPC();
            distanceFromPoint = (transform.position - destinationPoint).magnitude;
        }
        CheckPosition();
    }

    void CheckPosition()
    {
        //i need to check if wait time is over
        //or if i reach the destination
        if (waitTime <= Time.time&&!moving)
        {
            AssignDirection();
        }
        else if(distanceFromPoint<1&&moving)
        {
            waitTime = Random.Range(0.0f, 4.0f)+Time.time;
            moveCon.Movement = Vector3.zero;
            moveCon.MoveNPC();
            moving = false;
        }        
    }

    void AssignDirection()
    {
        moving = true;
        float randomX = Random.Range(-range, range);
        float randomY = Random.Range(-range, range);
        destinationPoint = new Vector3(startingPoint.x + randomX, startingPoint.y + randomY, 0);
        moveCon.Movement = destinationPoint - transform.position;
    }
}

class ShopGirlListener
{
    GirlsMovement girlsMovement;
    BoxCollider2D collider2D;

    public void SetReferences(GirlsMovement girlsMovement, BoxCollider2D collider2D)
    {
        this.girlsMovement = girlsMovement;
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
        girlsMovement.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        girlsMovement.enabled = true;
        collider2D.enabled = true;
    }
}

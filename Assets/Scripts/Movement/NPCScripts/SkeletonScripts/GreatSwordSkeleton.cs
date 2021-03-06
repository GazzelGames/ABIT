﻿using UnityEngine;

public class GreatSwordSkeleton : MonoBehaviour {

    GameObject player;
    Vector3 startingPos;
    // MovementController moveCon;
    public MovementController2 moveCon;
    public PlayerInRange playerInRange;
    public EnemyCombatListener combatListener;
    GreatSkeletonListener skeletonListener;
    bool canSubscribe;
    Animator anim;

    private void Awake()
    {
        canSubscribe = true;
        skeletonListener = new GreatSkeletonListener();
        skeletonListener.SetReferences(this, GetComponent<CapsuleCollider2D>());
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        moveCon = new MovementController2();
        combatListener = GetComponentInChildren<EnemyCombatListener>();
        playerInRange = GetComponentInChildren<PlayerInRange>();
        startingPos = transform.position;
        moveCon.Initialize(gameObject, GetComponent<Animator>());
        moveCon.movementSpeed = 5f;
    }

    private void OnEnable()
    {

        Invoke("DelayEnabled", 0.25f);
    }

    void DelayEnabled()
    {
        GetComponent<Animator>().enabled = true;
        if (canSubscribe)
        {
            skeletonListener.Subscribe();

            if (PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GamePaused)
            {
                skeletonListener.GameStopped();
            }
            canSubscribe = false;
        }
        combatListener.Burning = moveCon.SetOnFire;
        combatListener.IceBlock = moveCon.FrozenSolid;
        combatListener.knockBack = moveCon.CreateVector;
    }

    private void OnDisable()
    {
        GetComponent<Animator>().enabled=false;
        if (!gameObject.activeInHierarchy)
        {
            skeletonListener.Unsubscribe();
            canSubscribe = true;
        }
    }
    private void OnDestroy()
    {
        if (!canSubscribe)
        {
            skeletonListener.Unsubscribe();
        }
    }

    // Update is called once per frame
    // update is going to constantly check the ray distance between the player and the obj.
    public string test;
	void Update () {
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 rayDirection = new Vector2(player.transform.position.x - currentPos.x, player.transform.position.y- currentPos.y);
        //debug draw ray is vector 3s only
        //Debug.DrawRay(transform.position, rayDirection, Color.red, 0.01f);
        //physic2d can only use 2d stuff;

        RaycastHit2D hit = Physics2D.Raycast(currentPos, rayDirection);

        if (hit)
        {
            test = hit.collider.name;
            if (test == "Player" && hit.distance <3f)
            {
                anim.SetBool("SwingSword", true);
                moveCon.movementSpeed = 0;
                moveCon.Movement = Vector3.zero;
            }
            if (test == "Player" && hit.distance < 6f)
            {
                moveCon.Movement = rayDirection;
                moveCon.SetIdleStance();
            }
        }
    }

    private void FixedUpdate()
    {
        if(moveCon.movementSpeed != 0)
        {
            if (playerInRange.playerInRange)
            {
                moveCon.Movement = player.transform.position - transform.position;
                moveCon.MoveNPC();
            }
            else
            {
                if ((transform.position - startingPos).magnitude > 0.5)
                {
                    moveCon.Movement = startingPos - transform.position;
                    moveCon.MoveNPC();
                }
                else
                {

                    moveCon.Movement = Vector3.zero;
                    moveCon.anim.SetFloat("IdleStance", 0f);
                    moveCon.anim.SetBool("IsMoving", false);
                }
            }
        }
    }

    void ResetSwordSwing()
    {
        //moveCon.enabled = true;
        moveCon.movementSpeed = 2.5f;
        anim.SetBool("SwingSword", false);
    }
}

class GreatSkeletonListener
{
    GreatSwordSkeleton greatSwordSkeleton;
    Collider2D collider2D;

    public void SetReferences(GreatSwordSkeleton greatSwordSkeleton, Collider2D collider2D)
    {
        this.greatSwordSkeleton = greatSwordSkeleton;
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

    internal void GameStopped()
    {
        greatSwordSkeleton.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        greatSwordSkeleton.enabled = true;
        collider2D.enabled = true;
    }
}
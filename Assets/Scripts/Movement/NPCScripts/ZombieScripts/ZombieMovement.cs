using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour {

    MovementController2 moveCon;
    ZombieListener zombieListener;
    EnemyCombatListener combatListener;
    PlayerInRange playerInRange;
    bool canSubscribe = true;
    //MovementController moveCon;
    float timer;
    float timerRange;
    float timeTracker = 0;
    GameObject player;
    Vector3 startingPoint;
    Vector3[] direction = new Vector3[4];
    void CreateDirection()
    {
        direction[0] = new Vector3(-1, 0, 0);
        direction[1] = new Vector3(1, 0, 0);
        direction[2] = new Vector3(0, 1, 0);
        direction[3] = new Vector3(0, -1, 0);
    }

    private void Awake()
    {
        zombieListener = new ZombieListener();
        zombieListener.SetReferences(this, GetComponent<CapsuleCollider2D>());
        player = GameObject.FindGameObjectWithTag("Player");
        moveCon = new MovementController2();
        combatListener = GetComponentInChildren<EnemyCombatListener>();
        playerInRange = GetComponentInChildren<PlayerInRange>();
        startingPoint = transform.position;
        moveCon.Initialize(gameObject, GetComponent<Animator>());
        moveCon.movementSpeed = 6f;
        CreateDirection();
        if (!playerInRange.playerInRange)
        {
            //GetComponent<ZombieMovement>().enabled = false;
        }
    }

    private void OnEnable()
    {
        GetComponent<Animator>().enabled = true;
        ChangeDirection();
        if (canSubscribe)
        {
            zombieListener.Subscribe();
            canSubscribe = false;
        }
        timerRange = 3;
        combatListener.Burning = moveCon.SetOnFire;
        combatListener.IceBlock = moveCon.FrozenSolid;
        combatListener.knockBack = moveCon.CreateVector;
    }

    private void Start()
    {
        if (PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GamePaused)
        {
            zombieListener.GameStopped();
        }
    }

    private void OnDisable()
    {
        GetComponent<Animator>().enabled = false;
        if (!gameObject.activeInHierarchy)
        {
            zombieListener.Unsubscribe();
            canSubscribe = true;
        }
    }

    private void Update()
    {
        if (playerInRange.playerInRange&&!combatListener.OnFire)
        {
            moveCon.Movement = player.transform.position - transform.position;
        }else if (farAway)
        {
            moveCon.Movement = startingPoint - transform.position;
            float range = (transform.position - startingPoint).magnitude;
            if (range < 1)
            {
                farAway = false;
            }
        }else
        {
            if (timer < timeTracker)
            {
                ChangeDirection();
                timer = Random.Range(1, timerRange);
                timeTracker = 0;
            }
            else
            {
                timeTracker += Time.deltaTime;
                moveCon.SetIdleStance();
                LineDetection();
            }
        }

    }

    public bool test;
    void LineDetection()
    {
        Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPoint = new Vector2(transform.position.x + 2 * direction[directionIndex].x, transform.position.y + 2 * direction[directionIndex].y);
        RaycastHit2D hit = Physics2D.Linecast(newPos, newPoint, 1 << LayerMask.NameToLayer("Default"));
        Debug.DrawLine(transform.position, newPoint, Color.red);
        test = hit;
        if (hit)
        {
            timer = Random.Range(2, timerRange);
            timeTracker = 0;
            ChangeDirection();
        }
    }

    private void FixedUpdate()
    {
        if((transform.position - startingPoint).magnitude > 4&&!farAway)
        {
            farAway = true;
        }
        moveCon.MoveNPC();
    }
    public int directionIndex;
    public bool farAway;
    void ChangeDirection()
    {
        if (farAway)
        {
            moveCon.Movement = startingPoint - transform.position;
        }
        else
        {
            directionIndex = Random.Range(0, 3);
            moveCon.Movement = direction[directionIndex];
        }

    }
}

class ZombieListener
{
    ZombieMovement zombieMovement;
    Collider2D collider2D;

    public void SetReferences(ZombieMovement zombieMovement, Collider2D collider2D)
    {
        this.zombieMovement = zombieMovement;
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
        zombieMovement.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        zombieMovement.enabled = true;
        collider2D.enabled = true;
    }
}

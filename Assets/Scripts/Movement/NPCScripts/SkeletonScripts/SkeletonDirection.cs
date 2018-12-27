using UnityEngine;

public class SkeletonDirection : MonoBehaviour {

    //the Combat Listener is a reference to the child component 
    //the movement controller is the class that moves the object 
    EnemyCombatListener combatListener;
    MovementController2 moveCon;
    SkeletonListener skeletonListener;
    float timer;
    float timerRange;
    float timeTracker=0;
    bool canSubscribe;
    
    //these are the different directions that the skeleton can move
    Vector3[] direction = new Vector3[4];
    void CreateDirection()
    {
        direction[0] = new Vector3(-1, 0, 0);
        direction[1] = new Vector3(1, 0, 0);
        direction[2] = new Vector3(0, 1, 0);
        direction[3] = new Vector3(0, -1, 0);
    }
    Vector3 startingPoint;

    private void Awake()
    {
        canSubscribe = true;
        combatListener = GetComponentInChildren<EnemyCombatListener>();
        moveCon = new MovementController2();
        skeletonListener = new SkeletonListener();
        skeletonListener.SetReferences(GetComponent<SkeletonDirection>(), GetComponent<Collider2D>());
        moveCon.Initialize(gameObject, GetComponent<Animator>());
        moveCon.movementSpeed = 3;
        startingPoint = transform.position;
        CreateDirection();
    }
    private void OnEnable()
    {
        transform.position = startingPoint;
        if (canSubscribe)
        {
            skeletonListener.Subscribe();
            canSubscribe = false;
        }
        combatListener.Burning = moveCon.SetOnFire;
        combatListener.IceBlock = moveCon.FrozenSolid;
        combatListener.knockBack = moveCon.CreateVector;
    }
    private void OnDisable()
    {
        moveCon.anim.SetBool("IsMoving", false);
        if (!gameObject.activeInHierarchy)
        {
            skeletonListener.Unsubscribe();
            canSubscribe = true;
        }
    }

    private void Start()
    {
        if (PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GamePaused)
        {
            skeletonListener.GameStopped();
        }
        timerRange = 3;
        ChangeDirection();
    }

    private void Update()
    {        
        if (farAway)
        {
            float range = (transform.position - startingPoint).magnitude;
            if (range < 1)
            {
                farAway = false;
            }
        }
        else
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
            }
            //farAway = moveCon.LineDetection(transform);
        }
    }

    private void FixedUpdate()
    {
        moveCon.MoveNPC();
    }
    bool farAway;
    void ChangeDirection()
    {
        float range = (transform.position - startingPoint).magnitude;
        if (range > 5||farAway)
        {
            farAway = true;
            moveCon.Movement = startingPoint - transform.position;
        }
        else
        {
            int directionIndex = Random.Range(0, 3);
            moveCon.Movement = direction[directionIndex];
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D contact in collision.contacts)
        {
            float deltaX = contact.point.x - transform.position.x;
            float deltaY = contact.point.y - transform.position.y;
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                if (deltaX > 0)
                {
                    moveCon.Movement = Vector3.left;
                }
                else
                {
                    moveCon.Movement = Vector3.right;
                }

                //left or right
            }
            else
            {
                if (deltaY > 0)
                {
                    moveCon.Movement = Vector3.up;
                }
                else
                {
                    moveCon.Movement = Vector3.down;
                }
            }
        }
        ChangeDirection();
    }
}

class SkeletonListener
{
    SkeletonDirection skeletonDirection;
    Collider2D collider2D;

    public void SetReferences(SkeletonDirection skeletonDirection, Collider2D collider2D)
    {
        this.skeletonDirection = skeletonDirection;
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
        skeletonDirection.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        skeletonDirection.enabled = true;
        collider2D.enabled = true;
    }
}
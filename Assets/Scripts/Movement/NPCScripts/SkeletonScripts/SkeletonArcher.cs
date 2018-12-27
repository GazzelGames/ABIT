using UnityEngine;

public class SkeletonArcher : MonoBehaviour {

    //the Combat Listener is a reference to the child component 
    //the movement controller is the class that moves the object 
#pragma warning disable 649
    [SerializeField]
    GameObject arrow;
#pragma warning restore 649
    GameObject player;
    PlayerInRange playerInRange;
    EnemyCombatListener combatListener;
    public SkeletonArcherAiming skeletonArcherAiming;
    ArcherListener archerListener;
    MovementController2 moveCon;


    //these are used to determine how and when to change direction
    float timer;
    float timerRange;
    float timeTracker = 0;
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
    public Vector3 startingPoint;

    private void Awake()
    {
        canSubscribe = true;
        //create and assign all of the references that will be needed
        player = GameObject.Find("Player");
        combatListener = GetComponentInChildren<EnemyCombatListener>();
        playerInRange = GetComponentInChildren<PlayerInRange>();
        moveCon = new MovementController2();
        archerListener = new ArcherListener();
        archerListener.SetReferences(GetComponent<SkeletonArcher>(), GetComponent<Collider2D>());
        skeletonArcherAiming = new SkeletonArcherAiming();

        moveCon.Initialize(gameObject, GetComponent<Animator>());
        skeletonArcherAiming.Initialize(gameObject, GetComponent<Animator>(),ref player);
        moveCon.movementSpeed = 4;

        CreateDirection();
        startingPoint = transform.position;
    }
    private void OnEnable()
    {
        transform.position = startingPoint;
        GetComponent<Animator>().enabled = true;
        if (canSubscribe)
        {
            archerListener.Subscribe();

            canSubscribe = false;
        }
        combatListener.Burning = moveCon.SetOnFire;
        combatListener.IceBlock = moveCon.FrozenSolid;
        combatListener.knockBack = moveCon.CreateVector;
    }
    private void OnDisable()
    {
        GetComponent<Animator>().enabled = false;
        if (!gameObject.activeInHierarchy)
        {
            archerListener.Unsubscribe();
            canSubscribe = true;
        }
    }

    private void Start()
    {
        if (PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GamePaused)
        {
            archerListener.GameStopped();
        }
        timerRange = 3;
        ChangeDirection();
    }

    private void Update()
    {
        if (!skeletonArcherAiming.shootBow)
        {
            if (farAway)
            {
                float range = (transform.position - startingPoint).magnitude;
                ChangeDirection();
                if (range < 2)
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
                }
                farAway = moveCon.LineDetection(transform);
            }
            moveCon.SetIdleStance();
        }
    }

    private void FixedUpdate()
    {
        if (!skeletonArcherAiming.shootBow&&!combatListener.OnFire&&!combatListener.Frozen)
        {
            if (playerInRange.playerInRange)
            {
                if (skeletonArcherAiming.aiming)
                {
                    skeletonArcherAiming.AlignWithPlayer();
                }
                else
                {
                    moveCon.movementSpeed = 3;
                    moveCon.Movement = skeletonArcherAiming.DecidedDirection();
                }


            }
            moveCon.MoveNPC();
        } else if (combatListener.OnFire)
        {
            moveCon.movementSpeed = 5;
            moveCon.MoveNPC();
        } else if (moveCon.ifHit)
        {
            moveCon.MoveNPC();
        }
    }
    public int directionIndex;
    public bool farAway;
    void ChangeDirection()
    {
        float range = (transform.position - startingPoint).magnitude;
        if (range > 10||farAway)
        {
            farAway = true;
            moveCon.Movement = startingPoint - transform.position;
        }
        else
        {
            directionIndex = Random.Range(0, 3);
            moveCon.Movement = direction[directionIndex];
        }

    }

    public void SetShootingFalse()
    {
        skeletonArcherAiming.ShootBowState(false);
        moveCon.movementSpeed = 1;
    }
    public void SpawnArrow()
    {
        Instantiate(arrow,gameObject.transform);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
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

[System.Serializable]
public class SkeletonArcherAiming {

    //i am uncertian how to control with this class with a bool;
    public bool aiming = false;
    public bool shootBow = false;


    //these are the references that i require to know to manipulate the stuff
    private GameObject self;
    private GameObject player;
    private Animator anim;

    public void Initialize(GameObject newSelf, Animator newAnim,ref GameObject player)
    {
        self = newSelf;
        anim = newAnim;
        this.player = player;
    }

    //this function will decided which direction the enemy will move to align with the player
    public Vector3 DecidedDirection()
    {
        aiming = true;
        float deltaX = player.transform.position.x - self.transform.position.x;
        float deltaY = player.transform.position.y - self.transform.position.y;

        if (Mathf.Abs(deltaX) < Mathf.Abs(deltaY))
        {
            if (deltaX > 0)
            {
                return Vector3.right;
            }
            else
            {
                return Vector3.left;
            }
        }
        else
        {
            if (deltaY > 0)
            {
                return Vector3.up;
            }
            else
            {
                return Vector3.down;
            }
        }
    }
    //after the direction is decided pass return a direction to a vector 3 varialbe 

    //this function actually aligns the character with the player.

    public void AlignWithPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - self.transform.position.x) < 0.1f)
        {
            aiming = false;
            if (player.transform.position.y > self.transform.position.y)
            {
                anim.SetFloat("IdleStance", 3);
            }
            else
            {
                anim.SetFloat("IdleStance", 0);
            }
            ShootBowState(true);
        }else if (Mathf.Abs(player.transform.position.y - self.transform.position.y) < 0.1f)
        {
            aiming = false;
            if (player.transform.position.x > self.transform.position.x)
            {
                anim.SetFloat("IdleStance", 2);
            }
            else
            {
                anim.SetFloat("IdleStance", 1);
            }
            ShootBowState(true);
        }
    }
    
    //this fuction actually shoots the bow at the target
    public void ShootBowState(bool state)
    {
        //i need a way to change idle stance to fire towards 
        anim.SetBool("Shooting", state);
        shootBow = state;
    }
}

class ArcherListener
{
    SkeletonArcher skeletonArcher;
    Collider2D collider2D;

    public void SetReferences(SkeletonArcher skeletonArcher, Collider2D collider2D)
    {
        this.skeletonArcher = skeletonArcher;
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
        skeletonArcher.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        skeletonArcher.enabled = true;
        collider2D.enabled = true;
    }
}

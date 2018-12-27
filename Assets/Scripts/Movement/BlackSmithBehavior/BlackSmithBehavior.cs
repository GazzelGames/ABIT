using UnityEngine;

public class BlackSmithBehavior : MonoBehaviour {

    Animator anim;
    Vector3 destination;
    bool movingRight;
    bool canSubscribe=true;

    Vector3 startingPos;
    Vector3[] patrolRoute = new Vector3[2];

    BlackSmithListener smithListener;
    MovementController2 moveCon;

    private void Awake()
    {
        moveCon = new MovementController2();
        smithListener = new BlackSmithListener();
        anim = GetComponent<Animator>();
        smithListener.SetReferences(this, GetComponent<Collider2D>());
        Mathf.Clamp(patrolIndex, 0, 1);
    }

    private void OnEnable()
    {
        anim.enabled = true;
        anim.SetBool("IsMoving",true);
        if (canSubscribe)
        {
            smithListener.Subscribe();
            canSubscribe = false;
        }
    }
    private void OnDisable()
    {
        anim.SetBool("IsMoving", false);
        if (!gameObject.activeInHierarchy)
        {
            smithListener.Unsubscribe();
            canSubscribe = true;
        }
    }

    // Use this for initialization
    void Start () {
        moveCon.Initialize(gameObject, anim);
        startingPos = transform.position;
        patrolRoute[0] = startingPos + new Vector3(5, startingPos.y, 0);
        patrolRoute[1] = startingPos + new Vector3(-5, startingPos.y, 0);
        moveCon.movementSpeed = 2;
	}
	
	// Update is called once per frame
	void Update () {
        moveCon.SetIdleStance();
        PatrolLogic();
        moveCon.MoveNPC();
	}

    public int patrolIndex=0;
    public float distance;
    void PatrolLogic()
    {
        moveCon.Movement = patrolRoute[patrolIndex] - transform.position;
        distance = (transform.position - patrolRoute[patrolIndex]).magnitude;
        if (distance < 0.5f)
        {
            if (patrolIndex == 0)
            {
                patrolIndex++;
                moveCon.Movement = Vector3.left;
            }
            else
            {
                patrolIndex--;
                moveCon.Movement = Vector3.right;
            }

        }
    }
}

class BlackSmithListener
{
    BlackSmithBehavior blackSmith;
    Collider2D collider2D;

    public void SetReferences(BlackSmithBehavior blackSmith, Collider2D collider2D)
    {
        this.blackSmith = blackSmith;
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
        blackSmith.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        blackSmith.enabled = true;
        collider2D.enabled = true;
    }
}

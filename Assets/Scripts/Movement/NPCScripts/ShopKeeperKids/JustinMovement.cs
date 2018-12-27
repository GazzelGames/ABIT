using UnityEngine;

public class JustinMovement : MonoBehaviour {

    Vector3 startingPoint;
    Vector3 destination;
    bool movingRight;
    bool canSubscribe;

    JustinListener justinListener;
    MovementController2 moveCon;

    private void Awake()
    {
        canSubscribe = true;
        movingRight = true;
        justinListener = new JustinListener();
        justinListener.SetReferences(GetComponent<JustinMovement>(),GetComponent<BoxCollider2D>());
        justinListener.Subscribe();
        startingPoint = transform.position;
        destination = startingPoint + new Vector3(30, 0, 0);
        moveCon = new MovementController2();
        moveCon.Initialize(gameObject, GetComponent<Animator>());
        moveCon.movementSpeed = 6;
        //i need to get the starting position
    }

    private void OnEnable()
    {
        GetComponent<Animator>().enabled = true;
        if (canSubscribe)
        {
            justinListener.Subscribe();
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        GetComponent<Animator>().enabled = false;
        if (gameObject.activeInHierarchy==false)
        {
            justinListener.Unsubscribe();
            canSubscribe = true;
        }

    }

    // Update is called once per frame
    void Update () {
        moveCon.MoveNPC();
        moveCon.SetIdleStance();
        CheckDestination();
	}

    void CheckDestination()
    {
        if (movingRight)
        {
            float toDestination = (transform.position - destination).magnitude;
            if (toDestination<1f)
            {
                movingRight = false;
            }
            else
            {
                moveCon.Movement = Vector3.right;
            }
        }
        else
        {
            float toDestination = (transform.position - startingPoint).magnitude;
            if (toDestination < 1f)
            {
                movingRight = true;
            }
            else
            {
                moveCon.Movement = Vector3.left;
            }
        }
    }
}

class JustinListener
{
    JustinMovement justinMovement;
    BoxCollider2D collider2D;

    public void SetReferences(JustinMovement justinMovement, BoxCollider2D collider2D)
    {
        this.justinMovement = justinMovement;
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
        justinMovement.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        justinMovement.enabled = true;
        collider2D.enabled = true;
    }
}
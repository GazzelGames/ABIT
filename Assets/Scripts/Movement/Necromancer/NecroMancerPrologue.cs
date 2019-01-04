using UnityEngine;

public class NecroMancerPrologue : MonoBehaviour {

    MovementController2 moveCon;
    Listener listener;
    Vector3 startPoint;
    Vector3 destination;
    bool canSubscribe;

    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
        startPoint = transform.position;
        destination = new Vector3(startPoint.x, startPoint.y - 20, 0);
        listener = new Listener();
        moveCon = new MovementController2();
        moveCon.Initialize(gameObject, GetComponent<Animator>());
        listener.SetReferences(this, GetComponent<BoxCollider2D>());
        moveDown = true;
        moveCon.movementSpeed = 3;
        moveCon.Movement = destination - transform.position;
        canSubscribe = true;
    }

    private void OnEnable()
    {
        if (canSubscribe)
        {
            listener.Subscribe();
            GetComponent<Animator>().enabled = true;
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy == false)
        {
            listener.Unsubscribe();
            canSubscribe = true;
            GetComponent<Animator>().enabled = false;
        }
    }

	// Update is called once per frame
	void Update () {
        moveCon.MoveNPC();
        CheckPosition();
        if (!NecroMancerManager.instance.NecroMetPlayer)
        {
            if ((player.transform.position - transform.position).magnitude < 5)
            {
                NecroMancerManager.instance.NecroMetPlayer=true;
            }
        }
        if (WorldManager.instance.worldState != WorldState.WorldStateEnum.beginning)
        {
            gameObject.SetActive(false);
        }
    }

    bool moveDown;
    void CheckPosition()
    {
        float magnitude;
        if (moveDown)
        {
            magnitude = (transform.position - destination).magnitude;
            if (magnitude < 1)
            {
                moveCon.Movement = startPoint - transform.position;
                moveDown = false;
            }
        }
        else
        {
            magnitude = (transform.position - startPoint).magnitude;
            if (magnitude < 1)
            {
                moveCon.Movement = destination - transform.position;
                moveDown = true;
            }
        }
    }
}

class Listener
{
    NecroMancerPrologue necroMancerPrologue;
    Collider2D collider2D;

    public void SetReferences(NecroMancerPrologue necroMancerPrologue, Collider2D collider2D)
    {
        this.necroMancerPrologue = necroMancerPrologue;
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
        necroMancerPrologue.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        necroMancerPrologue.enabled = true;
        collider2D.enabled = true;
    }
}

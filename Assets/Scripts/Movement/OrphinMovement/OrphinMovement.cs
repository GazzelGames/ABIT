using System.Collections;
using UnityEngine;

public class OrphinMovement : MonoBehaviour {

    MovementController2 moveCon;
    OrphinMovingListener movingListener;
    float timer;
    [HideInInspector]
    public float timerRange;
    bool canSubscribe;

    private void Awake()
    {
        movingListener = new OrphinMovingListener();
        moveCon = new MovementController2();
        moveCon.Initialize(gameObject, GetComponent<Animator>());
        movingListener.SetReferences(GetComponent<OrphinMovement>(), GetComponent<Collider2D>());
        moveCon.movementSpeed = 3;
        canSubscribe = true;
    }

    private void OnEnable()
    {
        timerRange = 3;

        //ChangeDirection();
        if (canSubscribe)
        {
            movingListener.Subscribe();
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (gameObject.activeInHierarchy == false)
        {
            movingListener.Unsubscribe();
            canSubscribe = true;
        }
    }


    private void Start()
    {
        StartCoroutine(RandomPatrol());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Vector3 currentVec = moveCon.Movement;
            currentVec = new Vector3(currentVec.x * -1, currentVec.y * -1, 0);
            timer = Time.time + 2;
            moveCon.Movement = currentVec;
        }
    }

    private void Update()
    {
        moveCon.SetIdleStance();
        moveCon.MoveNPC();
    }

    IEnumerator RandomPatrol()
    {
        while (true)
        {
            if (timer < Time.time)
            {
                ChangeDirection();
                timer = Time.time + Random.Range(1, timerRange);
            }
            yield return null;
        }
    }

    void ChangeDirection()
    {
        int x = Random.Range(-1, 1);
        int y = Random.Range(-1, 1);
        if(x == 0&& y ==0)
        {
            x = Random.Range(-1, 1);
            y = Random.Range(-1, 1);
        }
        moveCon.Movement = new Vector3(x, y, 0);
    }
}

class OrphinMovingListener
{
    OrphinMovement orphinMovement;
    Collider2D collider2D;

    public void SetReferences(OrphinMovement orphinMovement, Collider2D collider2D)
    {
        this.orphinMovement = orphinMovement;
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
        orphinMovement.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        orphinMovement.enabled = true;
        collider2D.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroShields : MonoBehaviour {

    AudioClip shieldDestroyedSFX;

    //I am going to use object pooling for the particles
    //in a sense i am making my own particle system;
    public List<GameObject> particles;
    public GameObject particle;
    public GameObject parent;

    Transform necroMancer;     //necroMancer Gameobject Reference all I want is the transform
    ShieldListener shieldListener;
    
    //how many sword swings it takes to destroy shield
    int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (value == 1)
            {
                //GetComponent<SpriteFlickering>().enabled = true;
                GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }else if (value <= 0)
            {
                StopAllCoroutines();
                GetComponent<SpriteRenderer>().color = new Color(255, 255,255);
                canHit = false;
                GetComponent<CircleCollider2D>().enabled = false;

                if (necroMancer.GetComponent<NecroMancerBehavior>().necroMancerFunctionality.health < 6)
                {
                    StopAllCoroutines();
                    DestroyParticles();
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public GameObject preFab;
    public int spawnCounter;
    //List<GameObjectList> objList;
    public List<GameObject> gameObjects;
    public bool isPaused = false;
    public bool canSubscribe=true;
    Vector3[] quadReference = new Vector3[4];

    private void Awake()
    {
        shieldDestroyedSFX = Resources.Load<AudioClip>("Audio/ShieldExplosionsfx_exp_short_hard9");
        shieldListener = new ShieldListener();
        necroMancer = transform.parent.parent.gameObject.transform;
        parent = transform.parent.gameObject;
        phaseShift = Random.Range(0, 1f);

        shieldListener.SetReferences(this, GetComponent<Collider2D>());
    }

    void OnEnable () {
        if (particles.Count == 0)
        {
            for (int i = 0; i < 95; i++)
            {
                GameObject preFab = Instantiate(particle, transform.position, transform.rotation) as GameObject;
                particles.Add(preFab);
                preFab.SetActive(false);
            }
        }
        if (canSubscribe)
        {

            shieldListener.Subscribe();
            canSubscribe = false;
            health = 2;

            quadReference = necroMancer.gameObject.GetComponent<NecroMancerBehavior>().necroMancerFunctionality.quad;
            gameObjects = new List<GameObject>();
            StartCoroutine(CreateList());
            PlayerMangerListener.PlayerDead += ResetBattle;
        }
        index = 0;
        StartCoroutine(EmitParticles());
    }

    int index;
    IEnumerator EmitParticles()
    {
        while (true)
        {
            if (index >=90)
            {
                index = 0;
            }
            particles[index].transform.position = transform.position;
            particles[index].SetActive(true);
            index++;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator CreateList()
    {
        float x=0;
        float y=0;
        Vector3 randomPos = Vector3.zero;
        for (int i=0;  i < spawnCounter; i++)
        {
            do
            {
                x = randomPos.x;
                y = randomPos.y;
                //this is the quad
                randomPos = new Vector3(Random.Range(quadReference[2].x+3, quadReference[3].x-3), Random.Range(quadReference[0].y-3, quadReference[1].y+3), 0);
            } while (x == randomPos.x || randomPos.y == y);
            yield return new WaitUntil(() => PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GameListening);
            //yield return new WaitUntil(() => !PlayerMangerListener.instance.IsPaused);
            GameObject gameObj = Instantiate(preFab, randomPos, transform.rotation) as GameObject;
            gameObj.AddComponent<DestroyWithPlayer>();
            gameObj.AddComponent<RemoveFromList>().InitializeVariables(gameObject);
            gameObjects.Add(gameObj);
            yield return new WaitForSeconds(1.5f);
        }
        yield return null;
    }

    public void RemoveListItem(GameObject objReference)
    {
        gameObjects.Remove(objReference);

        if (gameObjects.Count == 0&&HudCanvas.instance.CurrentHP>0)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 230, 0);
            GetComponent<CircleCollider2D>().enabled = true;
            canHit = true;
        }
    }

    bool canHit;
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.name == "SwordHitBox"&&canHit)
        {
            Health -= 1;
        }
    }

    public void DestroyParticles()
    {
        gameObject.SetActive(false);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        foreach (GameObject particle in particles)
        {
            //particles.Remove(particle);
            Destroy(particle);
        }
        particles = new List<GameObject>();
    }

    float phaseShift;
    void Update()
    {
        transform.position += new Vector3(Mathf.Sin(3*Time.time+phaseShift)*Time.deltaTime*3, Mathf.Cos(3*Time.time+phaseShift)*Time.deltaTime*3, 0);
    }

    bool isQuitting;
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDisable()
    {
        AudioSource.PlayClipAtPoint(shieldDestroyedSFX, transform.position);
        if (isQuitting == false)
        {
            parent.GetComponent<SelfShield>().RemoveListItem(gameObject);
        }
        if (!gameObject.activeInHierarchy)
        {
            shieldListener.Unsubscribe();
            canSubscribe = true;
        }
    }

    public void ResetBattle()
    {    
        StopAllCoroutines();
        print("Coroutines are stopped");
        foreach (GameObject obj in particles)
        {
            print("Particle removed");
            Destroy(obj);
        }
        particles = new List<GameObject>();
        shieldListener.Unsubscribe();
        Destroy(gameObject);
        //TestFunction();
        //canSubscribe = true;
       // PlayerMangerListener.PlayerDead -= ResetBattle;*/
    }

    void TestFunction()
    {

    }
}

class ShieldListener
{
    NecroShields necroShields;
    Collider2D collider2D;

    public void SetReferences(NecroShields necroShields, Collider2D collider2D)
    {
        this.necroShields = necroShields;
        this.collider2D = collider2D;
    }

    //I need a method of doing this 
    public void Subscribe()
    {
        PlayerMangerListener.GameStopped += GameStopped;
        PlayerMangerListener.GameResumed += GameResumed;
        if (PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GamePaused)
        {
            GameStopped();
        }
    }

    public void Unsubscribe()
    {
        PlayerMangerListener.GameStopped -= GameStopped;
        PlayerMangerListener.GameResumed -= GameResumed;
    }

    void GameStopped()
    {
        necroShields.enabled = false;
        collider2D.enabled = false;
    }

    void GameResumed()
    {
        necroShields.enabled = true;
        collider2D.enabled = true;
    }
}
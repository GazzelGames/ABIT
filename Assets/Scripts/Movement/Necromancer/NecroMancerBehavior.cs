using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMancerBehavior : MonoBehaviour {

    //audioclips
    public AudioClip spawnShieldSound;
    public AudioClip teleportSound;
    public AudioClip hit;
    private AudioSource audioSource;


    public NecroMancerFunctionality necroMancerFunctionality;
    private NecroMancerListener necroMancerListener;
    private MovementController2 moveCon;
    //private Rigidbody2D rb;
    public Animator anim;
    private GameObject player;
    public Vector3 startingPos;

    //[HideInInspector]
    public bool wasEnabled=false;
    //[HideInInspector]
    public bool canSubscribe = true;
    //[HideInInspector]
    public bool isPaused = false;
    public bool canHit;
    private void Awake()
    {
        startingPos = GameObject.Find("MoutianTop").gameObject.GetComponent<RestartNecroBattle>().necroBossTransform.position;
        anim = GetComponent<Animator>();
        NecroMancerManager.StartFirstBattle += StartCastingShield;
        zombies = new List<GameObject>();
        audioSource = GetComponent<AudioSource>();
        necroMancerListener = new NecroMancerListener();
        necroMancerListener.SetReferences(this, GetComponent<CapsuleCollider2D>(), GetComponent<Animator>());

        canHit = false;
        necroMancerFunctionality.DeclareQuad(GameObject.Find("MoutianTop").gameObject.GetComponent<RestartNecroBattle>().necroBossTransform.position);
        moveCon = new MovementController2
        {
            movementSpeed = 6
        };

        player = GameObject.FindGameObjectWithTag("Player");

        //rb = GetComponent<Rigidbody2D>();
        moveCon.Initialize(gameObject, anim);
        necroMancerFunctionality.AssignTeleportLocations();
    }

    private void OnApplicationQuit()
    {
        NecroMancerManager.StartFirstBattle -= StartCastingShield;
    }

    private void OnEnable()
    {
        if (canSubscribe)
        {
            necroMancerListener.Subscribe();
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        necroMancerListener.Unsubscribe();
        NecroMancerManager.StartFirstBattle -= StartCastingShield;
    }

    private void OnDestroy()
    {
        NecroMancerManager.StartFirstBattle -= StartCastingShield;
    }

    private void Update()
    {
        moveCon.Movement = player.transform.position - transform.position;
        moveCon.SetIdleStance();
        moveCon.MoveNPC();
    }

    //this allows the necromancer to get hit and damaged
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SwordHitBox" && canHit)
        {
            audioSource.PlayOneShot(hit);
            necroMancerFunctionality.health -= 1;
            //PauseMenuManager.instance.SwordDamage();
            anim.SetBool("IsMoving", false);
            anim.SetTrigger("NecroHit");
            GetComponent<NecroMancerBehavior>().enabled = true;

            moveCon.CreateVector(collision.gameObject.transform.position);
            canHit = false;
            DecideNextAction(necroMancerFunctionality.health);
        }
    }
    
    void DecideNextAction(int i)
    {
        if (i > 4)
        {
            //move back to the beginning and start again.
            GetComponent<NecroMancerBehavior>().enabled = true;
            StartCoroutine(Phase1());
        }else if (i > 2)
        {
            StartCoroutine(Phase2());
            //teleport all over the place spawning a zombie at a time.
        }else if (i > 0)
        {
            StartCoroutine(Phase3());
            //just run at the player
        }
        else
        {
            anim.SetTrigger("EndDialogue");
            NecroMancerManager.instance.FirstEncounterDone = true;
            GetComponent<NecroMancerBehavior>().enabled = false;
        }    
    }

    public GameObject MainShield;
    //these methods are for phase 1 of battle
    public void EndCastingShield()
    {
        audioSource.PlayOneShot(spawnShieldSound);
        Instantiate(MainShield, transform);
        //transform.GetChild(0).gameObject.SetActive(true);   //this recasts the main shield
        GetComponent<CapsuleCollider2D>().enabled = false;   //this turns off the capsule collider so the necromancer cant get hit.

    }

    //this will be run at the end of the animation;
    public void StartCastingShield()
    {
        print("StartCastingShield");
        GetComponent<Animator>().SetTrigger("NecroCastShield");
    }

    //this is phase1 process just 
    IEnumerator Phase1()
    {
        yield return new WaitUntil(() => !moveCon.ifHit&&!isPaused);
        yield return StartCoroutine(Teleport(transform.position,startingPos));        
        StartCastingShield();
        yield return new WaitUntil(() => !isPaused);
        GetComponent<NecroMancerBehavior>().enabled = false;
        yield return null;
    }
    //this is phase2 teleport and spawn zombies

    [HideInInspector]
    public List<GameObject> zombies;
    IEnumerator Phase2()
    {
        GetComponent<NecroMancerBehavior>().enabled = false;
        Vector3[] teleportLocals = new Vector3[4];
        teleportLocals = necroMancerFunctionality.teleportLocations;
        yield return StartCoroutine(Teleport(transform.position, teleportLocals[0]));
        SpawnZombies();
        yield return StartCoroutine(Teleport(transform.position, teleportLocals[1]));
        SpawnZombies();
        yield return StartCoroutine(Teleport(transform.position, teleportLocals[2]));
        SpawnZombies();
        yield return StartCoroutine(Teleport(transform.position, teleportLocals[2]));
        yield return StartCoroutine(PausedTeleport(transform.position, teleportLocals[3]));
        necroMancerFunctionality.AssignTeleportLocations();
        yield return new WaitUntil(() => !isPaused);
        canHit = true;
        yield return null;
    }
    //this will spawn 2 zombies per call

    public void SpawnZombies()
    {
        float offset = 4.5f;
        Vector3 newPos = transform.position + new Vector3(Random.Range(-offset,offset), Random.Range(-offset, offset), 0);
        GameObject one = Instantiate(necroMancerFunctionality.zombie, newPos, transform.rotation);
        zombies.Add(one);
        one.AddComponent<RemoveZombiesFromBehavior>().parent = gameObject;
        one.AddComponent<DestroyWithPlayer>();

        newPos = transform.position + new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0);
        GameObject two = Instantiate(necroMancerFunctionality.zombie, newPos, transform.rotation);
        zombies.Add(two);
        two.AddComponent<RemoveZombiesFromBehavior>().parent = gameObject;
        two.AddComponent<DestroyWithPlayer>();
    }

    IEnumerator Phase3()
    {
        moveCon.movementSpeed = 0;
        yield return new WaitUntil(() => !moveCon.ifHit && !isPaused);
        yield return StartCoroutine(Teleport(transform.position, necroMancerFunctionality.teleportLocations[0]));
        moveCon.movementSpeed = 6;
        canHit = true;
        yield return null;
    }

    //these are for phase 2 of battle
    //this is a simple teleport method that will be used in all of the other stuff.
    IEnumerator Teleport(Vector3 currentLocation, Vector3 nextLocation)
    {
        audioSource.PlayOneShot(teleportSound);
        Instantiate(necroMancerFunctionality.portal, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        anim.SetTrigger("ReturnToIdle");
        yield return new WaitUntil(()=> !isPaused);
        yield return new WaitForSeconds(1f);
        transform.position = nextLocation;
        audioSource.PlayOneShot(teleportSound);
        Instantiate(necroMancerFunctionality.portal, transform.position, transform.rotation);
        yield return new WaitUntil(()=>!isPaused);
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitUntil(() => !isPaused);
    }
    
    public bool loopTest = false;
    public bool loopBreak;
    public int count=0;
    IEnumerator PausedTeleport(Vector3 currentLocation, Vector3 nextLocation)
    {
        audioSource.PlayOneShot(teleportSound);
        Instantiate(necroMancerFunctionality.portal, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;

        yield return new WaitUntil(() => zombies.Count == 0);

        transform.position = nextLocation;
        audioSource.PlayOneShot(teleportSound);
        Instantiate(necroMancerFunctionality.portal, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
        yield return new WaitUntil(() => !isPaused);
        yield return null;
    }

    private void RemoveZombie(List<GameObject> list,GameObject zombie)
    {
        list.Remove(zombie);
        Destroy(zombie, 1);
    }

    //THESE FUNCTIONS ARE USED FOR RESETING ANIMATIONS
    //this resets the GotHit Animation
    void ResetGotHit()
    {
        anim.SetBool("GotHit", false);
    }

    //this resets the attacking Animation
    private void ResetAttacking()
    {
        anim.SetBool("Attacking", false);
    }
}

[System.Serializable]
public class NecroMancerFunctionality {

    public int health = 6;

    public Vector3[] quad;              //this quad is defined at the individuals spawn by raycasts
    public Vector3[] teleportLocations;        //these four points will be used for teleportation
    public GameObject portal;           //this is the portal particles object
    public GameObject shield;           //this is the shild, I will need to reference this better
    float teleportOffset = 7;

    //this needs to change
    public GameObject zombie;                   //zombie prefab

    //this declares the up, down, left, right of the quad
    //the quad is the fighting arena which you fight in
    public void DeclareQuad(Vector3 startingPos)
    {
        //these are the rays that are sent out
        Vector2[] rays;
        rays = new Vector2[4];
        rays[0] = Vector2.up;
        rays[1] = Vector2.down;
        rays[2] = Vector2.left;
        rays[3] = Vector2.right;
        quad = new Vector3[4];

        RaycastHit2D hit;
        Vector2 newOrigin = new Vector2(startingPos.x, startingPos.y);
        for (int i = 0; i < quad.Length; i++)
        {
            hit = Physics2D.Raycast(newOrigin, rays[i]);
            if (hit)
            {               
                quad[i] = new Vector3(hit.point.x, hit.point.y, 0);
            }
        }
        //I need to make a range of values for x and y
    }

    //this was a corroutine priviously
    public void AssignTeleportLocations()
    {
        teleportLocations = new Vector3[4];
        for (int i = 0; i < teleportLocations.Length; i++)
        {
            teleportLocations[i] = new Vector3(Random.Range(quad[2].x + teleportOffset, quad[3].x - teleportOffset), Random.Range(quad[0].y - teleportOffset, quad[1].y + teleportOffset), 0);
        }
    }

}

[System.Serializable]
class NecroMancerListener
{
    public NecroMancerBehavior necroMancerBehavior;
    public Collider2D collider2D;
    public Animator anim;

    public void SetReferences(NecroMancerBehavior necroMancerBehavior, Collider2D collider2D, Animator anim)
    {
        this.necroMancerBehavior = necroMancerBehavior;
        this.collider2D = collider2D;
        this.anim = anim;
    }

    //I need a method of doing this 
    public void Subscribe()
    {
        PlayerMangerListener.GameStopped += GameStopped;
        PlayerMangerListener.GameResumed += GameResumed;

        if (PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GamePaused)
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
        if (necroMancerBehavior.enabled==true&&!necroMancerBehavior.wasEnabled)
        {
            necroMancerBehavior.wasEnabled = true;
            necroMancerBehavior.enabled = false;
        }
        anim.enabled = false;
        necroMancerBehavior.isPaused = true;
    }

    void GameResumed()
    {
        if (necroMancerBehavior.wasEnabled&& !necroMancerBehavior.enabled)
        {
            necroMancerBehavior.wasEnabled = false;
            necroMancerBehavior.enabled = true;
        }
        necroMancerBehavior.isPaused = false;
        anim.enabled = true;
    }
}


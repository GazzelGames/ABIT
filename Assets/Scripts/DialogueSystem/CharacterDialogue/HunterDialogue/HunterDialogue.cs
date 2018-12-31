using System.Collections;
using UnityEngine;

public class HunterDialogue : MonoBehaviour {

    //this is for dialogue for the before the undead attack;
    public int element;
    public NormalDialogue[] dialogue;
    WorldSectionTrigger worldSectionTrigger;

    //this is for the dialogue for after the attack;
    public int attackDialogueElement;
    public NormalDialogue[] zombieAttackDialogue;
    bool showPlayerPoint = true;

    //the parents animator
    private Animator anim;
    bool canSubscribe = true;

    //this is probably 
    public Transform[] movementLocations;

    //this is the starting point from the 
    private Vector3 startingPoint = new Vector3(16.5f, 29.5f, 0f);

    //GameObjects
    //private HunterListener hunterListener;  //this is the hunter listener
    private GameObject player;          //this is the player object
    private GameObject parentHunter;    //this is the parent object you are actually moving
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        parentHunter = transform.parent.gameObject;
        parentHunter.transform.position = startingPoint;
        anim = GetComponentInParent<Animator>();
        element = 0;
        attackDialogueElement = 0;
        canSubscribe = true;
        movingIsDone = false;
        //hunterListener = new HunterListener();
        //hunterListener.SetReferences(this, GetComponent<Collider2D>());
    }

    private void OnEnable()
    {
        Invoke("DelayEnabled", 0.25f);
    }
    void DelayEnabled()
    {
        if (WorldManager.instance.worldState == WorldState.WorldStateEnum.beginning)
        {
            parentHunter.transform.position = startingPoint;
        }
        else if (WorldManager.instance.worldState == WorldState.WorldStateEnum.zombieAttack)
        {
            if (canSubscribe)
            {
                GetComponent<HunterDialogue>().enabled = false;

                //hunterListener.Subscribe();
                parentHunter.transform.position = movementLocations[0].position;     //this places the Hunter infront of Govenor House    
                canSubscribe = false;
            }
        }
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy == false && canSubscribe == false)
        {           
            //hunterListener.Unsubscribe();
            canSubscribe = true;
        }
        anim.SetBool("IsMoving", false);
    }

    //this needs to be used for movement
    //I need a patrol set up for him
    bool movingIsDone;
    private void Update()
    {
        if (!movingIsDone)
        {
          if(WorldManager.instance.worldState == WorldState.WorldStateEnum.zombieAttack)
            {
                MoveNPC();
                MoveToPoint();
                Vector3 vector3 = movementVec;
                vector3.Normalize();
                SetAnimator(vector3.x,vector3.y);
            }
        }  
    }

    int index=0;
    Vector3 movementVec;
    public float movementSpeed = 12;
    void MoveToPoint()
    {
        //float magnitude = (transform.position - positions[index]).magnitude;
        float magnitude = (transform.position - movementLocations[index].position).magnitude;
        if (magnitude < 0.3f)
        {
            //if state is true
            if (index < movementLocations.Length-1)
            {
                index++;
                movementVec = movementLocations[index].position - parentHunter.transform.position;
            }
            else
            {
                movingIsDone = true;
                print("Parent is null");
                parentHunter.transform.parent.gameObject.GetComponent<WorldSectionTrigger>().areaObjects.Remove(parentHunter);
                worldSectionTrigger = GetComponentInParent<WorldSectionTrigger>();
                worldSectionTrigger.areaObjects.Remove(parentHunter);
                //parentHunter.transform.parent = null;

                showPlayerPoint = false;
                GetComponent<HunterDialogue>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = true;
            }

        }
    }

    //Set Animator
    void SetAnimator(float x, float y)
    {
        if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
        {
            anim.SetBool("IsMoving", false);
        }
        else
        {
            anim.SetBool("IsMoving", true);
            anim.SetFloat("Horizontal", x);
            anim.SetFloat("Vertical", y);
        }
    }
    //this is to move the character
    public void MoveNPC()
    {
        Vector3 currentVec = movementVec;
        currentVec.Normalize();
        currentVec = currentVec * movementSpeed;
        parentHunter.transform.Translate(currentVec * Time.deltaTime);
        SetAnimator(currentVec.x, currentVec.y);
    }

    IEnumerator WaitForDialogueFinish()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => !DialogueManager.instance.isActiveAndEnabled);
        GetComponent<HunterDialogue>().enabled = true;
    }

    void GetZombieAttackDialogue()
    {
        switch (attackDialogueElement)
        {
            case 0:
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    DialogueManager.instance.StartTalking(zombieAttackDialogue[attackDialogueElement]);
                    attackDialogueElement++;
                    StartCoroutine(WaitForDialogueFinish());
                    break;
                }
            case 1:
                {
                    DialogueManager.instance.StartTalking(zombieAttackDialogue[attackDialogueElement]);
                    attackDialogueElement++;
                    break;
                }
            case 2:
                {
                    DialogueManager.instance.StartTalking(zombieAttackDialogue[attackDialogueElement]);
                    break;
                }
        }       
    }

    void GetDialogue()
    {
        if (element >= dialogue.Length)
        {
            element = 0;
        }
        DialogueManager.instance.StartTalking(dialogue[element]);
        element++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerDialogueTransmitter.instance.objectReference == null)
        {
            PlayerDialogueTransmitter.instance.objectReference = gameObject;
        }

        if(WorldManager.instance.worldState == WorldState.WorldStateEnum.zombieAttack&&showPlayerPoint)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetZombieAttackDialogue();
            DialogueManager.instance.ClearText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(PlayerDialogueTransmitter.instance.objectReference == gameObject)
        {
            PlayerDialogueTransmitter.instance.objectReference = null;
            DialogueManager.instance.ClearText();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerDialogueTransmitter.instance.objectReference == gameObject)
        {
            if (Input.GetKey(KeyCode.C) && PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GameListening)
            {
                if(WorldManager.instance.worldState == WorldState.WorldStateEnum.zombieAttack)
                {
                    GetZombieAttackDialogue();
                    SetTalkingStance();
                }
                else
                {
                    GetDialogue();
                    SetTalkingStance();
                }
            }
        }
        if(PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GameListening)
        {
            DialogueManager.instance.DisplayTalk();
        }else
        {
            DialogueManager.instance.ClearText();
        }
    }

    private void SetTalkingStance()
    {
        Vector3 displacement = gameObject.transform.parent.position - player.transform.position;
        float x = displacement.x;
        float y = displacement.y;
        float stance = 0;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
            {
                stance = 1;
            }
            else
            {
                stance = 2;
            }
        }
        else
        {
            if (y > 0)
            {
                stance = 0;
            }
            else
            {
                stance = 3;
            }
        }

        anim.SetFloat("IdleStance", stance);
    }

}

class HunterListener
{
    /*
    HunterDialogue hunterDialogue;
    Collider2D collider2D;

    public void SetReferences(HunterDialogue hunterDialogue, Collider2D collider2D)
    {
        this.hunterDialogue = hunterDialogue;
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
        //hunterDialogue.enabled = false;
        //collider2D.enabled = false;
    }

    void GameResumed()
    {
        //hunterDialogue.enabled = true;
        //collider2D.enabled = true;
    }*/
}
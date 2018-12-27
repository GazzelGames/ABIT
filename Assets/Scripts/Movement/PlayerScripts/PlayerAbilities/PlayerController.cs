using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    PlayerListener playerListener;
    AudioClip swordSwing;
    public float playerSpeed;
    public float x, y;
    Animator anim;
    bool canSubscribe;

    public float horizontalDirection;
    public float verticalDirection;
    public bool hasControl;

    public float idleStance;
    private void SetIdleStance(float f)
    {
        idleStance = f;
        anim.SetFloat("IdleStance", f);
    }

    Rigidbody2D rb;
    //this makes the instance
    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //isMoving Property
    bool isMoving = false;
    bool IsMoving { get { return isMoving; } set { isMoving = value; } }

    private void Awake()
    {
        playerListener = new PlayerListener();
        playerListener.SetReferences(GetComponent<PlayerController>());
        hasControl = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        MakeInstance();
        canSubscribe = true;
    }

    private void Start()
    {
        swordSwing = Resources.Load<AudioClip>("Audio/SwordSwing");
    }

    private void OnEnable()
    {
        if (canSubscribe)
        {
            playerListener.Subscribe();
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        rb.velocity = new Vector2(0, 0);    //I dont think this applies
        anim.SetBool("IsMoving", false);    //I dont think this applies
        if (gameObject.activeInHierarchy == false)
        {
            playerListener.Unsubscribe();
            canSubscribe = true;
        }
    }

    private void Update()
    {
        if (isHit)
        {
            KnockBack();
        }
        else
        {
            DecideIdle();
            if (swordSwingDone)
            {
                if (PlayerMangerListener.instance.HasControl)
                {
                    MovePlayer();
                }
                else
                {
                    if (givePlayerMovement)
                    {
                        GivePlayerMovement();
                    }
                    else
                    {
                        SetAnimator(0, 0);
                        SetMoving(0, 0);
                    }
                }
            }
        }
    }

    void DecideIdle()
    {
        if (Input.GetKey(KeyCode.D))
        {
            SetIdleStance(2);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            SetIdleStance(1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            SetIdleStance(0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            SetIdleStance(3);
        }
    }

    public bool swordSwingDone;

    float pixPerUnit = 135;
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PlayerMangerListener.instance.HasSword&&swordSwingDone)
        {
            AudioSource.PlayClipAtPoint(swordSwing, transform.position);
            anim.SetBool("SwordSwing", true);
            swordSwingDone = false;
        }

        Vector3 updatedPos = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(updatedPos.x * pixPerUnit) / pixPerUnit, Mathf.RoundToInt(updatedPos.y * pixPerUnit) / pixPerUnit, 0);
        rb.velocity = Vector2.zero;
    }

    public float test;

    public bool givePlayerMovement;
    public Vector3 giveMovement;
    void GivePlayerMovement()
    {
        //this is trying a new method of movement

        Vector3 currentVec =  giveMovement-transform.position;
        Debug.DrawRay(transform.position, currentVec,Color.red,1);

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, currentVec, currentVec.magnitude);
        test = (giveMovement - transform.position).magnitude;
        if (test < 0.1)
        {
            hasControl = true;
        }
        currentVec.Normalize();

        SetMoving(currentVec.x, currentVec.y);
        SetAnimator(currentVec.x, currentVec.y);

        transform.Translate(currentVec * Time.deltaTime * 3f);
    }
	//moves player
	void MovePlayer(){

        horizontalDirection = (int) Input.GetAxisRaw("Horizontal");
        verticalDirection = (int) Input.GetAxisRaw("Vertical");

        SetMoving(horizontalDirection, verticalDirection);
        SetAnimator(horizontalDirection, verticalDirection);

        Vector3 currentVec = new Vector3(horizontalDirection, verticalDirection, 0);
        currentVec.Normalize();

        transform.Translate(currentVec * Time.deltaTime * playerSpeed);
        //rb.MovePosition(rb.position+new Vector2(currentVec.x, currentVec.y) * Time.deltaTime * playerSpeed);
    }

    float force = 20f;
    public Vector3 knockBack;
    public bool isHit = false;
    void CreateVector(Vector3 hitPoint)
    {
        print("CreateVector");
        isHit = true;
        knockBack = transform.position - hitPoint;
        knockBack.Normalize();
    }
    void KnockBack()
    {
        transform.Translate(knockBack * force * Time.deltaTime);
        force -= 1f;
        if (force <= 0)
        {
            isHit = false;
            knockBack = new Vector3(0, 0, 0);
            force = 20f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="HurtBox" && !PlayerMangerListener.instance.PlayerInvincible)
        {
            HudCanvas.instance.CurrentHP = -collision.gameObject.GetComponent<HurtBox>().damage;
            GetComponent<PlayerSpriteFlickering>().Begin();
            if (HudCanvas.instance.CurrentHP > 0)
            {
                CreateVector(collision.gameObject.transform.position);
            }

        }
    }

    //Set Animator
    private void SetAnimator(float x, float y)
    {
        anim.SetFloat("Horizontal",x);
        anim.SetFloat("Vertical", y);
    }
    //SetMovingBool
    private void SetMoving(float x,float y)
    {
        x=Mathf.Abs(x);
        y=Mathf.Abs(y);
        if ((x + y) != 0)
        {
            IsMoving = true;
            anim.SetBool("IsMoving", IsMoving);
        }
        else if ((x + y) == 0)
        {
            IsMoving = false;
            anim.SetBool("IsMoving", IsMoving);
        }
    }

    //reset sword swing
    public void ResetMovement()
    {
        anim.SetBool("SwordSwing", false);
        swordSwingDone = true;
    }

    void ControllerState()
    {
        GetComponent<PlayerController>().enabled = PlayerMangerListener.instance.HasControl;
    }
}

class PlayerListener
{
    PlayerController playerController;
    //Collider2D collider2D;

    public void SetReferences(PlayerController playerController)
    {
        this.playerController = playerController;
        //this.collider2D = collider2D;
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
        playerController.enabled = false;   
        //collider2D.enabled = false;
    }

    void GameResumed()
    {
        playerController.enabled = true;
        //collider2D.enabled = true;
    }
}
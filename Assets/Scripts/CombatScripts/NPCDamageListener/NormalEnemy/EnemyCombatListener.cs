using UnityEngine;

public class EnemyCombatListener : MonoBehaviour {

    public delegate void EnemyStatus(bool state);
    public EnemyStatus Burning;
    public EnemyStatus IceBlock;

    public delegate void KnockBack(Vector3 state);
    public KnockBack knockBack;

    //these are the references to the parent object
    GameObject parent;

    //these are the references to the children objects
    GameObject fireStatus;
    GameObject frozenStatus;

    AudioClip hurtSound;
    AudioClip deathSound;

    private bool onFire;
    public bool OnFire
    {
        get { return onFire; }
        set
        {
            if (value)
            {
                if (frozen)
                {
                    Frozen = false;
                    onFire = false;
                } else if (!onFire)
                {
                    onFire = value;
                    DisableHurtBox(false);
                    Burning(onFire);
                    fireStatus.SetActive(true);
                }
                else if (onFire)
                {
                    fireStatus.GetComponent<OnFire>().ResetFireStatus();
                }
            }
            else
            {
                onFire = value;
                Burning(onFire);
                fireStatus.SetActive(false);
                DisableHurtBox(true);
            }
        }
    }

    //if this character has a physical box that hurts the character it disables it during the duration of the spell
    void DisableHurtBox(bool state)
    {
        if (parent.GetComponentInChildren<HurtBox>())
        {
            GameObject hurtBox = parent.GetComponentInChildren<HurtBox>().gameObject;
            hurtBox.SetActive(state);
        }
    }

    private bool frozen;
    public bool Frozen
    {
        get { return frozen; }
        set
        {
            //frozen = value;
            if (value)
            {
                if (onFire)
                {
                    frozen = false;
                    OnFire = false;
                } else if (!frozen)
                {
                    //then spawn status
                    frozen = value;
                    DisableHurtBox(false);
                    frozenStatus.SetActive(true);
                    IceBlock(frozen);
                }
            }else
            {
                frozen = value;
                frozenStatus.SetActive(frozen);
                IceBlock(frozen);
                DisableHurtBox(true);
            }
        }
    }

    public GameObject lootGen;
    private float hp =1;
    [HideInInspector]
    public float maxHp = 1;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (frozen)
            {
                value *= 1.5f;
            }

            hp = value;
            if (hp <= 0)
            {
                Invoke("SpawnItem", 0.2f);
                
            }
            else
            {
                AudioSource.PlayClipAtPoint(hurtSound,transform.position);
            }
        }
    }

    void SpawnItem()
    {
        Instantiate(lootGen, transform.position, transform.rotation, null);
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        parent.SetActive(false);
    }

    //this declares all of the stuff required
    private void Awake()
    {
        parent = transform.parent.gameObject;
        if (gameObject.transform.childCount > 0)
        {
            fireStatus = transform.GetChild(0).gameObject;
            frozenStatus = transform.GetChild(1).gameObject;
        }
        onFire = frozen = false; 
        hp = 2;
    }

    private void Start()
    {
        hurtSound = Resources.Load<AudioClip>("Audio/hit_14");
        deathSound = Resources.Load<AudioClip>("Audio/SkeletonDestroyed");
    }
    //this value stuff is working
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objTag = collision.gameObject.tag;
        switch (objTag)
        {
            case "FireBall":
                {
                    OnFire = true;
                    knockBack(collision.gameObject.transform.position);
                    Destroy(collision.gameObject);
                    break;
                }
            case "IceSpell":
                {
                    //this happens when the character is hit by ice
                    Frozen = true;
                    break;
                }
            case "PlayerSword":
                {
                    //this is how much 
                    if (PauseMenuManager.instance.SwordDamage() == 0)
                    {
                        HP = hp - 1;
                    }
                    else
                    {
                        HP = hp - PauseMenuManager.instance.SwordDamage();
                    }
                    knockBack(collision.gameObject.transform.position);
                    break;
                }
            case "Fire":
                {
                    OnFire = true;
                    break;
                }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string objTag = collision.gameObject.tag;
        switch (objTag)
        {
            case "FireBall":
                {
                    OnFire = true;
                    knockBack(collision.gameObject.transform.position);
                    break;
                }
            case "IceSpell":
                {
                    //this happens when the character is hit by ice
                    Frozen = true;
                    break;
                }
        }
    }

}

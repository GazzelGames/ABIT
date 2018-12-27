using UnityEngine;

[System.Serializable]
public class MovementController2
{ 
    public bool patrol = true;
    private Vector3 movement = new Vector3(0f, 0f, 0f);
    public Vector3 Movement { get { return movement; } set { movement = value; } }
    public float movementSpeed;

    public GameObject self;
    SpriteRenderer parentRender;

    public Animator anim;

    //this assigns the parent object and its animator to this class
    public void Initialize(GameObject newGameObject, Animator newAnimator)
    {
        anim = newAnimator;
        self = newGameObject;
        if (self.GetComponent<SpriteRenderer>())
        {
            parentRender = self.GetComponent<SpriteRenderer>();
        }
    }

    //this continlly checks the movement of the character to determine 
    public void SetIdleStance()
    {        
        if (movement.x > 0.1)
        {
            anim.SetFloat("IdleStance", 2);
        }
        else if (movement.x < -0.1)
        {
            anim.SetFloat("IdleStance", 1);
        }
        else if (movement.y > 0.1)
        {
            anim.SetFloat("IdleStance", 3);
        }
        else if (movement.y < -0.1)
        {
            anim.SetFloat("IdleStance", 0);
        }
    }

    //this is to move the character
    public void MoveNPC()
    {
        if (ifHit)
        {
            KnockBack();
        }
        else
        {
            Vector3 currentVec = Movement;
            currentVec.Normalize();
            currentVec = currentVec * movementSpeed;
            self.transform.Translate(currentVec * Time.deltaTime);
            SetAnimator(currentVec.x, currentVec.y);
        }
    }

    //this is used for creating knockBack for these creatures
    float force = 20f;
    public Vector3 knockBack;
    public bool ifHit=false;
    public void CreateVector(Vector3 hitPoint)
    {
        ifHit = true;
        knockBack = self.transform.position - hitPoint;
        knockBack.Normalize();
    }
    void KnockBack()
    {
        self.transform.Translate(knockBack * force * Time.deltaTime);
        force -= 1f;
        if (force <= 0)
        {
            ifHit = false;
            knockBack = new Vector3(0, 0, 0);
            force = 30f;
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

    //not sure how to implement this
    public void SetOnFire(bool state)
    {
        if (state)
        {
            anim.speed *= 2;
            movementSpeed *= 1.5f;
            parentRender.color = new Color(255, 0, 0);
        }
        else
        {
            anim.speed /= 2;
            movementSpeed /= 1.5f;
            parentRender.color = new Color(255, 255, 255);
        }
    }

    //not sure what to do with this
    public void FrozenSolid(bool state)
    {
        if (state)
        {
            movementSpeed = 0;
            anim.enabled = false;
            parentRender.color = new Color(0, 0, 255);
        }
        else
        {
            anim.enabled = true;
            movementSpeed = 1;
            parentRender.color = new Color(255, 255, 255);
        }
    }

    public bool LineDetection(Transform transform)
    {
        bool wallDetected;
        Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
        //I need to find a way to set directions
        RaycastHit2D hitUp = Physics2D.Linecast(newPos, newPos+new Vector2(0,4), 1 << LayerMask.NameToLayer("Default"));
        RaycastHit2D hitDown = Physics2D.Linecast(newPos, newPos+new Vector2(0, -4), 1 << LayerMask.NameToLayer("Default"));
        RaycastHit2D hitLeft = Physics2D.Linecast(newPos, newPos+new Vector2(-4,0), 1 << LayerMask.NameToLayer("Default"));
        RaycastHit2D hitRight = Physics2D.Linecast(newPos, newPos + new Vector2(4, 0), 1 << LayerMask.NameToLayer("Default"));        
        if (hitUp||hitDown||hitLeft||hitRight)
        {
            wallDetected = true;
        }
        else
        {
            wallDetected = false;
        }

        return wallDetected;
    }
}

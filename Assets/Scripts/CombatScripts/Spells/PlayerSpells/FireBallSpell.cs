using UnityEngine;

public class FireBallSpell : MonoBehaviour {

    AudioSource audioSource;
#pragma warning disable 649
    [SerializeField]
    private float force;
#pragma warning restore 649
    Rigidbody2D rb;
    GameObject parent;
    public GameObject fireStatus;
    public int damage;
    private void Start()
    {
       PlayerMangerListener.instance.HasControl = false;
       Invoke("Invoke", 0.5f);
       parent = transform.parent.gameObject;
       transform.parent = null;
       rb = GetComponent<Rigidbody2D>();
       if (parent.GetComponent<Animator>())
       {
            DecideOrientation();
       }
    }
    private void OnDestroy()
    {
        Invoke();
    }

    void Invoke()
    {
        PlayerMangerListener.instance.HasControl = true;
    }
    void DecideOrientation()
    {
        float f = parent.GetComponent<Animator>().GetFloat("IdleStance");
        if (f == 1)
        {
            rb.AddForce(Vector2.left * force);
            transform.eulerAngles= new Vector3(0, 0, 180);
        }
        else if (f == 2)
        {
            rb.AddForce(Vector2.right * force);
        }
        else if (f == 3)
        {
            rb.AddForce(Vector2.up * force);
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (f == 0)
        {
            rb.AddForce(Vector2.down * force);
            transform.eulerAngles = new Vector3(0, 0, -90);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            Destroy(gameObject);
        }
    }

}

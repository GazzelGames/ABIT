using System.Collections;
using UnityEngine;

public class IceSpellScript : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    private float force;
#pragma warning restore 649

    Rigidbody2D rb;
    GameObject parent;
    public GameObject frozenStatus;

    private void Start()
    {
        PlayerMangerListener.instance.HasControl = false;
        Invoke("Invoke", 0.5f);
        parent = transform.parent.gameObject;
        transform.parent = null;
        StartCoroutine(ScaleMist());
        rb = GetComponent<Rigidbody2D>();

        if (parent.GetComponent<Animator>())
        {
            DecideOrientation();
        }

    }

    void Invoke()
    {
        PlayerMangerListener.instance.HasControl = true;
    }

    void DecideOrientation()
    {
        float f = parent.GetComponent<Animator>().GetFloat("IdleStance");
        if (f==1)
        {
            rb.AddForce(Vector2.left * force);
        }
        else if (f==2)
        {
            rb.AddForce(Vector2.right * force);
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (f==3)
        {
            rb.AddForce(Vector2.up * force);
            transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else if (f==0)
        {
            rb.AddForce(Vector2.down * force);
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    IEnumerator ScaleMist()
    {
        while (true)
        {
            gameObject.transform.localScale += new Vector3(2 * Time.deltaTime, 2 * Time.deltaTime, 0);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        Invoke();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}

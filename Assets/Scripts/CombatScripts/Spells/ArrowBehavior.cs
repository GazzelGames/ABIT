using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    private float force;
    AudioClip shootBowClip;
    [SerializeField]
    private float damage;
#pragma warning restore 649

    Animator anim;

    Rigidbody2D rb;

    private void Start()
    {
        shootBowClip = Resources.Load<AudioClip>("Audio/ShootArrowhit_20");
        AudioSource.PlayClipAtPoint(shootBowClip, transform.position);
        anim = GetComponentInParent<Animator>();
        transform.parent = null;
        rb = GetComponent<Rigidbody2D>();
        DecideOrientation();
    }

    void DecideOrientation()
    {
        rb.velocity = Vector2.zero;
        float f = anim.GetFloat("IdleStance");
        if (f == 1)
        {
            rb.AddForce(Vector2.left * force);
        }
        else if (f == 2)
        {
            rb.AddForce(Vector2.right * force);
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (f == 3)
        {
            rb.AddForce(Vector2.up * force);
            transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else if (f == 0)
        {
            rb.AddForce(Vector2.down * force);
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}

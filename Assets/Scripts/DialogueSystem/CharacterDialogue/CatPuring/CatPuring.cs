using UnityEngine;

public class CatPuring : MonoBehaviour {

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            audioSource.enabled = true;
            audioSource.Play();
            audioSource.loop = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            audioSource.enabled = false;
            audioSource.loop = false;
            audioSource.Stop();
        }

    }

}

using UnityEngine;

public class CuttingBushes : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    Sprite bushDestroyed;
    [SerializeField]
    GameObject lootGenerator;
    AudioClip bushCut;
#pragma warning restore 649

    private void Start()
    {
        bushCut = Resources.Load<AudioClip>("Audio/BushSoundEffect");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerSword")
        {
            Instantiate(lootGenerator, transform.position,transform.rotation);
            AudioSource.PlayClipAtPoint(bushCut, transform.position);
            GetComponent<SpriteRenderer>().sprite = bushDestroyed;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<ParticleSystem>().Play();
        }
    }
}

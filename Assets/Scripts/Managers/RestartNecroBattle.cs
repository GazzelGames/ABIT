using UnityEngine;

public class RestartNecroBattle : MonoBehaviour {

    public Transform necroBossTransform;
    public GameObject aliveObj;
    public GameObject reference;
    Vector3 referenceStartPos;
    private void Awake()
    {
        //reference = necroBoss;
        referenceStartPos = necroBossTransform.position;
        GameObject necro = Instantiate(reference, necroBossTransform);
        aliveObj = necro;
        
        necro.transform.position = referenceStartPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerMangerListener.PlayerDead += DelayRestart;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerMangerListener.PlayerDead -= DelayRestart;
        }
    }

    private void OnDestroy()
    {
        PlayerMangerListener.PlayerDead -= DelayRestart;
    }

    void DelayRestart()
    {
        Invoke("Restart", 1.4f);
    }

    void Restart()
    {
        MusicManager.instance.AreaTag = "MoutianInterior";
        Destroy(aliveObj);
        GameObject necro = Instantiate(reference,necroBossTransform);
        aliveObj = necro;
        necro.transform.position = referenceStartPos;
    }
}

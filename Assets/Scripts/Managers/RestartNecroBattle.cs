using UnityEngine;

public class RestartNecroBattle : MonoBehaviour {

    public Transform necroBoss;
    public GameObject aliveObj;
    public GameObject reference;
    Vector3 referenceStartPos;
    private void Awake()
    {
        //reference = necroBoss;
        referenceStartPos = necroBoss.position;
        GameObject necro = Instantiate(reference, necroBoss);
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
        Destroy(aliveObj);
        GameObject necro = Instantiate(reference,necroBoss);
        aliveObj = necro;
        necro.transform.position = referenceStartPos;
    }
}

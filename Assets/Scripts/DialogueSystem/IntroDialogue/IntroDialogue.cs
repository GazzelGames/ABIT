using UnityEngine;

public class IntroDialogue : MonoBehaviour {

    public NormalDialogue nd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            DialogueManager.instance.StartTalking(nd);
            Destroy(gameObject,1);
        }
    }
}

/*    public GameObject purpleQuinton;
    IEnumerator IntroWallet()
    {
        DialogueManager.instance.StartTalking(introDialogue);
        yield return new WaitUntil(()=>DialogueManager.instance.enabled==false);

        PlayerMangerListener.instance.GameHaulted = true;
        yield return new WaitForSeconds(0.25f);
        GameObject @object = Instantiate(wallet, player.transform);
        player.GetComponent<Animator>().SetBool("VictoryPose", true);
        yield return new WaitUntil(() => @object == null);
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        Instantiate(purpleQuinton, player.transform.position,player.transform.rotation);

        PlayerMangerListener.instance.GameHaulted = false;

        yield return null;
    }*/

using System.Collections;
using UnityEngine;

public class TimePortal : MonoBehaviour {
   
    GameObject necromancer;
    GameObject player;

	// Use this for initialization
	void OnEnable() {
        necromancer = GameObject.Find("NecroMancer(Clone)");
        player = PlayerController.instance.gameObject;
        CameraController.instance.GoToTransformPosition(transform.position);
        CameraController.instance.followPlayer = false;

        StartCoroutine(BringNecroMancerIn());
        StartCoroutine(BringPlayerIn());
	}

    IEnumerator BringNecroMancerIn()
    {
        Vector3 length = transform.position - necromancer.transform.position;
        while (necromancer.transform.localScale.x > 0.1f)
        {
            necromancer.transform.parent = gameObject.transform;
            transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime);

            if((transform.position - necromancer.transform.position).magnitude > 0.1f)
            {
                necromancer.transform.Translate(length * Time.deltaTime * 0.2f);
            }

            necromancer.transform.localScale += new Vector3(-0.1f, -0.1f, 0) * Time.deltaTime;
            yield return null;
        }
        //suck, suck, suck
        yield return null;
    }

    IEnumerator BringPlayerIn()
    {
        yield return new WaitForSeconds(0.5f);
        player.transform.parent = gameObject.transform;
        //suck 

        //this will fade in the fader
        HudCanvas.instance.FadeInFader();

        Vector3 length = transform.position - player.transform.position;
        while (player.transform.localScale.x > 0.5f)
        {
            player.transform.parent = gameObject.transform;
            transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime);

            if ((transform.position - player.transform.position).magnitude > 0.1f)
            {
                player.transform.Translate(length * Time.deltaTime * 0.2f);
            }

            player.transform.localScale += new Vector3(-0.1f, -0.1f, 0) * Time.deltaTime;
            yield return null;
        }
        HudCanvas.instance.GetComponent<Animator>().SetTrigger("EndOfDemo");

        yield return null;
    }

}

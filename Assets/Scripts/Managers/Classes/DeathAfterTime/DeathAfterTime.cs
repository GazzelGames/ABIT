using System.Collections;
using UnityEngine;

public class DeathAfterTime : MonoBehaviour {

    public float deathTime;

	// Use this for initialization
	void Start () {
        StartCoroutine(DeathOnDelay());
	}

    IEnumerator DeathOnDelay()
    {
        yield return new WaitForSeconds(deathTime);
        if (gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }
	
}

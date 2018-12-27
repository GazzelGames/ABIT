using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour {

    float xOffset;
    float yOffset;

	// Use this for initialization
	void Start () {
        xOffset = Random.Range(0.0f, 2f);
        yOffset = Random.Range(0.0f, 2f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0.002f * Mathf.Sin(2 * Time.time + xOffset), 0.002f * Mathf.Cos(Time.time + yOffset), 0);
	}

    private void OnDisable()
    {
        //spawn particle system
    }
}

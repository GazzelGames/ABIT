using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldParticles : MonoBehaviour {

    public Vector3 startingPoint;
    public GameObject end;
    private void Awake()
    {
        end = GameObject.Find("NecroMancer");
    }

    private void OnEnable()
    {
        startingPoint = transform.position;

    }

    public float distanceToEnd;

    private void Update()
    {
        distanceToEnd = (transform.position - end.transform.position).magnitude;

        Vector3 test = end.transform.position - startingPoint;

        test.Normalize();

        transform.Translate((test)*Time.deltaTime*10);

        if (distanceToEnd < 1f)
        {
            gameObject.SetActive(false);
        }
    }

}

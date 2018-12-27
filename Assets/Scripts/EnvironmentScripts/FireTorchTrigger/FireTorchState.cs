using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTorchState : MonoBehaviour {
    
    public delegate void TorchDelegate();
    public event TorchDelegate torchState;

    public bool torchOnFire;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FireBall")
        {
            GetComponent<Animator>().enabled = true;
            torchOnFire = true;
            torchState();
        }
    }

}

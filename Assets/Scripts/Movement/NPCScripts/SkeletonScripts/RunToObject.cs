using UnityEngine;

public class RunToObject : MonoBehaviour {

    public GameObject destinationGameObject;

    MovementController2 moveCon;
    EnemyCombatListener enemyCombatListener;

	// Use this for initialization
	void Start () {
        moveCon = new MovementController2();
        moveCon.Initialize(gameObject, GetComponent<Animator>());
        moveCon.movementSpeed = 5;
        enemyCombatListener = GetComponentInChildren<EnemyCombatListener>();
        enemyCombatListener.Burning = StartFunction;
        enabled = false;
	}
	
    void StartFunction(bool state)
    {
        if (state)
        {
            enabled = state;
            moveCon.Movement = destinationGameObject.transform.position - transform.position;
        }
    }

	// Update is called once per frame
	void Update () {
        moveCon.MoveNPC();
        if ((transform.position - destinationGameObject.transform.position).magnitude < 3)
        {
            enabled = false;
            gameObject.SetActive(false);
        }
	}
}

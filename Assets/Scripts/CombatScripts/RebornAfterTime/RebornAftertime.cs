using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebornAftertime : MonoBehaviour {

    EnemyCombatListener enemyCombat;
    public MoutianDungeonManager dungeonManager;

    Vector3 startingPos;
    private void Awake()
    {
        enemyCombat = GetComponentInChildren<EnemyCombatListener>();
        startingPos = transform.position;
    }

    private void OnDisable()
    {
        if (dungeonManager.playerInMoutian)
        {
            Invoke("Respawn", 4);
        }
    }

    void Respawn()
    {
        enemyCombat.HP = enemyCombat.maxHp;
        transform.position = startingPos;
        gameObject.SetActive(true);
        
    }
}

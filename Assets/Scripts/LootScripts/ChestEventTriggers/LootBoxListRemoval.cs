using UnityEngine;

public class LootBoxListRemoval : MonoBehaviour {

    //there needs to be a specific function that occurs at the end of the combat meaning wehn this target reachs 0 hp

    EnemyCombatListener combatListener;
    private void Awake()
    {
        combatListener = GetComponentInChildren<EnemyCombatListener>();
    }

    bool ifQuit;
    private void OnApplicationQuit()
    {
        ifQuit = true;
    }

    private void OnDisable()
    {        
        if (ifQuit == false&&combatListener.HP<=0)
        {
            GameObject parent = transform.parent.gameObject;
            SpawnByEnemies var = parent.GetComponent<SpawnByEnemies>();
            var.Remove(this.gameObject);
        }
    }
}

using UnityEngine;
using WorldState;

public class WorldManager : MonoBehaviour {

    public static WorldManager instance;
    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    //public WorldStateEnum worldState = WorldStateEnum.beginning;
    public WorldStateEnum worldState;

    private void Awake()
    {
        CreateSingleton();
        worldState = WorldStateEnum.beginning;
    }
}

namespace WorldState
{
    public enum WorldStateEnum {beginning, zombieAttack, necromancerFight1,NA};
}




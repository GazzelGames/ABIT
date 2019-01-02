using UnityEngine;

public class NecroMancerManager : MonoBehaviour {

    //public GameObject necroCharacter;

    public delegate void StartBehavior();
    public static event StartBehavior StartFirstBattle;
    public static event StartBehavior EndFirstBattle;

    public static NecroMancerManager instance;
    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Awake()
    {
        CreateSingleton();
    }

    //this is reference to the first encounter/Battle
    bool firstEncounter;
    public bool FirstEncounter
    {
        get
        {
            return firstEncounter;
        }
        set
        {
            firstEncounter = true;
            if (firstEncounter)
            {
                print("StartBattle has begun");
                StartFirstBattle();
            }
            else
            {
                print("The Bool is reset");
            }

        }        
    }
    bool firstEncounterDone;
    public bool FirstEncounterDone
    {
        get { return firstEncounterDone; }
        set
        {
            firstEncounterDone = value;
            if (value)
            {
                EndFirstBattle();
            }
        }
    }

    public bool necroMetPlayer;
    public bool NecroMetPlayer { get { return necroMetPlayer; } set { necroMetPlayer = value; } }

}

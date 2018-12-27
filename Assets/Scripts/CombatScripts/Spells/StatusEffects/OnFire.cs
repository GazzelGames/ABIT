using UnityEngine;

public class OnFire : MonoBehaviour {

    EnemyCombatListener combatListener;
    bool gamePaused;
    float timeVariable;
    float tickingDamage;
    float tickingTime;

    private void OnEnable()
    {
        combatListener = GetComponentInParent<EnemyCombatListener>();
        tickingDamage = 0.1f;
        tickingTime = 1f;
        changeDirectiontimer = 0;
        timeVariable = 0;
        gamePaused = false;     //this will change if the game is paused
    }

    void PauseGame()
    {
        gamePaused = !gamePaused;
        GetComponent<OnFire>().enabled = gamePaused;
    }

    public void ResetFireStatus()
    {
        timeVariable = 0;
        tickingTime = 0.4f;
    }

    //this keeps a track of the timer and destoys it 
    private void Update()
    {
        if (!gamePaused)
        {
            if (5 >= timeVariable)
            {
                timeVariable = Time.deltaTime + timeVariable;
                if(tickingTime<timeVariable)
                {
                    combatListener.HP -= tickingDamage;
                    tickingTime += tickingTime;
                }
                if (changeDirectiontimer < timeVariable)
                {
                    OnFireMovement();
                }
            }
            else
            {
                combatListener.OnFire = false;
                gameObject.SetActive(false);
            }
        }
    }

    float changeDirectiontimer;
    void OnFireMovement()
    {
        int x = Random.Range(-1, 1);
        int y = Random.Range(-1, 1);
        if (x == 0 && y == 0)
        {
            x = Random.Range(-1, 1);
            y = Random.Range(-1, 1);
        }
        //moveCon.Movement = new Vector3(x, y, 0);
        changeDirectiontimer = changeDirectiontimer + Random.Range(0, 3);
    }

}

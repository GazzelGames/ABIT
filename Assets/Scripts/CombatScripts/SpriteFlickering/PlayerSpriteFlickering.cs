using UnityEngine;

public class PlayerSpriteFlickering : MonoBehaviour {
    SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        wasRunning = false;
        PlayerMangerListener.GameStopped += GameStopped;
        PlayerMangerListener.GameResumed += GameResumed;
    }

    private void OnApplicationQuit()
    {
        PlayerMangerListener.GameStopped -= GameStopped;
        PlayerMangerListener.GameResumed -= GameResumed;
    }

    public void Begin()
    {
        this.enabled = true;
        wasRunning = true;
        otherFrame = true;
        timer = 0;
        PlayerMangerListener.instance.PlayerInvincible = true;
    }

    float timer;
    bool otherFrame;
    private void Update()
    {
        timer += Time.deltaTime;
        sprite.enabled = otherFrame;
        if (otherFrame)
        {
            otherFrame = false;
        }
        else
        {
            otherFrame = true;
        }
        if (timer > 2)
        {
            GetComponent<PlayerSpriteFlickering>().enabled = false;
            PlayerMangerListener.instance.PlayerInvincible = false;
            sprite.enabled = true;
            wasRunning = false;
        }
    }

    bool wasRunning;
    void GameStopped()
    {
        this.enabled = false;
    }

    void GameResumed()
    {
        if (wasRunning)
        {
            this.enabled = true;
        }
    }
}


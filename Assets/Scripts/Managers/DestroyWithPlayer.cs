using UnityEngine;

public class DestroyWithPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerMangerListener.PlayerDead += TimedDestroy;
	}
    
    
    void TimedDestroy()
    {
        Destroy(gameObject);
        PlayerMangerListener.PlayerDead -= TimedDestroy;
    }

    private void OnDestroy()
    {
        PlayerMangerListener.PlayerDead -= TimedDestroy;
    }
}

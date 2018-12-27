using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour {

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(CheckForLoad());
	}
	
    IEnumerator CheckForLoad()
    {
        yield return new WaitForSeconds(0.5f);
        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene("MainMenu");
        yield return null;
    }

}

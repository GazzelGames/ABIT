using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoutianDungeonManager : MonoBehaviour {

    /// <summary>
    /// 
    /// I need to store the keys here
    /// 
    /// 
    /// both sets of doors will be placed 
    /// and will set them to open
    /// 
    /// 
    /// </summary>
    /// 

    public static MoutianDungeonManager instance;
    void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Awake()
    {
        CreateInstance();
    }
    public GameObject keyImage;
    public Text keyCounterText;
    int keyCounter;
    public int KeyCounter
    {
        get { return keyCounter; }
        set
        {
            keyCounter = value;
            keyCounterText.text = keyCounter.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        keyImage.SetActive(true);
        keyCounterText.text = keyCounter.ToString();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        keyImage.SetActive(false);
    }
}

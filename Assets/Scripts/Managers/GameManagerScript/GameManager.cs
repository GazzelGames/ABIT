using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

    //the properties for the audio Modifiers
    public float SFXModifier { get; set; } 
    public float VolumeModifier { get; set; }

    private bool vSync;
    public bool VSync
    {
        get
        {
            return vSync;
        }
        set
        {
            vSync = value;
            if (value)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
        }
    }

    // Use this for initialization
    void Awake () {
        VSync = true;
        Application.targetFrameRate = 30;
        SFXModifier = VolumeModifier = 1f;
		MakeSingleton();
        //set cursor to not be visible
        //Cursor.visible = false;
    }

	//this creates and sustains the singleton
	void MakeSingleton(){
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	//these functions will be used for saving 
	public void SaveGame(){

		//BinaryFormatter bf = new BinaryFormatter();
		//FileStream file = File.Create(Application.persistentDataPath + "/SorusData.EVO");

		//SaveAbleData Data = new SaveAbleData();

		//ClassVar.variable = getdata

		//bf.Serialize(file, ClassVar);

		//file.Close();
	}

	public void LoadGame(){
		if (File.Exists(Application.persistentDataPath + "/SorusData.EVO"))
		{
			//BinaryFormatter bf = new BinaryFormatter();
			//FileStream file = File.Open(Application.persistentDataPath + "/SorusData.EVO", FileMode.Open);
			//className ClassVar = (ClassName)bf.Deserialize(file);
			//file.Close();

			//reassign variable data to appropriate info
		}
	}

}

[Serializable]
public class SaveAbleData{
	//public PlayerStatistics playerStats;
	public WorldManager terisManager;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance;

    public AudioSource townMusic;
    //AudioSource townHomes;

    public AudioSource overWorld;
    public AudioSource witchHome;

    public AudioSource moutianInterior;
    public AudioSource necroMancerIntro;
    public AudioSource necroMancerBattle;

    public AudioSource endOfBattle;

    AudioSource currentMusicPlaying;

    public string areaTag;
    public string AreaTag
    {
        get{ return areaTag; }
        set
        {
            if (value != areaTag)
            {
                switch (value)
                {
                    /*case "HomeInterior":
                        {
                            ChangeMusic(townHomes);
                            break;
                        }*/
                    case "Town":
                        {
                            ChangeMusic(townMusic);
                            break;
                        }
                    case "OverWorld":
                        {
                            ChangeMusic(overWorld);
                            break;
                        }
                    case "WitchHouse":
                        {
                            ChangeMusic(witchHome);
                            break;
                        }
                    case "MoutianInterior":
                        {
                            ChangeMusic(moutianInterior);
                            break;
                        }
                    case "NecroIntro":
                        {
                            ChangeMusic(necroMancerIntro);
                            break;
                        }
                    case "NecroBattle":
                        {
                            ChangeMusic(necroMancerBattle);
                            break;
                        }
                    case "EndOfBattle":
                        {
                            ChangeMusic(endOfBattle);
                            break;
                        }
                }
            }

            areaTag = value;
        }
    }

    private void Awake()
    {
        townMusic = GetComponents<AudioSource>()[0];
        overWorld = GetComponents<AudioSource>()[1];
        witchHome = GetComponents<AudioSource>()[2];
        moutianInterior = GetComponents<AudioSource>()[3];
        necroMancerIntro = GetComponents<AudioSource>()[4];
        necroMancerBattle = GetComponents<AudioSource>()[5];
        endOfBattle = GetComponents<AudioSource>()[6];

        StartCoroutine(InitializeAudioSources());

        /*townMusic = Resources.Load<AudioClip>("Audio/GameMusic/PeacefulVillage18Dec2018");
        //townHomes = Resources.Load<AudioClip>("");
        overWorld = Resources.Load<AudioClip>("Audio/GameMusic/Title");
        witchHome = Resources.Load<AudioClip>("Audio/GameMusic/PyramidsPyramids");
        moutianInterior = Resources.Load<AudioClip>("Audio/GameMusic/YetAnotherJourney");
        necroMancerIntro = Resources.Load<AudioClip>("Audio/GameMusic/NecroMan18Dec2018");
        necroMancerBattle = Resources.Load<AudioClip>("Audio/GameMusic/NecroBattle18Dec2018");*/
        PlayerMangerListener.PlayerDead += StopMusic;
        CreateSingleton();
    }

    IEnumerator InitializeAudioSources()
    {
        townMusic.volume = overWorld.volume = witchHome.volume = moutianInterior.volume = necroMancerIntro.volume = necroMancerBattle.volume = GameManager.instance.VolumeModifier;
        yield return null;
    }

    private void OnApplicationQuit()
    {
        PlayerMangerListener.PlayerDead -= StopMusic;
    }

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

    // Use this for initialization
    void Start () {
        townMusic.Play();
        currentMusicPlaying = townMusic;
        currentMusicPlaying.volume = GameManager.instance.VolumeModifier;
    }

    public void ChangeMusic(AudioSource nextMusic)
    {
        currentMusicPlaying.Stop();
        nextMusic.Play();
        currentMusicPlaying.volume = GameManager.instance.VolumeModifier;
        currentMusicPlaying = nextMusic;
    }

    public void StopMusic()
    {
        currentMusicPlaying.Stop();
    }

    public void StartMusic()
    {
        currentMusicPlaying.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AreaTag = collision.gameObject.tag;
    }

}

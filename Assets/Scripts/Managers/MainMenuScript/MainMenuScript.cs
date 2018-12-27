using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour {

    public EventSystem eventSystem;
    private GameObject priviousButton;

	private List<GameObject> mainPanelButtons;
    private List<GameObject> optionsPanelButton;

    public AudioClip buttonConfirm;
    public AudioClip buttonHighlighted;
    AudioSource audioSource;


    public GameObject mainPanel;
    public GameObject optionsPanel;
    public Slider musicSlider;

    private Animator anim;

	void Awake(){
        mainPanelButtons = new List<GameObject>();
        optionsPanelButton = new List<GameObject>();
		anim = GetComponent<Animator>();
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        for (int i =0; i < mainPanel.transform.childCount; i++)
        {
            mainPanelButtons.Add(mainPanel.transform.GetChild(i).gameObject);
        }
	}

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameObject var = optionsPanel.transform.Find("Changers").gameObject;
        for (int i = 0; i < var.transform.childCount; i++)
        {
            optionsPanelButton.Add(var.transform.GetChild(i).gameObject);
        }
        eventSystem.firstSelectedGameObject =GameObject.Find("StartDemoButton");

        //I need to make the first button the start demo button
    }

    private void PlaySounds(AudioClip audioClip)
    {
        audioSource.volume = GameManager.instance.VolumeModifier;
        audioSource.PlayOneShot(audioClip);
    }

    bool firstButton=false;
    public void OnButtonSelect(GameObject @object)
    {
        Image image = @object.GetComponent<Image>();
        if (firstButton)
        {
            PlaySounds(buttonHighlighted);
        }
        else
        {
            firstButton = true;
        }
        image.enabled = true;
    }

    public void OnButtonDeselect(GameObject @object)
    {
        Image image = @object.GetComponent<Image>();
        image.enabled = false;
    }

	public void StartDemo(){
		anim.SetBool("FadeIn",true);
        PlaySounds(buttonConfirm);
        StartCoroutine(LoadLevel());
	}

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("OverWorld");
        yield return null;
    }

	public void OptionsMenu(){
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(optionsPanelButton[0]);
        PlaySounds(buttonConfirm);
    }
	public void BackToMainFromOptions(){
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        eventSystem.currentSelectedGameObject.GetComponent<Image>().enabled = false;
        eventSystem.SetSelectedGameObject(mainPanelButtons[1]);
        PlaySounds(buttonConfirm);
    }

	public void QuitGame(){
        PlaySounds(buttonConfirm);
        StartCoroutine(QuitProcess());
    }
    IEnumerator QuitProcess()
    {
        anim.SetBool("FadeIn", true);
        yield return new WaitForSeconds(1.2f);

        #if UNITY_STANDALONE_WIN
        Application.Quit();
        #endif
        yield return null;

        /*#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else 
		Application.QuitGame();
        #endif*/
        yield return null;
    }

    public void ChangeVSyncButton(GameObject @object)
    {       
        if (GameManager.instance.VSync)
        {
            GameManager.instance.VSync = false;
            @object.GetComponentInChildren<Text>().text = "Off";
        }
        else
        {
            GameManager.instance.VSync = true;
            @object.GetComponentInChildren<Text>().text = "On";
        }
        PlaySounds(buttonConfirm);
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(priviousButton);
        }else if (priviousButton != eventSystem.currentSelectedGameObject)
        {
            priviousButton = eventSystem.currentSelectedGameObject;
        }

    }
}
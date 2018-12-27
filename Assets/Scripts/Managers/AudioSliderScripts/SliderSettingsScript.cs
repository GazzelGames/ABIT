using UnityEngine;
using UnityEngine.UI;

public class SliderSettingsScript : MonoBehaviour {

    public Slider volumeSlider;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        volumeSlider.value = GameManager.instance.VolumeModifier;
    }

    public void VolumeSlider(){
        GameManager.instance.VolumeModifier = volumeSlider.value;
        audioSource.volume = volumeSlider.value;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionsController : MonoBehaviour
{
    private Slider mVolSlider;
    private Slider fxVolSlider;

    public int fxVolume;
    public int mVolume;

    private MusicComtroler mc;

    // Start is called before the first frame update
    void Start()
    {
        mVolSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        fxVolSlider = GameObject.Find("FXSlider").GetComponent<Slider>();

        mc = GameObject.FindObjectOfType<MusicComtroler>();

        fxVolume = PlayerPrefs.GetInt("FXVolume");
        fxVolSlider.value = fxVolume;

        mVolume = PlayerPrefs.GetInt("MVolume");
        mVolSlider.value = mVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateMVolumeSlider()
    {
        int volumeInt = (int)mVolSlider.value;
        PlayerPrefs.SetInt("MVolumeSet", 1);
        PlayerPrefs.SetInt("MVolume", volumeInt);
        mc.SetVolume();
    }

    public void updateFXVolumeSlider()
    {
        int volumeInt = (int)fxVolSlider.value;
        PlayerPrefs.SetInt("FXvolumeSet", 1);
        PlayerPrefs.SetInt("FXVolume", volumeInt);
        mc.SetVolume();
    }
}

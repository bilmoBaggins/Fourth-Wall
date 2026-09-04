using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static float musicVol = 1f;
    public static float SFXVol = 1f;

    public GameObject Music;
    public GameObject SFX;
    private UnityEngine.UI.Slider musicLevel;
    private UnityEngine.UI.Slider SFXLevel;

    [Header("--------------- Audio Sources ---------------")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("--------------- Generic Audio Clips ---------------")]
    public AudioClip background;
    public AudioClip logo;
    public AudioClip startGame;
    public AudioClip death;
    public AudioClip jump;
    public AudioClip wallTouch;
    public AudioClip pickUp;
    public AudioClip teleport;
    public AudioClip dimensionSwitch;
    public AudioClip buttonClick;

    [Header("--------------- Enemy Audio Clips ---------------")]
    public AudioClip laser;
    public AudioClip spike;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;

        if (buildIndex == 0)
        {
            musicSource.clip = background;
            musicSource.Play(44100);
        }
        else
        {
            musicSource.clip = background;
            musicSource.Play();
        }

            musicLevel = Music.GetComponent<UnityEngine.UI.Slider>();
        SFXLevel = SFX.GetComponent<UnityEngine.UI.Slider>();

        musicSource.volume = musicVol;
        SFXSource.volume = SFXVol;
        musicLevel.value = musicSource.volume;
        SFXLevel.value = SFXSource.volume;
    }

    void Update()
    {
        musicLevel.value = musicSource.volume;
        SFXLevel.value = SFXSource.volume;

        musicVol = musicSource.volume;
        SFXVol = SFXSource.volume;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public static (float, float) GetVolume()
    {
        return (AudioManager.musicVol, AudioManager.SFXVol);
    }

    public static void SetVolume(float music, float SFX)
    {
        AudioManager.musicVol = music;
        AudioManager.SFXVol = SFX;
    }
}

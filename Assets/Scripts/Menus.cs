using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    [SerializeField] GameObject toggle;
    [SerializeField] GameObject speedrunWarning;
    private Toggle speedrun;

    [SerializeField] GameObject timesMenu;
    private List<float> times = new();

    [SerializeField] RectTransform content;
    [SerializeField] GameObject timesText;
    TextMeshProUGUI timesText_text;
    string text = "";

    float heightMult = 1.08f;

    float ms;
    float s;
    float m;
    string stringMs;
    string stringS;
    string stringM;

    float musicVol;
    float SFXVol;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        speedrun = toggle.GetComponent<Toggle>();
        if (speedrun != null)
        {
            speedrun.isOn = Game.speedrun;
        }
        if (timesText != null)
        {
            timesText_text = timesText.GetComponent<TextMeshProUGUI>();
        }
        if (content != null)
        {
            content.sizeDelta = new Vector2(content.sizeDelta.x, 185);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (speedrunWarning != null && speedrun != null)
        {
            speedrunWarning.SetActive(speedrun.isOn);
        }
    }

    public void StartGame()
    {
        if (!speedrun.isOn)
        {
            Game.speedrun = false;
            (musicVol, SFXVol) = AudioManager.GetVolume();
            audioManager.PlaySFX(audioManager.startGame);
            Teleport.scene++;
            SceneManager.LoadScene(Teleport.scene);
            Teleport.levelStart = true;
            AudioManager.SetVolume(musicVol, SFXVol);
        }
        else
        {
            Game.speedrun = true;
            Game.seconds = 0f;
            (musicVol, SFXVol) = AudioManager.GetVolume();
            audioManager.PlaySFX(audioManager.startGame);
            Teleport.scene += 2;
            SceneManager.LoadScene(Teleport.scene);
            Teleport.levelStart = true;
            AudioManager.SetVolume(musicVol, SFXVol);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit.");
    }

    public void ShowTimes()
    {
        text = "";
        times = Game.SaveSystem.LoadFloatList("Times");

        for (int i = 0; i < times.Count; i++)
        {
            ms = Mathf.Floor((times[i] - Mathf.Floor(times[i])) * 1000f);
            if (ms.ToString().Length == 2)
            {
                stringMs = $"0{ms}";
            }
            else if (ms.ToString().Length == 1)
            {
                stringMs = $"00{ms}";
            }
            else
            {
                stringMs = ms.ToString();
            }

            s = (Mathf.Floor(times[i])) % 60;
            if (s.ToString().Length == 1)
            {
                stringS = $"0{s}";
            }
            else
            {
                stringS = s.ToString();
            }

            m = Mathf.Floor((Mathf.Floor(times[i])) / 60);
            if (m.ToString().Length == 1)
            {
                stringM = $"0{m}";
            }
            else
            {
                stringM = m.ToString();
            }

            if (i < 9)
            {
                text += $"0{i + 1} ----- {stringM}:{stringS}:{stringMs}\n";
            }
            else
            {
                text += $"{i + 1} ----- {stringM}:{stringS}:{stringMs}\n";
            }
        }

        if (timesMenu.activeInHierarchy)
        {
            content.transform.position = Vector3.zero;
            timesText_text.text = text;

            if (times.Count > 7)
            {
                int extra = times.Count - 7;
                heightMult = 23.333333f * extra;

                content.sizeDelta = new Vector2(content.sizeDelta.x, 185f + heightMult);
            }
            else
            {
                content.sizeDelta = new Vector2(content.sizeDelta.x, 185f);
            }
        }
    }

    public void ClearData()
    {
        times = Game.SaveSystem.LoadFloatList("Times");
        times.Clear();
        Game.SaveSystem.SaveFloatList("Times", times);

        ShowTimes();
    }

    public void QuitLevel()
    {
        Teleport.scene = 0;
        SceneManager.LoadScene(Teleport.scene);
    }

    public void Click()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
    }
}

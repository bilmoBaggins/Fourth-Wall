using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SpeedrunCutscene : MonoBehaviour
{
    PlayableDirector director;
    [SerializeField] GameObject canvas;
    Animator animator;

    [SerializeField] RectTransform content;

    [SerializeField] GameObject timesText;

    [SerializeField] GameObject newBest;

    [SerializeField] GameObject currentTime;
    [SerializeField] GameObject bestTime;
    [SerializeField] GameObject newBestTime;

    TextMeshProUGUI timesText_text;

    TextMeshProUGUI currentTimeText;
    TextMeshProUGUI bestTimeText;
    TextMeshProUGUI newBestTimeText;

    List<float> times = new();

    string text;
    float heightMult;

    float ms;
    float s;
    float m;

    string stringMs;
    string stringS;
    string stringM;

    bool end = false;
    float seconds;

    float musicVol;
    float SFXVol;

    // Start is called before the first frame update
    void Start()
    {
        director = gameObject.GetComponent<PlayableDirector>();
        animator = canvas.GetComponent<Animator>();

        times = Game.SaveSystem.LoadFloatList("Times");

        seconds = 0f;

        timesText_text = timesText.GetComponent<TextMeshProUGUI>();

        currentTimeText = currentTime.GetComponent<TextMeshProUGUI>();
        bestTimeText = bestTime.GetComponent<TextMeshProUGUI>();
        newBestTimeText = newBestTime.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;
        RectTransform viewport = content.parent.GetComponent<RectTransform>();
        float targetY = content.sizeDelta.y - viewport.rect.height;

        if (content.anchoredPosition.y < targetY)
        {
            content.anchoredPosition += new Vector2(0, 20 * Time.deltaTime);
        }
        else
        {
            if (!end && seconds > 5)
            {
                end = true;
                seconds = 0;
            }
        }

        if (end && seconds >= 2)
        {
            (musicVol, SFXVol) = AudioManager.GetVolume();
            Teleport.scene++;
            SceneManager.LoadScene(Teleport.scene);
            Teleport.levelStart = true;
            AudioManager.SetVolume(musicVol, SFXVol);
        }

        DisplayTime(Game.seconds, currentTimeText);

        if (Game.newBestTime)
        {
            animator.enabled = true;
            DisplayTime(times[1], bestTimeText);
            DisplayTime(times[0], newBestTimeText);

            newBest.SetActive(true);
        }
        else
        {
            animator.enabled = false;
            DisplayTime(times[0], bestTimeText);

            newBest.SetActive(false);
        }

        ShowPrevTimes();

        if (seconds >= 5f || end)
        {
            director.time = 5f;
        }
    }

    void DisplayTime(float seconds, TextMeshProUGUI timeText)
    {
        ms = Mathf.Floor((seconds - Mathf.Floor(seconds)) * 1000f);
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

        s = (Mathf.Floor(seconds)) % 60;
        if (s.ToString().Length == 1)
        {
            stringS = $"0{s}";
        }
        else
        {
            stringS = s.ToString();
        }

        m = Mathf.Floor((Mathf.Floor(seconds)) / 60);
        if (m.ToString().Length == 1)
        {
            stringM = $"0{m}";
        }
        else
        {
            stringM = m.ToString();
        }

        timeText.text = $"{stringM}:{stringS}:{stringMs}";
    }

    void ShowPrevTimes()
    {
        text = "";

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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject boundScreen;
    [SerializeField] GameObject lifeHeart1;
    [SerializeField] GameObject lifeHeart2;
    [SerializeField] GameObject lifeHeart3;

    [SerializeField] GameObject SpeedrunTime;

    TextMeshProUGUI timeText;

    public static bool death = false;
    public static bool speedrun = false;

    private List<float> times = new();
    public static float seconds;
    public static bool newBestTime = false;

    private float ms;
    private float s;
    private float m;

    private string stringMs;
    private string stringS;
    private string stringM;

    AudioManager audioManager;

    public static class SaveSystem
    {
        public static void SaveFloatList(string key, List<float> floatList)
        {
            string json = JsonUtility.ToJson(new FloatListWrapper(floatList));
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static List<float> LoadFloatList(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<FloatListWrapper>(json).floats;
            }
            return new List<float>();
        }
    }

    public class FloatListWrapper
    {
        public List<float> floats;

        public FloatListWrapper(List<float> list)
        {
            floats = list;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.SetActive(false);
        newBestTime = false;

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        timeText = SpeedrunTime.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
            }
            else
            {
                pauseMenu.SetActive(false);
            }
        }

        if (PlayerController2D.hit)
        {
            LifeLoss();
            PlayerController2D.hit = false;
        }
        if (PickUpHeart.lifeUp)
        {
            LifeUp();
            PickUpHeart.lifeUp = false;
        }

        if (pauseMenu.activeInHierarchy || boundScreen.activeInHierarchy)
        {
            death = true;
        }
        else if (!pauseMenu.activeInHierarchy && !gameOverCanvas.activeInHierarchy && !boundScreen.activeInHierarchy)
        {
            death = false;
        }

        if (speedrun)
        {
            SpeedrunTime.SetActive(true);

            if (Teleport.scene >= 1 && Teleport.scene <= 10)
            {
                seconds = Timer(seconds);

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
            else if (Teleport.scene > 10)
            {
                times = SaveSystem.LoadFloatList("Times");
                if (times.Count > 0)
                {
                    if (seconds < times[0])
                    {
                        newBestTime = true;
                    }
                }

                times.Add(seconds);
                times.Sort();

                SaveSystem.SaveFloatList("Times", times);
            }
        }
        else
        {
            SpeedrunTime.SetActive(false);
        }

        if (Teleport.scene == 0)
        {
            Console.Clear();

            times = SaveSystem.LoadFloatList("Times");
        }
    }

    void LifeLoss()
    {
        if (!lifeHeart3.activeInHierarchy)
        {
            if (!lifeHeart2.activeInHierarchy)
            {
                if (!lifeHeart1.activeInHierarchy)
                {
                    Death();
                }
                else
                {
                    lifeHeart1.SetActive(false);
                    LifeLoss();
                }
            }
            else
            {
                lifeHeart2.SetActive(false);
            }
        }
        else
        {
            lifeHeart3.SetActive(false);
        }
    }

    public void LifeUp()
    {
        if (lifeHeart2.activeInHierarchy)
        {
            if (lifeHeart3.activeInHierarchy)
            {
                lifeHeart3.SetActive(true);
            }
            else
            {
                lifeHeart3.SetActive(true);
            }
        }
        else
        {
            lifeHeart2.SetActive(true);
        }
    }

    void Death()
    {
        gameOverCanvas.SetActive(true);
        death = true;
        audioManager.PlaySFX(audioManager.death);
    }

    public void Reset()
    {
        SceneManager.LoadScene(Teleport.scene);
        Teleport.levelStart = true;
    }

    public static float GetTime()
    {
        return Game.seconds;
    }

    public static void SetTime(float seconds)
    {
        Game.seconds = seconds;
    }

    private float Timer(float seconds)
    {
        if (!death)
        {
            seconds += Time.deltaTime;
        }

        if (Teleport.scene > 10)
        {
            return seconds;
        }

        return seconds;
    }
}

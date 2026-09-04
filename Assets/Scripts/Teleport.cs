using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject particles;

    public static bool levelStart = false;
    public static int scene = 0;
    public static bool teleportActive = false;

    public static bool teleport = false;

    float musicVol;
    float SFXVol;
    float seconds;
    float time = 3f;

    // Start is called before the first frame update
    void Start()
    {
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        particles.SetActive(teleportActive);

        if (Game.speedrun && scene > 12)
        {
            teleport = true;
            scene = 0;
            SceneManager.LoadScene(scene);
            levelStart = true;
        }
        else if (!Game.speedrun && scene > 11)
        {
            teleport = true;
            scene = 0;
            SceneManager.LoadScene(scene);
            levelStart = true;
        }

        if (Game.speedrun)
        {
            if (Input.GetKey(KeyCode.R))
            {
                time = Timer(time);
            }
        }
    }

    float Timer(float time)
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            time = 3f;
            Game.seconds = 0;
            teleport = true;
            scene = 2;
            SceneManager.LoadScene(scene);
            levelStart = true;

        }
        return time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (teleportActive)
        {
            if (!Game.speedrun)
            {
                (musicVol, SFXVol) = AudioManager.GetVolume();
                teleport = true;
                scene++;
                SceneManager.LoadScene(scene);
                levelStart = true;
                AudioManager.SetVolume(musicVol, SFXVol);
            }
            else
            {
                seconds = Game.GetTime();
                (musicVol, SFXVol) = AudioManager.GetVolume();
                teleport = true;
                scene += 2;
                SceneManager.LoadScene(scene);
                levelStart = true;
                Game.SetTime(seconds);
                AudioManager.SetVolume(musicVol, SFXVol);
            }
        }
    }
}

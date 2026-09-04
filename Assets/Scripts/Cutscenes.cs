using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class Cutscenes : MonoBehaviour
{
    public float sceneDuration;

    float musicVol;
    float SFXVol;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        sceneDuration -= Time.deltaTime;

        if (sceneDuration <= 0)
        {
            (musicVol, SFXVol) = AudioManager.GetVolume();
            Teleport.scene++;
            SceneManager.LoadScene(Teleport.scene);
            Teleport.levelStart = true;
            AudioManager.SetVolume(musicVol, SFXVol);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            (musicVol, SFXVol) = AudioManager.GetVolume();
            Teleport.scene++;
            SceneManager.LoadScene(Teleport.scene);
            Teleport.levelStart = true;
            AudioManager.SetVolume(musicVol, SFXVol);
        }
    }
}

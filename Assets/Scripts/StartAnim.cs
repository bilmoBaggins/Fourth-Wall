using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnim : MonoBehaviour
{
    [SerializeField] GameObject brokenWall;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (brokenWall.activeInHierarchy)
        {
            audioManager.PlaySFX(audioManager.logo);
        }
    }
}

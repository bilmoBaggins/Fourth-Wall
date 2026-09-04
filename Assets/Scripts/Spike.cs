using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Spike : MonoBehaviour
{
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !Game.death)
        {
            audioManager.PlaySFX(audioManager.spike);
        }
    }
}
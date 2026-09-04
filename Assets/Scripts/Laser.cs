using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;

public class Laser : MonoBehaviour
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
        transform.Rotate(0, 0, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !Game.death)
        {
            audioManager.PlaySFX(audioManager.laser);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    Vector3 pos2D;

    public static bool pick = false;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        pos2D = transform.position;

        pick = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DimensionSwitch.secondDim)
        {
            transform.position = new Vector3(-91.5f, 1, -3);
            transform.rotation = Quaternion.Euler(-9, 0, 0);
        }
        else if (DimensionSwitch.secondDim)
        {
            transform.position = pos2D;
            transform.rotation = Quaternion.Euler(-15, -90, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.pickUp);
            pick = true;
            gameObject.SetActive(false);
        }
    }
}

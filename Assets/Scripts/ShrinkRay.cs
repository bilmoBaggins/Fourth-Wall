using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkRay : MonoBehaviour
{
    [SerializeField] GameObject ray;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        ray.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!DimensionSwitch.secondDim)
        {
            transform.position = new Vector3(60, 2, -3);
            transform.rotation = Quaternion.identity;
        }
        else if (DimensionSwitch.secondDim)
        {
            transform.position = new Vector3(60, -6, -1.5f);
            transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.pickUp);
            ray.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

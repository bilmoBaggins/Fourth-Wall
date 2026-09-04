using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHeart : MonoBehaviour
{
    Vector3 pos2D;

    public float xPos;
    public float height2D;

    public static bool lifeUp = false;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        pos2D = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DimensionSwitch.secondDim)
        {
            transform.position = new Vector3(xPos, height2D + 8, -3);
            transform.rotation = Quaternion.identity;
        }
        else if (DimensionSwitch.secondDim)
        {
            transform.position = pos2D;
            transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.pickUp);
            lifeUp = true;
            gameObject.SetActive(false);
        }
    }
}

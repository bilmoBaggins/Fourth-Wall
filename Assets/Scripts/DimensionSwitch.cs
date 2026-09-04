using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionSwitch : MonoBehaviour
{
    [SerializeField] GameObject scene;
    [SerializeField] GameObject light;
    [SerializeField] GameObject fg;
    private Rigidbody playerRb;

    public static bool secondDim = true;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.constraints = RigidbodyConstraints.FreezePositionZ;

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (secondDim && !Game.death)
            {
                scene.transform.rotation = Quaternion.Euler(90, 0, 0);
                light.transform.rotation = Quaternion.Euler(90, 0, 0);
                playerRb.constraints = RigidbodyConstraints.None;
                transform.rotation = Quaternion.identity;
                transform.position = new Vector3(transform.position.x, 3.5f, transform.position.y);
                Camera.main.orthographic = false;
                fg.SetActive(true);

                secondDim = false;

                audioManager.PlaySFX(audioManager.dimensionSwitch);
            }
            else if (!secondDim && !Game.death)
            {
                scene.transform.rotation = Quaternion.identity;
                light.transform.rotation = Quaternion.Euler(10, 0, -1);
                playerRb.constraints = RigidbodyConstraints.FreezePositionZ;
                transform.rotation = Quaternion.identity;
                transform.position = new Vector3(transform.position.x, transform.position.z, -1);
                Camera.main.orthographic = true;
                fg.SetActive(false);

                secondDim = true;

                audioManager.PlaySFX(audioManager.dimensionSwitch);
            }
        }

        if (Teleport.levelStart)
        {
            scene.transform.rotation = Quaternion.identity;
            light.transform.rotation = Quaternion.Euler(10, 0, -1);
            playerRb.constraints = RigidbodyConstraints.FreezePositionZ;
            transform.rotation = Quaternion.identity;
            Camera.main.orthographic = true;
            fg.SetActive(false);

            secondDim = true;
            Teleport.teleportActive = false;
            Teleport.levelStart = false;
        }
    }
}

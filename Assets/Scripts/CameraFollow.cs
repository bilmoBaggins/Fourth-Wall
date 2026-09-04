using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;

    public static Vector3 offset;
    private Vector3 targetPos;
    private float speed = 3f;

    private float cameraInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraInput = Input.GetAxis("Horizontal2");
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(20, 90, 0);
        if (!DimensionSwitch.secondDim)
        {
            if (PlayerController3D.forwardInput >= 0)
            {
                offset = new Vector3(-8, 5.5f, 0);
            }
            else
            {
                offset = new Vector3(-15, 5.5f, 0);
            }

            if (DimensionSwitch.secondDim)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (!DimensionSwitch.secondDim)
            {
                transform.rotation = Quaternion.Euler(20, 90, 0);
                transform.rotation = Quaternion.Euler(20, 90 - 90 * -cameraInput, 0);
            }
        }
        else
        {
            offset = new Vector3(0, 3.35f, -9);
            transform.rotation = Quaternion.identity;
        }

        if (DimensionSwitch.secondDim)
        {
            targetPos = player.transform.position + offset;
        }
        if (!DimensionSwitch.secondDim)
        {
            targetPos = player.transform.position + offset + new Vector3(7.5f * Mathf.Abs(cameraInput), 0, 5f * cameraInput);
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);

        if ((transform.position.y != 7) && !DimensionSwitch.secondDim && Teleport.scene != 4 && Teleport.scene != 10)
        {
            transform.position = new Vector3(transform.position.x, 7, transform.position.z);
        }
        if (!DimensionSwitch.secondDim && Teleport.scene == 4)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed / 3);

            if (transform.position.y > 18)
            {
                transform.position = new Vector3(transform.position.x, 18, transform.position.z);
            }
            if (transform.position.y < 7)
            {
                transform.position = new Vector3(transform.position.x, 7, transform.position.z);
            }
        }
        if (!DimensionSwitch.secondDim && Teleport.scene == 10)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed / 3);

            if (transform.position.y > 12)
            {
                transform.position = new Vector3(transform.position.x, 12, transform.position.z);
            }
            if (transform.position.y < -2)
            {
                transform.position = new Vector3(transform.position.x, -2, transform.position.z);
            }
        }

        if (transform.position.y != -2.75f && DimensionSwitch.secondDim && Teleport.scene != 10)
        {
            transform.position = new Vector3(transform.position.x, -2.75f, transform.position.z);
        }
        if (DimensionSwitch.secondDim && Teleport.scene == 10)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed / 3);

            if (transform.position.y > 4.5f)
            {
                transform.position = new Vector3(transform.position.x, 4.5f, transform.position.z);
            }
            if (transform.position.y < -10.5f)
            {
                transform.position = new Vector3(transform.position.x, -10.5f, transform.position.z);
            }
        }
    }
}
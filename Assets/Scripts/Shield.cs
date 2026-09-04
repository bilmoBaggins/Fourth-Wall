using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private GameObject player;

    private Vector3 offset;
    private Quaternion rotation;

    private bool active = false;

    AudioManager audioManager;

    public Animator animator;

    private Rigidbody rb;
    private MeshCollider mesh;

    private float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        Missile.missileCount = 5;
        gameObject.SetActive(true);

        animator.SetBool("Active", false);

        active = false;

        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        mesh = gameObject.GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, speed);
            transform.rotation = rotation;
        }
        else
        {
            if (DimensionSwitch.secondDim)
            {
                transform.position = new Vector3(-67.5f, 6.25f, -1);
                transform.rotation = Quaternion.Euler(-90, 0, 0);
            }
            if (!DimensionSwitch.secondDim)
            {
                transform.position = new Vector3(-67.5f, 0, 7.25f);
                transform.rotation = Quaternion.Euler(-90, 0, 0);
            }
        }

        if (!DimensionSwitch.secondDim && PlayerController3D.forwardInput >= 0)
        {
            offset = new Vector3(-1, 0, 0);
            rotation = Quaternion.Euler(-90, 0, 0);
        }
        else if (!DimensionSwitch.secondDim && PlayerController3D.forwardInput < 0)
        {
            offset = new Vector3(1, 0, 0);
            rotation = Quaternion.Euler(-90, 0, 0);
        }
        else if (DimensionSwitch.secondDim && PlayerController2D.horizontalInput >= 0)
        {
            offset = new Vector3(-1, 0, 0);
            rotation = Quaternion.Euler(-90, 0, 0);
        }
        else if (DimensionSwitch.secondDim && PlayerController2D.horizontalInput < 0)
        {
            offset = new Vector3(1, 0, 0);
            rotation = Quaternion.Euler(-90, 0, 0);
        }

        if (Missile.missileCount <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Active", true);

            active = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Missile.explode = true;
        }
    }
}

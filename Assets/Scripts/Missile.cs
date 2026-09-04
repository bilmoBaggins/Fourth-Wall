using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private GameObject player;
    public float speed;
    public Vector3 offset;

    private bool follow = false;

    public static bool explode = false;

    public Animator animator;
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public ParticleSystem ps3;

    public static int missileCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game.death)
        {
            if (player.transform.position.x > -30)
            {
                follow = true;
            }
            if (follow)
            {
                transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * speed);
                if (player.transform.position.x > transform.position.x)
                {
                    animator.SetBool("TurnL", false);
                    animator.SetBool("TurnR", true);
                }
                if (player.transform.position.x < transform.position.x)
                {
                    animator.SetBool("TurnR", false);
                    animator.SetBool("TurnL", true);
                }
            }

            if (explode)
            {
                ps1.transform.position = transform.position;
                ps2.transform.position = transform.position;
                ps3.transform.position = transform.position;
                ps1.Play();
                ps2.Play();
                ps3.Play();
                gameObject.SetActive(false);
                missileCount--;

                explode = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BreakableWall") || collision.gameObject.CompareTag("Player"))
        {
            ps1.transform.position = transform.position;
            ps2.transform.position = transform.position;
            ps3.transform.position = transform.position;
            ps1.Play();
            ps2.Play();
            ps3.Play();
            gameObject.SetActive(false);
            missileCount--;
        }
    }
}

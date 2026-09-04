using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject player;
    private float speed = 1.0f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > 65 && !animator.GetBool("Shrink") && !Game.death)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * speed);

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
    }

    private void OnParticleCollision(GameObject other)
    {
        animator.SetBool("Shrink", true);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Teleport.teleportActive = true;
    }
}

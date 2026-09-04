using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    AudioManager audioManager;
    public Animator animation;

    private float speed = 20f;

    public float direction = 1;
    public float minX;
    public float maxX;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game.death)
        {
            transform.Translate(Vector3.right * speed * direction * Time.deltaTime);
            transform.localScale = new Vector3(direction, 1, 1);

            if (transform.position.x > maxX)
            {
                direction *= -1;
            }
            if (transform.position.x < minX)
            {
                direction *= -1;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !Game.death)
        {
            audioManager.PlaySFX(audioManager.spike);
        }
    }
}

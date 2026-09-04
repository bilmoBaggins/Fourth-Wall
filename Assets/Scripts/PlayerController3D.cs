using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    [SerializeField] GameObject pick;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject ray;
    [SerializeField] GameObject boundScreen;

    private Rigidbody playerRb;
    private float speed = 10f;
    private float fallMult = 5f;
    private float lowJumpMult = 1.5f;
    private float jumpForce = 8f;
    public static float horizontalInput;
    public static float forwardInput;
    private bool isGrounded = true;
    public Animator animation;

    [SerializeField] ParticleSystem particleSystem;

    private bool checkCol;
    private float hitTime = 1f;

    public static Vector3 startPos3D;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        startPos3D = transform.position;

        pick.SetActive(false);
        checkCol = true;

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -30)
        {
            Game.death = true;
            boundScreen.SetActive(true);
        }

        if (!DimensionSwitch.secondDim && !Game.death)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            forwardInput = Input.GetAxis("Vertical");

            transform.Translate(Vector3.back * horizontalInput * Time.deltaTime * speed);
            transform.Translate(Vector3.right * forwardInput * Time.deltaTime * speed);

            if (forwardInput >= 0)
            {
                animation.SetBool("MovingR", true);
                animation.SetBool("MovingL", false);
            }
            else
            {
                animation.SetBool("MovingR", false);
                animation.SetBool("MovingL", true);
            }

            if (horizontalInput > 0)
            {
                Quaternion tilt = Quaternion.Euler(30, 0, 0);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, transform.rotation.y, 0), tilt, Time.deltaTime * speed / 1.5f);
            }
            else if (horizontalInput < 0)
            {
                Quaternion tilt = Quaternion.Euler(-30, 0, 0);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, transform.rotation.y, 0), tilt, Time.deltaTime * speed / 1.5f);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }

            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;

                if (horizontalInput >= 0)
                {
                    animation.SetBool("JumpingR", true);
                    animation.SetBool("JumpingL", false);
                }
                else
                {
                    animation.SetBool("JumpingR", false);
                    animation.SetBool("JumpingL", true);
                }

                audioManager.PlaySFX(audioManager.jump);
            }
            if (playerRb.velocity.y < 0)
            {
                playerRb.velocity += Vector3.up * Physics.gravity.y * fallMult * Time.deltaTime;
            }
            else if (playerRb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                playerRb.velocity += Vector3.up * Physics.gravity.y * lowJumpMult * Time.deltaTime;
            }

            if (wall.activeInHierarchy)
            {
                pick.SetActive(Pickaxe.pick);
            }

            if (Input.GetKey(KeyCode.E))
            {
                if (pick.activeInHierarchy)
                {
                    if (forwardInput >= 0)
                    {
                        animation.SetBool("SwingR", true);
                        animation.SetBool("SwingL", false);
                    }
                    else
                    {
                        animation.SetBool("SwingR", false);
                        animation.SetBool("SwingL", true);
                    }
                }

                if (ray.activeInHierarchy)
                {
                    if (forwardInput >= 0)
                    {
                        particleSystem.transform.position = transform.position + new Vector3(1.5f, 0.5f, -0.5f);
                        particleSystem.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    if (forwardInput < 0)
                    {
                        particleSystem.transform.position = transform.position + new Vector3(-1.5f, 0.5f, -0.5f);
                        particleSystem.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    particleSystem.Play();
                }
            }
            else
            {
                animation.SetBool("SwingR", false);
                animation.SetBool("SwingL", false);
            }

            if (!checkCol)
            {
                hitTime = HitTimer(hitTime);
            }
            else
            {
                hitTime = 1f;
            }
        }

        if (Game.death)
        {
            animation.SetBool("Damage", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!DimensionSwitch.secondDim && !Game.death)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                animation.SetBool("JumpingR", false);
                animation.SetBool("JumpingL", false);

                audioManager.PlaySFX(audioManager.wallTouch);
            }

            if (collision.gameObject.CompareTag("3DGround") && !DimensionSwitch.secondDim)
            {
                isGrounded = true;
                animation.SetBool("JumpingR", false);
                animation.SetBool("JumpingL", false);

                audioManager.PlaySFX(audioManager.wallTouch);
            }

            if (collision.gameObject.CompareTag("BreakableWall"))
            {
                isGrounded = true;
                animation.SetBool("JumpingR", false);
                animation.SetBool("JumpingL", false);

                if (pick.activeInHierarchy)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        wall.SetActive(false);
                        pick.SetActive(false);
                    }
                }
            }

            if (collision.gameObject == VaultDoor.circlePlatform)
            {
                VaultDoor.code.Add("circle");
                VaultDoor.circlePlatform.SetActive(false);
            }

            if (collision.gameObject == VaultDoor.trianglePlatform)
            {
                VaultDoor.code.Add("triangle");
                VaultDoor.trianglePlatform.SetActive(false);
            }

            if (collision.gameObject == VaultDoor.squarePlatform)
            {
                VaultDoor.code.Add("square");
                VaultDoor.squarePlatform.SetActive(false);
            }

            if (collision.gameObject == VaultDoor.crossPlatform)
            {
                VaultDoor.code.Add("cross");
                VaultDoor.crossPlatform.SetActive(false);
            }

            if (collision.gameObject == VaultDoor.resetPlatform)
            {
                VaultDoor.code.Clear();
                VaultDoor.circlePlatform.SetActive(true);
                VaultDoor.trianglePlatform.SetActive(true);
                VaultDoor.squarePlatform.SetActive(true);
                VaultDoor.crossPlatform.SetActive(true);
                VaultDoor.resetPlatform.SetActive(false);
            }

            if (collision.gameObject.CompareTag("Off"))
            {
                collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindGameObjectWithTag("On").GetComponent<MeshRenderer>().enabled = true;
                Teleport.teleportActive = true;

                audioManager.PlaySFX(audioManager.buttonClick);
            }

            if (collision.gameObject.CompareTag("Platform"))
            {
                collision.gameObject.SetActive(false);

                isGrounded = true;
                animation.SetBool("JumpingR", false);
                animation.SetBool("JumpingL", false);

                audioManager.PlaySFX(audioManager.buttonClick);
            }

            if (collision.gameObject.CompareTag("Climbable"))
            {
                gameObject.transform.Translate(Vector3.up * Time.deltaTime * speed);

                isGrounded = true;
                animation.SetBool("JumpingR", false);
                animation.SetBool("JumpingL", false);
            }

            if (collision.gameObject.CompareTag("Enemy") && !Game.death)
            {
                if (checkCol)
                {
                    PlayerController2D.hit = true;
                    checkCol = false;
                }
            }
        }
    }

    private float HitTimer(float seconds)
    {
        seconds -= Time.deltaTime;
        animation.SetBool("Damage", true);

        if (seconds <= 0)
        {
            animation.SetBool("Damage", false);
            checkCol = true;
            return 0;
        }

        return seconds;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultDoor : MonoBehaviour
{
    [SerializeField] GameObject circle;
    [SerializeField] GameObject triangle;
    [SerializeField] GameObject square;
    [SerializeField] GameObject cross;
    [SerializeField] GameObject reset;
    [SerializeField] GameObject door;

    public static GameObject circlePlatform;
    public static GameObject trianglePlatform;
    public static GameObject squarePlatform;
    public static GameObject crossPlatform;
    public static GameObject resetPlatform;

    public static List<string> code = new List<string>();

    private bool cleared = false;
    private float hitTime = 1f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("Opened", false);

        circlePlatform = circle;
        trianglePlatform = triangle;
        squarePlatform = square;
        crossPlatform = cross;
        resetPlatform = reset;

        circle.SetActive(true);
        triangle.SetActive(true);
        square.SetActive(true);
        cross.SetActive(true);
        reset.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (code.Count == 4)
        {
            if (code[0] == "circle" && code[1] == "triangle" && code[2] == "square" && code[3] == "cross")
            {
                animator.SetBool("Opened", true);
            }
            else
            {
                animator.SetBool("Wrong", true);
                cleared = true;

                circle.SetActive(true);
                triangle.SetActive(true);
                square.SetActive(true);
                cross.SetActive(true);
                reset.SetActive(true);

                code.Clear();
            }
        }

        if (!reset.activeInHierarchy || cleared)
        {
            hitTime = ResetTimer(hitTime);
        }
        else
        {
            hitTime = 1f;
        }
    }

    private float ResetTimer(float seconds)
    {
        seconds -= Time.deltaTime;

        if (seconds <= 0)
        {
            animator.SetBool("Wrong", false);
            reset.SetActive(true);
            cleared = false;

            return 0;
        }

        return seconds;
    }
}

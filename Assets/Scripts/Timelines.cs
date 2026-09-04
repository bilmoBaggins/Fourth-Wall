using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Timelines : MonoBehaviour
{
    public GameObject speedrunTimeline;
    public List<float> xPos = new List<float>();
    public List<GameObject> cutscenes = new List<GameObject>();

    private GameObject player;

    private float time = 4f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game.speedrun)
        {
            if (xPos.Count > 0)
            {
                if (player.transform.position.x > xPos[0])
                {
                    cutscenes[0].transform.localScale = Vector3.one;
                    cutscenes[0].GetComponent<PlayableDirector>().Play();

                    time -= Time.deltaTime;

                    if (time <= 0)
                    {
                        cutscenes[0].GetComponent<PlayableDirector>().Stop();
                        cutscenes[0].SetActive(false);

                        cutscenes.RemoveAt(0);
                        xPos.RemoveAt(0);

                        time = 4;
                    }
                }
            }
        }
        else if (Game.speedrun && Teleport.scene == 2)
        {
            if (player.transform.position.x > -153f)
            {
                speedrunTimeline.transform.localScale = Vector3.one;
                speedrunTimeline.GetComponent<PlayableDirector>().Play();

                time -= Time.deltaTime;

                if (time <= 0)
                {
                    speedrunTimeline.GetComponent<PlayableDirector>().Stop();
                    speedrunTimeline.SetActive(false);
                }
            }
        }
    }
}

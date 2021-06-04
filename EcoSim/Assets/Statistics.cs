using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    private float interval;
    private GameObject[] count;
    private GameObject[] count2;
    private int times;
    public int children;
    private float R;
    private float lastfem;
    private float lastchild;
    public string tocount;
    private float counting;
    private float counting2;

    void Start()
    {
        // tocount = "prey";
        interval = 0.5f;
        times = 0;
    }

    private void GetStatistics()
    {
        switch (tocount)
        {
            case "preypredator":
                count = GameObject.FindGameObjectsWithTag("Maleprey");
                count2 = GameObject.FindGameObjectsWithTag("Femaleprey");
                int Total = count.Length + count2.Length;
                if (Total != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, Total * 2), new Color(1f, 1f, 1f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Malepredator");
                count2 = GameObject.FindGameObjectsWithTag("Femalepredator");
                Total = count.Length + count2.Length;
                if (Total != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, Total * 2), new Color(0.6f, 0.6f, 0.6f, 1));
                }
                break;
            case "malefemale":
                count = GameObject.FindGameObjectsWithTag("Maleprey");
                if (count.Length != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, count.Length * 2), new Color(0.5f, 0.3f, 0.8f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Femaleprey");
                if (count.Length != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, count.Length * 2), new Color(0.8f, 0.5f, 0.3f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Malepredator");
                if (count.Length != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, count.Length * 2), new Color(0.3f, 0.1f, 0.6f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Femalepredator");
                if (count.Length != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, count.Length * 2), new Color(0.6f, 0.3f, 0.1f, 1));
                }
                if (children != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, children * 2), new Color(1f, 1f, 1f, 1));
                }
                break;
            case "des":
                count = GameObject.FindGameObjectsWithTag("Maleprey");
                counting = 0;
                if (count.Length != 0)
                {
                    for (int i = 0; i < count.Length; i++)
                    {
                        counting += count[i].GetComponent<PreyController>().desirability;
                    }
                    counting = counting / count.Length;
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting * 10), new Color(1f, 0.5f, 0.5f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Malepredator");
                counting = 0;
                if (count.Length != 0)
                {
                    for (int i = 0; i < count.Length; i++)
                    {
                        counting += count[i].GetComponent<PredatorController>().desirability;
                    }
                    counting = counting / count.Length;
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting * 10), new Color(0.8f, 0.3f, 0.3f, 1));
                }
                break;
            case "fertinc":
                count = GameObject.FindGameObjectsWithTag("Femaleprey");
                counting = 0;
                counting2 = 0;
                if (count.Length != 0)
                {
                    for (int i = 0; i < count.Length; i++)
                    {
                        counting += count[i].GetComponent<PreyController>().fertility;
                        counting2 += count[i].GetComponent<PreyController>().incubationperiod;
                    }
                    counting = counting / count.Length;
                    counting2 = counting2 / count.Length;
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting * 10), new Color(1f, 0.5f, 0.5f, 1));
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting2 * 10), new Color(0.7f, 1f, 0.7f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Femalepredator");
                counting = 0;
                counting2 = 0;
                if (count.Length != 0)
                {
                    for (int i = 0; i < count.Length; i++)
                    {
                        counting += count[i].GetComponent<PredatorController>().fertility;
                        counting2 += count[i].GetComponent<PredatorController>().incubationperiod;
                    }
                    counting = counting / count.Length;
                    counting2 = counting2 / count.Length;
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting * 10), new Color(0.8f, 0.3f, 0.3f, 1));
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting2 * 10), new Color(0.5f, 0.8f, 0.5f, 1));
                }
                break;
            case "speedradius":
                count = GameObject.FindGameObjectsWithTag("Maleprey");
                count2 = GameObject.FindGameObjectsWithTag("Femaleprey");
                Total = count.Length + count2.Length;
                counting = 0;
                counting2 = 0;
                if (Total != 0)
                {
                    for (int i = 0; i < count.Length; i++)
                    {
                        counting += count[i].GetComponent<PreyController>().speed;
                        counting2 += count[i].GetComponent<PreyController>().detectionradius;
                    }
                    for (int i = 0; i < count2.Length; i++)
                    {
                        counting += count2[i].GetComponent<PreyController>().speed;
                        counting2 += count2[i].GetComponent<PreyController>().detectionradius;
                    }
                    counting = counting / Total;
                    counting2 = counting2 / Total;
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting * 10), new Color(1f, 0.5f, 0.5f, 1));
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting2 * 10), new Color(0.7f, 1f, 0.7f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Malepredator");
                count2 = GameObject.FindGameObjectsWithTag("Femalepredator");
                Total = count.Length + count2.Length;
                counting = 0;
                counting2 = 0;
                if (Total != 0)
                {
                    for (int i = 0; i < count.Length; i++)
                    {
                        counting += count[i].GetComponent<PredatorController>().speed;
                        counting2 += count[i].GetComponent<PredatorController>().detectionradius;
                    }
                    for (int i = 0; i < count2.Length; i++)
                    {
                        counting += count2[i].GetComponent<PredatorController>().speed;
                        counting2 += count2[i].GetComponent<PredatorController>().detectionradius;
                    }
                    counting = counting / Total;
                    counting2 = counting2 / Total;
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting * 10), new Color(0.8f, 0.3f, 0.3f, 1));
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, counting2 * 10), new Color(0.5f, 0.8f, 0.5f, 1));
                }
                break;
            case "all":
                //preypredator
                count = GameObject.FindGameObjectsWithTag("Maleprey");
                count2 = GameObject.FindGameObjectsWithTag("Femaleprey");
                Total = count.Length + count2.Length;
                if (Total != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, Total * 2), new Color(1f, 1f, 1f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Malepredator");
                count2 = GameObject.FindGameObjectsWithTag("Femalepredator");
                Total = count.Length + count2.Length;
                if (Total != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, Total * 2), new Color(0.6f, 0.6f, 0.6f, 1));
                }
                //malefemale
                count = GameObject.FindGameObjectsWithTag("Maleprey");
                if (count.Length != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, count.Length * 2), new Color(0.5f, 0.3f, 0.8f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Femaleprey");
                if (count.Length != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, count.Length * 2), new Color(0.8f, 0.5f, 0.3f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Malepredator");
                if (count.Length != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, count.Length * 2), new Color(0.3f, 0.1f, 0.6f, 1));
                }
                count = GameObject.FindGameObjectsWithTag("Femalepredator");
                if (count.Length != 0)
                {
                    GameObject.Find("Window_Graph").GetComponent<Window_Graph>().CreateCircle(new Vector2(times * 10, count.Length * 2), new Color(0.6f, 0.3f, 0.1f, 1));
                }
                break;
        }
        children = 0;
    }

    void Update()
    {
       OnUpdateStatisticsEvent();
    }

    private void OnUpdateStatisticsEvent()
    {
        interval -= Time.deltaTime;
        if (tocount == "easyall")
        {
            interval = 2.5f;
            string[] things = new string[5];
            things[0] = "preypredator";
            things[1] = "malefemale";
            things[2] = "des";
            things[3] = "fertinc";
            things[4] = "speedradius";
            for (int i = 0; i < things.Length; i++)
            {
                tocount = things[i];
                GetStatistics();
            }
            times++;
            tocount = "easyall";
        }
        else
        {
            if (interval < 0)
            {
                interval = 2.5f;
                times++;
                GetStatistics();
            }
        }
    }
}

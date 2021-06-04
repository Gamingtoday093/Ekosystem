using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHandler : MonoBehaviour
{
    private float scrollSpeed = 0.25f;
    private GameObject[] WaterTileList;

    void Awake()
    {
        WaterTileList = new GameObject[0];
    }

    public void AddWaterTile(GameObject water)
    {
        WaterTileList = new GameObject[1];
        WaterTileList[0] = water;
    }

    // Update is called once per frame
    void Update()
    {
        if (WaterTileList.Length != 0)
        {
            float offset = Time.time * scrollSpeed;
            WaterTileList[0].GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(offset, offset));
            WaterTileList[0].GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_DetailAlbedoMap", new Vector2(-offset, -offset));
        }
    }
}

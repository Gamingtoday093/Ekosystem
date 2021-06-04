using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Landgenerator : MonoBehaviour
{
    public int rows = 12;
    public int cols = 12;
    public int seed;
    private int initialSeed;
    private float tileSize = 5;
    public GameObject wtile;
    public GameObject prey;
    public int preytotal = 5;
    public GameObject predator;
    public int predatortotal = 1;
    public GameObject Food;
    public Material watermat;
    public Material grassmat;
    private float posX;
    private float posZ;
    private float rndNumber;
    private GameObject Tiles;
    private GameObject Foods;

    // Start is called before the first frame update
    void Start()
    {
        initialSeed = seed;
        Random.InitState(initialSeed);
        generatemap();
        addprey();
        addpredator();
    }

    private void generatemap()
    {
        GameObject Group = new GameObject();
        Tiles = (GameObject)Instantiate(Group, transform.position, Quaternion.identity, this.gameObject.transform);
        Tiles.name = "Tiles";
        Foods = (GameObject)Instantiate(Group, transform.position, Quaternion.identity, this.gameObject.transform);
        Foods.name = "Foods";
        Destroy(Group);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {

                GameObject tile = (GameObject)Instantiate(wtile, transform.position, Quaternion.identity, Tiles.transform);

                posX = col * tileSize;
                posZ = row * -tileSize;

                tile.transform.position = new Vector3(posX, 0,posZ);

                rndNumber = UnityEngine.Random.Range(0f, 100f);
                if (rndNumber <= 25)
                {
                    tile.GetComponent<Renderer>().material = watermat;
                    tile.name = "Water";
                    GameObject water = (GameObject)Instantiate(Food, transform.position, Quaternion.identity, Foods.transform);
                    water.name = "Water";
                    water.transform.position = new Vector3(posX, 0.5f, posZ);
                    water.tag = "Water";
                    water.GetComponent<Renderer>().material = watermat;
                    GameObject.Find("WaterHandler").GetComponent<WaterHandler>().AddWaterTile(tile);
                }
                else
                {
                    tile.GetComponent<Renderer>().material = grassmat;
                    tile.name = "Grass";
                    if (rndNumber >= 90)
                    {
                        GameObject food = (GameObject)Instantiate(Food, transform.position, Quaternion.identity, Foods.transform);
                        food.name = "Food";
                        food.transform.position = new Vector3(posX, 0.5f, posZ);
                        food.tag = "Food";
                    }
                }
            }
        }
    }

    private void regeneratemap()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                posX = col * tileSize;
                posZ = row * -tileSize;
                rndNumber = UnityEngine.Random.Range(0f, 100f);
                Collider[] tilecolliders = Physics.OverlapSphere(new Vector3(posX, 0.5f, posZ), 1f);
                Collider[] foodcolliders = Physics.OverlapSphere(new Vector3(posX, 0.5f, posZ), 0.25f);
                if (foodcolliders.Length != 0)
                { 
                    if (foodcolliders[0].tag == "Food")
                    {
                    }
                    else if (foodcolliders[0].tag == "Water")
                    {
                    }
                }
                else if (foodcolliders.Length == 0)
                {
                    if (tilecolliders[0].tag == "Tile")
                    {
                        if (tilecolliders[0].GetComponent<Renderer>().sharedMaterial == watermat)
                        {
                            GameObject water = (GameObject)Instantiate(Food, transform.position, Quaternion.identity, Foods.transform);
                            water.name = "Water";
                            water.transform.position = new Vector3(posX, 0.5f, posZ);
                            water.tag = "Water";
                            water.GetComponent<Renderer>().material = watermat;
                        }
                        else
                        {
                            rndNumber = UnityEngine.Random.Range(0f, 100f);
                            if (rndNumber >= 90)
                            {
                                GameObject food = (GameObject)Instantiate(Food, transform.position, Quaternion.identity, Foods.transform);
                                food.name = "Food";
                                food.transform.position = new Vector3(posX, 0.5f, posZ);
                                food.tag = "Food";
                            }
                        }
                    }
                }
            }
        }
    }

    private void addprey()
    {
        for (int i = 0; i < preytotal; i++)
        {
            GameObject bunny = (GameObject)Instantiate(prey, transform);
            bunny.name = "Family " + (i);
            bunny.GetComponent<PreyController>().age = 10f;
            Random.InitState(initialSeed * i);
            float setRandom = UnityEngine.Random.Range(1f, 14f);
            bunny.GetComponent<PreyController>().speed = setRandom;
            setRandom = UnityEngine.Random.Range(1f, 14f);
            bunny.GetComponent<PreyController>().detectionradius = setRandom;
            posX = UnityEngine.Random.Range(0, (rows * 5));
            posZ = UnityEngine.Random.Range(-(cols * 5), 0);
            bunny.transform.position = new Vector3(posX, 0.5f, posZ);
        }
    }

    private void addpredator()
    {
        for (int i = 0; i < predatortotal; i++)
        {
            GameObject wolf = (GameObject)Instantiate(predator, transform);
            wolf.name = "(Predator)Family " + (i);
            wolf.GetComponent<PredatorController>().age = 10f;
            Random.InitState(initialSeed * i * i);
            float setRandom = UnityEngine.Random.Range(1f, 14f);
            wolf.GetComponent<PredatorController>().speed = setRandom;
            setRandom = UnityEngine.Random.Range(1f, 14f);
            wolf.GetComponent<PredatorController>().detectionradius = setRandom;
            posX = UnityEngine.Random.Range(0, (rows * 5));
            posZ = UnityEngine.Random.Range(-(cols * 5), 0);
            Random.InitState(initialSeed);
            wolf.transform.position = new Vector3(posX, 0.5f, posZ);
        }
    }

    void Update()
    {
        OnTimerEvent();
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private float interval = 7.0f;
    private void OnTimerEvent()
    {
        interval -= Time.deltaTime;
        if (interval < 0)
        {
            regeneratemap();
            interval = 5.0f;
        }
    }
}

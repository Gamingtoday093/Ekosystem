using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorController : MonoBehaviour
{
    public float detectionradius = 10f;
    public float speed = 5.0f;
    public float hunger = 1500;
    public float thirst = 1000;
    public string primary;
    public float desirability;
    public float Averagedesirability;
    private float storeddesirability;
    public float fertility;
    public float incubationperiod;
    public bool pregnant;
    public float age;
    private float interval;
    private float intervalage = 10f;
    private int initialSeed;
    private float fullspeed;
    private float fulldetectionradius;

    private float MyX;
    private float MyZ;
    private float FoodX;
    private float FoodZ;
    private float DiffX;
    private float DiffZ;
    private bool wandering = true;
    private int wanderX = 30;
    private int wanderZ = -30;
    private int rows;
    private int cols;
    Vector3 pos;
    Color prevcol;

    // Start is called before the first frame update
    void Start()
    {
        wanderX = UnityEngine.Random.Range(0, (rows * 5));
        wanderZ = UnityEngine.Random.Range(-(cols * 5), 0);
        wanderX = wanderX - (wanderX % 5);
        wanderZ = wanderZ - (wanderZ % 5);
        initialSeed = GameObject.Find("Map").GetComponent<Landgenerator>().seed;
        AssignSex();
        fullspeed = speed / (age / 10);
        fulldetectionradius = detectionradius / (age / 10);
        pos = transform.position;
        Get_WanderArea();
        Get_Diff();
        prevcol = this.gameObject.GetComponent<Renderer>().material.color;
        ColorHandler();
    }

    private void ColorHandler()
    {
        if (this.gameObject.tag == "Malepredator")
        {
            float factor = desirability;
            Color redfurr;
            redfurr = new Color(0, 0.05f, 0, 1);
            redfurr = (prevcol + (redfurr * factor)) / (2 - (1 - (factor / 10)));
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", redfurr);
        }
        else if (this.gameObject.tag == "Femalepredator")
        {
            if (pregnant == true)
            {
                Color pregnantcol;
                pregnantcol = new Color(0.25f, 0, 0.25f, 1);
                pregnantcol = (prevcol + pregnantcol) / 2;
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", pregnantcol);
            }
            else if (pregnant == false)
            {
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", prevcol);
            }
        }
    }

    public void GetPrimary()
    {
        if (hunger > 750 && thirst > 750 && age == 10)
        {
            switch (this.gameObject.tag)
            {
                case "Malepredator":
                    primary = "Femalepredator";
                    break;
                case "Femalepredator":
                    if (pregnant == false)
                    {
                        primary = "Malepredator";
                    }
                    break;
            }
        }
        else if (hunger > thirst)
        {
            primary = "Water";
        }
        else if (hunger < thirst)
        {
            primary = "Maleprey";
        }
    }

    private void AssignSex()
    {
        Random.InitState(initialSeed * (int)System.DateTime.Now.Ticks);
        float rndNumber = UnityEngine.Random.Range(this.gameObject.transform.position.x, this.gameObject.transform.position.z);
        rndNumber = Mathf.Abs(rndNumber - (rndNumber % 3));
        float rndNumbercalc = UnityEngine.Random.Range(0, rndNumber);
        if (rndNumbercalc <= (rndNumber / 2))
        {
            this.gameObject.tag = "Malepredator";
            rndNumber = Mathf.Clamp(rndNumber, 1, 10);
            desirability = rndNumber;
            fertility = 0;
            incubationperiod = 0;
        }
        else if (rndNumbercalc >= (rndNumber / 2))
        {
            this.gameObject.tag = "Femalepredator";
            rndNumber = Mathf.Clamp(rndNumber, 1, 10);
            if (Mathf.Ceil(rndNumbercalc) > 10)
            {
                rndNumbercalc = 10;
            }
            if (Mathf.Ceil(rndNumbercalc) < 1)
            {
                rndNumbercalc = 1;
            }
            fertility = Mathf.Ceil(rndNumbercalc);
            incubationperiod = Mathf.Abs(rndNumber);
            desirability = 0;
        }
    }

    private void Get_WanderArea()
    {
        rows = GameObject.Find("Map").GetComponent<Landgenerator>().rows;
        cols = GameObject.Find("Map").GetComponent<Landgenerator>().cols;
    }

    private int TotMale;

    private void Search()
    {
        GetPrimary();
        MyX = transform.position.x;
        MyZ = transform.position.z;
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(MyX, 0.5f, MyZ), detectionradius);
        List<Collider> legalhitColliders = new List<Collider>();
        TotMale = 0;
        Averagedesirability = 0;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Malepredator" && primary != "Water" && primary != "Maleprey")
            {
                Averagedesirability += hitCollider.GetComponent<PredatorController>().desirability;
                TotMale++;
                Averagedesirability = Mathf.Ceil(Averagedesirability / TotMale);
            }
            if (hitCollider.tag == primary)
            {
                if (primary == "Femalepredator")
                {
                    if (hitCollider.GetComponent<PredatorController>().pregnant == false && hitCollider.GetComponent<PredatorController>().age == 10)
                    {
                        if (desirability >= Averagedesirability)
                        {
                            legalhitColliders.Add(hitCollider);
                        }
                    }
                }
                else if (primary == "Malepredator" && hitCollider.GetComponent<PredatorController>().desirability >= Averagedesirability && hitCollider.GetComponent<PredatorController>().age == 10)
                {
                    legalhitColliders.Add(hitCollider);
                }
                else
                {
                    legalhitColliders.Add(hitCollider);
                }
            }
            else if (primary == "Maleprey" && hitCollider.tag == "Femaleprey")
            {
                legalhitColliders.Add(hitCollider);
            }
        }
        if (legalhitColliders.Count == 0)
        {
            if (wandering == false)
            {
                wanderX = UnityEngine.Random.Range(0, (rows * 5));
                wanderZ = UnityEngine.Random.Range(-(cols * 5), 0);
                wanderX = wanderX - (wanderX % 5);
                wanderZ = wanderZ - (wanderZ % 5);
                wandering = true;
            }
            else if (Mathf.Floor(MyX - wanderX) == 0 && Mathf.Floor(MyZ - wanderZ) == 0)
            {
                wandering = false;
            }
            FoodX = wanderX;
            FoodZ = wanderZ;
        }
        else if (legalhitColliders.Count != 0)
        {
            FoodX = legalhitColliders[0].transform.position.x;
            FoodZ = legalhitColliders[0].transform.position.z;
            if (Mathf.Floor(MyX - FoodX) == 0 && Mathf.Floor(MyZ - FoodZ) == 0)
            {
                float localdesirability;
                if (primary == "Malepredator" && this.gameObject.tag == "Femalepredator" && pregnant == false)
                {
                    localdesirability = legalhitColliders[0].GetComponent<PredatorController>().desirability;
                    if (localdesirability >= Averagedesirability)
                    {
                        storeddesirability = localdesirability;
                        pregnant = true;
                        legalhitColliders.Clear();
                        ColorHandler();
                        ispregnant();
                    }
                }
                if (primary == "Maleprey" || primary == "Femaleprey")
                {
                    hunger += 500;
                    Destroy(legalhitColliders[0].gameObject);
                }
                else if (primary == "Water")
                {
                    thirst += 500;
                    Destroy(legalhitColliders[0].gameObject);
                }
            }
        }
    }

    private void ispregnant()
    {
        if (pregnant == true)
        {
            interval = incubationperiod * 2;
        }
        else if (pregnant == false)
        {
            GameObject Child = GameObject.Find("Map").GetComponent<Landgenerator>().predator;
            float Children = UnityEngine.Random.Range(0, fertility);
            for (int i = 0; i < Children; i++)
            {
                Random.InitState(i);
                Child.name = (this.gameObject.name + " Child" + (i));
                Child = (GameObject)Instantiate(Child, transform.position, Quaternion.identity, GameObject.Find("Map").transform);
                var Childprop = Child.GetComponent<PredatorController>();
                float varies = UnityEngine.Random.Range(0.75f, 1.25f);
                Childprop.age = incubationperiod;
                Childprop.hunger = (hunger / 10) * incubationperiod;
                Childprop.thirst = (thirst / 10) * incubationperiod;
                Childprop.speed = (speed * varies) * (Childprop.age / 10);
                Childprop.detectionradius = (detectionradius * varies) * (Childprop.age / 10);
                Childprop.desirability = Mathf.Clamp((storeddesirability * varies), 0f, 10f);
                Childprop.fertility = Mathf.Clamp((fertility * varies), 0f, 10f);
                Childprop.incubationperiod = Mathf.Clamp((incubationperiod * varies), 0f, 10f);
                float scale = 0.5f * (1f + (incubationperiod / 10f));
                Child.transform.localScale = new Vector3(scale, scale, scale);
                storeddesirability = 0;
                ColorHandler();
            }
        }
    }

    private void Get_Diff()
    {
        MyX = transform.position.x;
        MyZ = transform.position.z;
        Search();
        DiffX = Mathf.Floor(MyX - FoodX);
        DiffZ = Mathf.Floor(MyZ - FoodZ);
    }

    void FixedUpdate()
    {
        Get_Diff();
        if (DiffX > 0)
        {
            Get_Diff();
            pos += Vector3.left;
        }
        else if (DiffX < 0)
        {
            Get_Diff();
            pos += Vector3.right;
        }
        else if (DiffX == 0)
        {
            pos = new Vector3(FoodX, 0.5f, FoodZ);
        }

        if (DiffZ > 0)
        {
            pos += Vector3.back;
            Get_Diff();
        }
        else if (DiffZ < 0)
        {
            pos += Vector3.forward;
            Get_Diff();
        }
        else if (DiffZ == 0)
        {
            pos = new Vector3(FoodX, 0.5f, FoodZ);
        }

        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        Get_Diff();

    }

    // Update is called once per frame
    void Update()
    {
        if (hunger <= 0)
        {
            this.transform.position = new Vector3(MyX, -1, MyZ);
            hunger = 0;
            Destroy(this.gameObject);
        }
        else
        {
            hunger -= 0.25f * ((1 + (detectionradius / 10)) + (Mathf.Pow(speed, 2) / 50));
        }
        if (thirst <= 0)
        {
            this.transform.position = new Vector3(MyX, -1, MyZ);
            thirst = 0;
            Destroy(this.gameObject);
        }
        else
        {
            thirst -= 0.5f * ((1 + (detectionradius / 10)) + (Mathf.Pow(speed, 2) / 50));
        }
        OnTimerEvent();
        OnAgeEvent();
    }

    private void OnTimerEvent()
    {
        if (pregnant == true)
        {
            interval -= Time.deltaTime;
            if (interval < 0)
            {
                pregnant = false;
                ispregnant();
                interval = 0;
            }
        }
    }

    private void OnAgeEvent()
    {
        if (age < 10)
        {
            intervalage -= Time.deltaTime;
            if (intervalage < 0)
            {
                age++;
                intervalage = 10f;
                speed = fullspeed * (age / 10);
                detectionradius = fulldetectionradius * (age / 10);
                float scale = 0.5f * (1f + (age / 10f));
                this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        else
        {
            age = 10;
            speed = fullspeed * (age / 10);
            float scale = 0.5f * (1f + (age / 10f));
            this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}

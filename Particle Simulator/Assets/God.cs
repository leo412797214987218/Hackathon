using UnityEngine;
using System.Collections.Generic;
using Random=UnityEngine.Random;

public class God : MonoBehaviour
{
    public GameObject RadioationTemplate;
    public int startingRadiation;
    public float speedOfLight;
    public GameObject particle;

    void Start()
    {
        for(int i = 0; i < startingRadiation; i++)
        {
            GameObject temp = Instantiate(RadioationTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            temp.GetComponent<Radiation>().speedOfLight = speedOfLight;
            temp.GetComponent<Radiation>().energy = (Random.Range(50f,60f)/100)*speedOfLight;
        } 
    }
}
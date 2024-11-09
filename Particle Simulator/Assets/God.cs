using UnityEngine;
using System.Collections.Generic;

public class God : MonoBehaviour
{
    public GameObject RadioationTemplate;
    public int startingRadiation;
    public float speedOfLight;
    public List<Particle> particleList;

    

    void Start()
    {
        for(int i = 0; i < startingRadiation; i++)
        {
            GameObject temp = Instantiate(RadioationTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            temp.GetComponent<Radiation>().speedOfLight = speedOfLight;
            temp.GetComponent<Radiation>().God = gameObject.GetComponent<God>();
        } 
    }

    void Update()
    {
        
    }
}

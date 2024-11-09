using UnityEngine;

public class God : MonoBehaviour
{
    public GameObject RadioationTemplate;
    public int startingRadiation;
    public float speedOfLight;

    

    void Start()
    {
        for(int i = 0; i < startingRadiation; i++)
        {
            GameObject temp = Instantiate(RadioationTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            temp.GetComponent<Radiation>().speedOfLight = speedOfLight;
        } 
    }

    void Update()
    {
        
    }
}

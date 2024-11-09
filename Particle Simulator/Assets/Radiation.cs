using System;
using UnityEngine;
using Random=UnityEngine.Random;

public class Radiation : MonoBehaviour
{
    public float energy;
    public Vector3 velocity;
    public float speedOfLight;
    public GameObject Particle;
    public int ChanceOfPP;
    public God God;

    void Start()
    {
        float x = Random.Range(-speedOfLight, speedOfLight);
        float y = ((float)Math.Sqrt(Math.Pow(speedOfLight,2) - Math.Pow(x,2))) * ((float)Math.Pow(-1,Random.Range(1,3)));
        if (Random.value > 0.5f){
            y = -y;
        }
        velocity = new Vector3(x, y, 0);
    }

    void FixedUpdate()
    {
        transform.position = transform.position + velocity;
        if (Random.Range(0, ChanceOfPP) == 1) 
        {
            PairProduction();
        }
    }

    public void PairProduction()
    {
        System.Random rnd = new System.Random();
        int number = rnd.Next(0, 5);
        int chance = rnd.Next(0, 100);

        if (number == 0)
        {
            GameObject tempElectron = Instantiate(Particle, transform.position, Quaternion.identity);
            float tempSpeed = Random.Range(10f, 90f) / (100f / speedOfLight) * Mathf.Pow(-1, Random.Range(1, 3));
            float tempX = Random.Range(-tempSpeed, tempSpeed);
            float tempY = (float)Math.Sqrt(Math.Pow(tempSpeed,2) - Math.Pow(tempX,2));
            tempElectron.GetComponent<Particle>().CreateParticle(new Vector3(tempX, tempY, 0), 0, -1, God);
            if (chance > 1)
            {
                GameObject tempPositron = Instantiate(Particle, transform.position, Quaternion.identity);
                tempPositron.GetComponent<Particle>().CreateParticle(new Vector3(-tempX, -tempY, 0), 0, 1, God);
            }
        }
        else if (number == 1 || number == 2)
        {
            // takes an up and anti-up quark
            GameObject tempUpQuark = Instantiate(Particle, transform.position, Quaternion.identity);
            float tempSpeed = Random.Range(10f, 90f) / (100f / speedOfLight) * Mathf.Pow(-1, Random.Range(1, 3));
            float tempX = Random.Range(-tempSpeed, tempSpeed);
            float tempY = (float)Math.Sqrt(Math.Pow(tempSpeed,2) - Math.Pow(tempX,2));
            tempUpQuark.GetComponent<Particle>().CreateParticle(new Vector3(tempX, tempY, 0), 2.4f, 2f/3f, God);
            if (chance > 1)
            {
                GameObject tempAntiUpQuark = Instantiate(Particle, transform.position, Quaternion.identity);
                tempAntiUpQuark.GetComponent<Particle>().CreateParticle(new Vector3(-tempX, -tempY, 0), 2.4f, -2f/3f, God);
            }
        }
        else if (number == 3 || number == 3)
        {
            // takes a down and anti-down quark
            GameObject tempDownQuark = Instantiate(Particle, transform.position, Quaternion.identity);
            float tempSpeed = Random.Range(10f, 90f) / (100f / speedOfLight) * Mathf.Pow(-1, Random.Range(1, 3));
            float tempX = Random.Range(-tempSpeed, tempSpeed);
            float tempY = (float)Math.Sqrt(Math.Pow(tempSpeed,2) - Math.Pow(tempX,2));
            tempDownQuark.GetComponent<Particle>().CreateParticle(new Vector3(tempX, tempY, 0), 4.8f, -1f/3f, God);
            if (chance > 1)
            {
                GameObject tempAntiDownQuark = Instantiate(Particle, transform.position, Quaternion.identity);
                tempAntiDownQuark.GetComponent<Particle>().CreateParticle(new Vector3(-tempX, -tempY, 0), 4.8f, 1f/3f, God);
            }
        }
        Destroy(gameObject);
    }

     
}

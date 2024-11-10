using System;
using UnityEngine;
using Random=UnityEngine.Random;

public class Radiation : MonoBehaviour
{
    public float energy;
    public Vector3 velocity;
    public float speedOfLight;
    public GameObject particlePrefab;
    public int pairProductionChance;
    private float speedOfLightSquared;

    void Start()
    {

        speedOfLightSquared = Mathf.Pow(speedOfLight, 2);

        //Randomly generate initial velocity
        float x = Random.Range(-speedOfLight, speedOfLight);
        float y = Mathf.Sqrt(speedOfLightSquared - Mathf.Pow(x, 2)) * (Random.value > 0.5f ? 1 : -1); //Ensures magnitude is the speed of light
        velocity = new Vector3(x, y, 0);
    }

    void FixedUpdate()
    {
        //If out of bounds, destroy
        if (transform.position.y > 105 || transform.position.y < -105)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x > 205 || transform.position.x < -205)
        {
            Destroy(gameObject);
        }

        //Regular movement, plus chance for pair production
        transform.position += velocity;
        if (Random.Range(0, pairProductionChance) == 1)
        {
            PairProduction();
        }
    }

    public void PairProduction()
    {
        //Controls what is produced from the pair production
        int particleType = Random.Range(0, 5);
        bool shouldCreateAntiparticle = Random.Range(0, 100) > 1;

        // Set particle properties based on the random type
        float mass, charge;
        switch (particleType)
        {
            case 0:  // Electron and Positron
                mass = 0f;
                charge = -1f;
                break;
            case 1:
            case 2:  // Up Quark and Anti-Up Quark
                mass = 2.4f;
                charge = 2f / 3f;
                break;
            case 3:
            case 4:  // Down Quark and Anti-Down Quark
                mass = 4.8f;
                charge = -1f / 3f;
                break;
            default:
                return;
        }

        // Calculate random velocity components
        float tempSpeed = energy * (Random.value > 0.5f ? 1 : -1) - Random.Range(1, 10) * speedOfLight / 100;
        float tempX = Random.Range(-tempSpeed, tempSpeed);
        float tempY = Mathf.Sqrt(tempSpeed * tempSpeed - tempX * tempX);

        // Create anti-antiparticle
        GameObject primaryParticle = Instantiate(particlePrefab, transform.position + new Vector3(2, 2, 0), Quaternion.identity);
        primaryParticle.GetComponent<Particle>().CreateParticle(new Vector3(tempX, tempY, 0), mass, charge, speedOfLight);

        // Probably make antiparticle
        if (shouldCreateAntiparticle)
        {
            GameObject antiParticle = Instantiate(particlePrefab, transform.position + new Vector3(-2, -2, 0), Quaternion.identity);
            antiParticle.GetComponent<Particle>().CreateParticle(new Vector3(-tempX, -tempY, 0), mass, -charge, speedOfLight);
        }

        // Destroy the radiation
        Destroy(gameObject);
    }

     
}
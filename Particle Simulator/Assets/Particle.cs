using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using UnityEngine;
using Vector3=UnityEngine.Vector3;
using Quaternion=UnityEngine.Quaternion;


public class Particle : MonoBehaviour
{
    public float mass;
    public float charge;
    public Vector3 velocity;
    public string type;
    public float AttarctionCheckDistance;
    public ContactFilter2D contactFilter = new ContactFilter2D();
    public Collider2D[] NearbyQuarks = new Collider2D[5];
    public float multiplier;
    public GameObject radiationprefab;
    public Vector3 particleScaling;
    public float lightSpeed;
    Vector3 pullforce;
    public GameObject particle;
    bool annihilated = false;


    void FixedUpdate()
    {
        //if out of bounds, kill
        if (transform.position.y > 105 || transform.position.y < -105)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y > 205 || transform.position.y < -205)
        {
            Destroy(gameObject);
        }


        //if breaching the laws of special relativity, slow it down
        if (velocity.magnitude > lightSpeed)
        {
            velocity = velocity * (lightSpeed * 0.95f / velocity.magnitude);
        }
        transform.position = transform.position + velocity;

        //Calculate the acceleration of each particle on a given frame based on the summative attraction to every "nearby" particle
        Physics2D.OverlapCircle(transform.position, AttarctionCheckDistance, contactFilter, NearbyQuarks);
        attraction(NearbyQuarks);
    }

    public void CreateParticle(Vector3 v, float m, float c, float l)
    {
        mass = m;
        charge= c;
        velocity= v;
        lightSpeed = l;

        //Colours!
        Color lightGreen;
        ColorUtility.TryParseHtmlString("#90EE90", out lightGreen);
        Color darkGreen;
        ColorUtility.TryParseHtmlString("#006400", out darkGreen);
        Color pink;
        ColorUtility.TryParseHtmlString("#FFC0CB", out pink);
        Color purple;
        ColorUtility.TryParseHtmlString("#800080", out purple);
        Color teal;
        ColorUtility.TryParseHtmlString("#008080", out teal);
        Color red;
        Color darkRed;
        ColorUtility.TryParseHtmlString("#FF0000", out red);
        ColorUtility.TryParseHtmlString("#8B0000", out darkRed);
        Color lightGrey;
        Color darkGrey;
        ColorUtility.TryParseHtmlString("#D3D3D3", out lightGrey);
        ColorUtility.TryParseHtmlString("#545454", out darkGrey);

        //Basing particle properties from its unique charge
        //(includes mass if charge not unique)
        if (charge==2/3f){
            type = "u";
            gameObject.GetComponent<SpriteRenderer>().color = lightGreen;
            gameObject.AddComponent<Quark>();
        }
        else if (charge==-2/3f){
            type = "au";
            gameObject.GetComponent<SpriteRenderer>().color = darkGreen;
            gameObject.AddComponent<Quark>();
        }
        else if (charge==-1/3f){
            type = "d";
            gameObject.GetComponent<SpriteRenderer>().color = pink;
            gameObject.AddComponent<Quark>();
        }
        else if (charge==1/3f){
            type = "ad";
            gameObject.GetComponent<SpriteRenderer>().color = purple;
            gameObject.AddComponent<Quark>();
        }
        else if (charge==1){
            if (mass == 0){
                type = "e";
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            } else{
                type = "p";
                gameObject.GetComponent<SpriteRenderer>().color = red;
                transform.localScale = particleScaling;
            }
        }
        else if (charge==-1){
            if (mass == 0){
                type = "ae";
                gameObject.GetComponent<SpriteRenderer>().color = teal;
            } else{
                type = "ap";
                gameObject.GetComponent<SpriteRenderer>().color = darkRed;
                transform.localScale = particleScaling;
            }
        }
        else if (charge==0){
            if (type == "n"){
                gameObject.GetComponent<SpriteRenderer>().color = lightGrey;
                transform.localScale = particleScaling;
            } else{
                gameObject.GetComponent<SpriteRenderer>().color = darkGrey;
                transform.localScale = particleScaling;
            }
        }
        
    }

    //Calculates the attraction to every nearby (for performance reasons) particle
    private void attraction(Collider2D[] particles)
    {
        pullforce = Vector3.zero;
        //for each particle, add its pull to the net pull
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i] != null && particles[i].gameObject.GetComponent<Particle>()!= null)
            {
                float distance = Vector3.Distance(particles[i].gameObject.GetComponent<Transform>().position, transform.position);
                //If the distance is 0 for any reason we get a million billion NaN errors unless we filter it 
                if (distance != 0)
                {
                    float attraction= (float)(charge * particles[i].gameObject.GetComponent<Particle>().charge) / (distance) * multiplier;
                    pullforce += (particles[i].gameObject.GetComponent<Transform>().position - transform.position).normalized * 
                    attraction;
                }
            }
        }
        velocity += pullforce * (0.02f-velocity.magnitude)/0.02f;

    }

    //The annihilation function
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision can only be 'hosted' by one participant particle or else we'd get double results
        //Double results leads to one wave -> two particles -> two waves -> four particles
        //Reverse entropy that crashes your laptop
        //So we use this clause to ensure that the slower particle alone is the host
        if (collision.gameObject.GetComponent<Particle>()== null || velocity.magnitude > collision.gameObject.GetComponent<Particle>().velocity.magnitude || annihilated)
        {
            return;
        }
        //Quark annihilation (one wave emitted)
        else if (collision.gameObject.GetComponent<Particle>().type == "u" && type == "au" || collision.gameObject.GetComponent<Particle>().type == "au" && type == "u" ||
        collision.gameObject.GetComponent<Particle>().type == "e" && type == "ae" || collision.gameObject.GetComponent<Particle>().type == "ae" && type == "e"
        || collision.gameObject.GetComponent<Particle>().type == "d" && type == "ad" || collision.gameObject.GetComponent<Particle>().type == "ad" && type == "d") 
        {
            annihilated = true;
            GameObject radiationObject = Instantiate(radiationprefab, transform.position, Quaternion.identity);
            radiationObject.GetComponent<Radiation>().energy = (velocity + collision.gameObject.GetComponent<Particle>().velocity).magnitude/2;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        //Hadron annihilation (three waves emitted, three quarks in a hadron)
        else if (collision.gameObject.GetComponent<Particle>().type == "p" && type == "ap" || collision.gameObject.GetComponent<Particle>().type == "ap" && type == "p" ||
        collision.gameObject.GetComponent<Particle>().type == "n" && type == "an" || collision.gameObject.GetComponent<Particle>().type == "an" && type == "n")
        {
            annihilated = true;
            GameObject radiationObject = Instantiate(radiationprefab, transform.position, Quaternion.identity);
            GameObject radiationObject2 = Instantiate(radiationprefab, transform.position, Quaternion.identity);
            GameObject radiationObject3 = Instantiate(radiationprefab, transform.position, Quaternion.identity);
            radiationObject.GetComponent<Radiation>().energy = (velocity + collision.gameObject.GetComponent<Particle>().velocity).magnitude/6;
            radiationObject2.GetComponent<Radiation>().energy = (velocity + collision.gameObject.GetComponent<Particle>().velocity).magnitude/6;
            radiationObject3.GetComponent<Radiation>().energy = (velocity + collision.gameObject.GetComponent<Particle>().velocity).magnitude/6;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        //Proton-electron combination into a neutron
        else if (collision.gameObject.GetComponent<Particle>().type == "p" && type == "e" || collision.gameObject.GetComponent<Particle>().type == "e" && type == "p")
        {
            annihilated = true;
            GameObject temp = Instantiate(particle, transform.position, Quaternion.identity);
            temp.GetComponent<Particle>().CreateParticle(velocity, 12f, 0f, gameObject.GetComponent<Particle>().lightSpeed);
            temp.GetComponent<Particle>().type = "n";
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        //Antimatter version of the last interaction
        else if (collision.gameObject.GetComponent<Particle>().type == "ap" && type == "ae" || collision.gameObject.GetComponent<Particle>().type == "ae" && type == "ap")
        {
            annihilated = true;
            GameObject temp = Instantiate(particle, transform.position, Quaternion.identity);
            temp.GetComponent<Particle>().CreateParticle(velocity, 12f, 0f, gameObject.GetComponent<Particle>().lightSpeed);
            temp.GetComponent<Particle>().type = "an";
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    } 
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using UnityEngine;
using Vector3=UnityEngine.Vector3;

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

    void FixedUpdate()
    {
        transform.position = transform.position + velocity;

        Physics2D.OverlapCircle(transform.position, AttarctionCheckDistance, contactFilter, NearbyQuarks);
        print(NearbyQuarks);
        attraction(NearbyQuarks);
    }

    public void CreateParticle(Vector3 v, float m, float c )
    {
        mass = m;
        charge= c;
        velocity= v;

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

        if (charge==2/3){
            type = "u";
            gameObject.GetComponent<SpriteRenderer>().color = lightGreen;
            gameObject.AddComponent<Quark>();
        }
        else if (charge==-2/3){
            type = "au";
            gameObject.GetComponent<SpriteRenderer>().color = darkGreen;
            gameObject.AddComponent<Quark>();
        }
        else if (charge==-1/3){
            type = "d";
            gameObject.GetComponent<SpriteRenderer>().color = pink;
            gameObject.AddComponent<Quark>();
        }
        else if (charge==1/3){
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
            }
        }
        else if (charge==-1){
            if (mass == 0){
                type = "ae";
                gameObject.GetComponent<SpriteRenderer>().color = teal;
            } else{
                type = "ap";
                gameObject.GetComponent<SpriteRenderer>().color = darkRed;
            }
        }
        else if (charge==0){
            //Neutron not defined
        }
        
    }


    private void attraction(Collider2D[] particles)
    {
        float force_x = 0;
        float force_y = 0;
        Vector3 pullforce = new Vector3(0,0,0);
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i] == null)
            {
                break;
            }    
                float distance = Vector3.Distance(particles[i].gameObject.GetComponent<Transform>().position, transform.position);
                //float attraction= (float)(charge * particles[i].gameObject.GetComponent<Particle>().charge) / (float)System.Math.Pow(Vector3.Distance(transform.position, particles[i].gameObject.transform.position),4);
                pullforce = (particles[i].gameObject.GetComponent<Transform>().position - transform.position).normalized/distance*multiplier;
                //force_x += transform.position.x * attraction;
                //force_x += transform.position.y * attraction;
        }

        float acc_x = (force_x / mass) * 100000000;
        float acc_y = (force_y / mass) * 100000000;

        velocity = velocity + pullforce;

    }
}
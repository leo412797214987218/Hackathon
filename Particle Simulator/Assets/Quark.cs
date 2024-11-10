using System.Linq;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class Quark : Particle
{
    public float StrongForce = 10f;
    public ContactFilter2D contactFilterQ = new ContactFilter2D();
    public GameObject particle;
    public God god;
    float tCharge;

    private void Start() {
        god = GameObject.Find("God").GetComponent<God>();
        particle = god.particle;
    }

    void FixedUpdate()
    {
        Collider2D[] NearbyQuarks = new Collider2D[2];

        Physics2D.OverlapCircle(transform.position, StrongForce, contactFilterQ, NearbyQuarks);
        try
        {
            tCharge = gameObject.GetComponent<Particle>().charge + NearbyQuarks[0].GetComponent<Particle>().charge + NearbyQuarks[1].GetComponent<Particle>().charge;
            if (tCharge==1)
            {
                //print("proton");
                GameObject temp = Instantiate(particle, transform.position, Quaternion.identity);
                Vector3 tempVel = (gameObject.GetComponent<Particle>().velocity + NearbyQuarks[0].GetComponent<Particle>().velocity + NearbyQuarks[1].GetComponent<Particle>().velocity)/3;
                temp.GetComponent<Particle>().CreateParticle(tempVel, 9.6f, 1f, gameObject.GetComponent<Particle>().lightSpeed);
                Destroy(NearbyQuarks[0]);
                Destroy(NearbyQuarks[1]);
                Destroy(gameObject);
            }
            else if (tCharge==-1)
            {
                //print("anti proton");
                GameObject temp = Instantiate(particle, transform.position, Quaternion.identity);
                Vector3 tempVel = (gameObject.GetComponent<Particle>().velocity + NearbyQuarks[0].GetComponent<Particle>().velocity + NearbyQuarks[1].GetComponent<Particle>().velocity)/3;
                temp.GetComponent<Particle>().CreateParticle(tempVel, 9.6f, -1f, gameObject.GetComponent<Particle>().lightSpeed);
                Destroy(NearbyQuarks[0]);
                Destroy(NearbyQuarks[1]);
                Destroy(gameObject);
            }
            else if (tCharge==0)
            {
                if (gameObject.GetComponent<Particle>().type.Contains('a'))
                {
                    //print("anti neutron");
                    GameObject temp = Instantiate(particle, transform.position, Quaternion.identity);
                    Vector3 tempVel = (gameObject.GetComponent<Particle>().velocity + NearbyQuarks[0].GetComponent<Particle>().velocity + NearbyQuarks[1].GetComponent<Particle>().velocity)/3;
                    temp.GetComponent<Particle>().CreateParticle(tempVel, 12f, 0f, gameObject.GetComponent<Particle>().lightSpeed);
                    temp.GetComponent<Particle>().type = "an";
                    Destroy(NearbyQuarks[0]);
                    Destroy(NearbyQuarks[1]);
                    Destroy(gameObject);
                }
                else
                {
                    //print("neutron");
                    GameObject temp = Instantiate(particle, transform.position, Quaternion.identity);
                    Vector3 tempVel = (gameObject.GetComponent<Particle>().velocity + NearbyQuarks[0].GetComponent<Particle>().velocity + NearbyQuarks[1].GetComponent<Particle>().velocity)/3;
                    temp.GetComponent<Particle>().CreateParticle(tempVel, 12f, 0f, gameObject.GetComponent<Particle>().lightSpeed);
                    temp.GetComponent<Particle>().type = "n";
                    Destroy(NearbyQuarks[0]);
                    Destroy(NearbyQuarks[1]);
                    Destroy(gameObject);
                }
            }
        }
        catch (System.Exception){}
    }
}
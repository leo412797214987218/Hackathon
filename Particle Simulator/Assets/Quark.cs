using UnityEngine;

public class Quark : Particle
{
    public float StrongForce;
    public ContactFilter2D contactFilter = new ContactFilter2D();
    public GameObject Hadron;

    void FixedUpdate()
    {
        /*Collider2D[] NearbyQuarks = new Collider2D[5];
        int number = Physics2D.OverlapCircle(transform.position, StrongForce, contactFilter, NearbyQuarks);
        if (number >= 2)
        {
            Destroy(NearbyQuarks[0].gameObject);
            Destroy(NearbyQuarks[1].gameObject);
            Instantiate(Hadron, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }*/
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class UniverseEdge : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Particle>())
        {
            Particle otherParticle = other.GetComponent<Particle>();
            Transform otherTransform = other.transform;

            if (otherTransform.position.y > 95 || otherTransform.position.y < -95)
            {
                otherParticle.velocity = new Vector3(otherParticle.velocity.x, -otherParticle.velocity.y, 0);
            }
            else{
                otherParticle.velocity = new Vector3(-otherParticle.velocity.x, otherParticle.velocity.y, 0);
            }
        }
        else{
            Radiation otherParticle = other.GetComponent<Radiation>();
            Transform otherTransform = other.transform;

            if (otherTransform.position.y > 95 || otherTransform.position.y < -95)
            {
                otherParticle.velocity = new Vector3(otherParticle.velocity.x, -otherParticle.velocity.y, 0);
            }
            else{
                otherParticle.velocity = new Vector3(-otherParticle.velocity.x, otherParticle.velocity.y, 0);
            }
        }
    }
}

using UnityEngine;

//Make sure there is always a ParticleSystem component on the GameObject where this script is added.
[RequireComponent(typeof(ParticleSystem))]
public class EmitParticlesOnHit : MonoBehaviour
{
    //Make the Number of particles to be emitted configurable in the editor. Default is set to 30.
    public int NumberOfParticles = 30;

    //OnCollisionEnter will only be called when the other collider has a rigidbody, like our ball has
    void OnCollisionEnter(Collision c)
    {
        GetComponent<ParticleSystem>().Emit(NumberOfParticles);
    }
}
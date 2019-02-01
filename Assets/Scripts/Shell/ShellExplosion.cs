using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask tankMask;
    public ParticleSystem explosionParticles;       
    public AudioSource explosionAudio;              
    public float maxDamage = 100f;                  
    public float explosionForce = 1000f;            
    public float maxLifeTime = 2f;                  
    public float explosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] reachedColliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);

        for (int i = 0; i < reachedColliders.Length; ++i)
        {
            Rigidbody targetRigidBody = reachedColliders[i].GetComponent<Rigidbody>();
            if (!targetRigidBody)
            {
                continue;
            }

            targetRigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            TankHealth targetHealth = targetRigidBody.GetComponent<TankHealth>();

            if (!targetHealth)
            {
                continue;
            }

            float damage = CalculateDamage(targetRigidBody.position);
            targetHealth.TakeDamage(damage);
        }

        explosionParticles.transform.parent = null;
        explosionParticles.Play();

        explosionAudio.Play();

        Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

        float damage = relativeDistance * maxDamage;

        damage = Mathf.Max(0f, damage);

        return damage;
    }
}
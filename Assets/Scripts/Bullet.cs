using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private float currentBulletDamage, currentBulletSpeed;
    private ParticleSystem currentBulletEffect;

    // Reference to BulletPool to get the necessary data (bullet damage, speed)
    private BulletPool bulletPool;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletPool = FindFirstObjectByType<BulletPool>(); // Find the BulletPool instance

        // The damage and speed will be set when the bullet is fired, not in Awake.
    }

    public void Fire(Vector3 direction, Vector3 position)
    {
        // Retrieve the bullet data from BulletPool when fired
        bulletPool.GetBulletData(out currentBulletDamage, out currentBulletSpeed, out currentBulletEffect, out _);

        // Set the bullet's position and velocity
        transform.position = position;
        rb.linearVelocity = direction * currentBulletSpeed;

        // Automatically disable after 3 seconds
        Invoke(nameof(Disable), 3f);
    }

    void Disable()
    {
        gameObject.SetActive(false); // Deactivate the bullet
    }

    void OnDisable()
    {
        CancelInvoke();                // Cancel the Invoke to Disable if the object is deactivated early
        rb.linearVelocity = Vector3.zero;    // Reset linear velocity
        rb.angularVelocity = Vector3.zero;  // Reset angular velocity
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentBulletDamage); // Deal damage to the enemy
            }
            // Instantiate the hit particle effect when the bullet hits an object
            if (currentBulletEffect != null)
            {
                // Get the collision point from the collision normal
                Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);

                // Instantiate the particle effect at the point of impact
                ParticleSystem effect = Instantiate(currentBulletEffect, hitPoint, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, 2f);
            }
        }

    }
}

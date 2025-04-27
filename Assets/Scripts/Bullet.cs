using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private float currentBulletDamage, currentBulletSpeed;

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
        bulletPool.GetBulletData(out currentBulletDamage, out currentBulletSpeed);

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
        }
    }
}

using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public ParticleSystem deathEffect;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        SoundManager.PlaySound(SoundType.ENEMYHIT, 0.45f);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, rb.position, Quaternion.identity);
        Destroy(gameObject);
    }
}


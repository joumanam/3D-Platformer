using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float health = 100f;

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
        Destroy(gameObject);
    }
}


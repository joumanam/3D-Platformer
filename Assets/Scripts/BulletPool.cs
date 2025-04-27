using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public BulletData bulletData;  // Reference to BulletData
    public int poolSize = 20;

    private List<GameObject> bullets;

    void Awake()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletData.bulletPrefab);
            bullet.SetActive(false);  // Deactivate the bullet initially
            bullets.Add(bullet);
        }
    }

    // Get a bullet from the pool and set its data (damage, speed)
    public GameObject GetBullet()
    {
        foreach (var bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet; // Return the bullet that was deactivated
            }
        }

        // If no bullets are available, log a warning and return null
        Debug.LogWarning("All bullets used! Consider increasing pool size.");
        return null;
    }

    // Get bullet data (damage, speed) to pass to the Bullet script when firing
    public void GetBulletData(out float damage, out float speed)
    {
        damage = bulletData.damage;
        speed = bulletData.speed;
    }
}

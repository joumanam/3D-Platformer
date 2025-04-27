using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    public Transform firePoint;
    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab;
    public float fireRate = 0.4f;
    private float nextFireTime = 0f;

    public BulletPool bulletPool; // Reference to the BulletPool

    public bool isShooting;
    public bool enableShooting;

    private void Update()
    {
        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void ToggleShooting(InputAction.CallbackContext context)
    {
        if (context.started && enableShooting)
        {
            isShooting = true;
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        SoundManager.PlaySound(SoundType.SHOOT, 0.35f);

        if (Physics.Raycast(ray, out hit, 100f)) // Using 100f as range for simplicity
        {
            targetPoint = hit.point;

            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, targetPoint, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 2f);
            }
        }
        else
        {
            targetPoint = ray.GetPoint(100f); // Again, using 100f as range for simplicity
        }

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        // Get a bullet from the pool and fire it
        GameObject bulletObj = bulletPool.GetBullet();
        if (bulletObj != null)
        {
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            bulletScript.Fire(direction, firePoint.position);
        }

        Debug.DrawLine(firePoint.position, targetPoint, Color.red, 1f);
    }
}

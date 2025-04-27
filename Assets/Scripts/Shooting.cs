using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    public Transform firePoint;
    public Camera cam;
    private float nextFireTime = 0f;
    public BulletPool bulletPool;
    public bool isShooting;
    public bool enableShooting;

    private void Update()
    {
        if (isShooting && Time.time >= nextFireTime)
        {
            bulletPool.GetBulletData(out _, out _, out _, out float fireRate);
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
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        SoundManager.PlaySound(SoundType.SHOOT, 0.35f);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
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

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

    public float range = 100f;
    public BulletPool bulletPool;
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

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // center of screen
        RaycastHit hit;
        Vector3 targetPoint;
        SoundManager.PlaySound(SoundType.SHOOT, 0.35f);

        if (Physics.Raycast(ray, out hit, range))
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
            targetPoint = ray.GetPoint(range); // Point far away in the same direction
        }

        GameObject bulletObj = bulletPool.GetBullet();
        if (bulletObj == null)
            return;

        bulletObj.transform.position = firePoint.position;

        // Get direction from firePoint to targetPoint
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.Fire(direction);

        Debug.DrawLine(firePoint.position, targetPoint, Color.red, 1f);
    }

}

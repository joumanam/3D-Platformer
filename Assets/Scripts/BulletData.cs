using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable Objects/BulletData")]
public class BulletData : ScriptableObject
{
    public float speed = 7f;
    public float damage = 10f;
    public float fireRate = 0.4f;
    public GameObject bulletPrefab;
    public ParticleSystem bulletEffect;
}

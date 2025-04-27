using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable Objects/BulletData")]
public class BulletData : ScriptableObject
{
    public float speed = 7f;
    public float damage = 10f;
    public GameObject bulletPrefab;
}

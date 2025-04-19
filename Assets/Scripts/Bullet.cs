using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 7f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 direction)
    {
        rb.linearVelocity = direction * speed; // Use rb.velocity instead of linearVelocity
        Invoke(nameof(Disable), 3f);
    }


    void Disable()
    {
        gameObject.SetActive(false); // this puts it back into the pool
    }

    void OnDisable()
    {
        CancelInvoke();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

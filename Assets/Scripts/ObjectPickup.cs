using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPickup : MonoBehaviour
{
    public Transform holdPosition;
    public float pickupRange = 3f;
    public float pickupAngleThreshold = 60f;
    public LayerMask pickupLayer;
    public LayerMask playerLayer;
    public GameObject heldObject;
    public GameObject pickableItemEffectPrefab;

    private Rigidbody heldObjectRb;
    private GameObject effect;
    private GameObject currentlyHighlighted;

    private void Update()
    {
        CheckForNearbyPickups();
    }

    public void HoldOrDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (heldObject)
            {
                DropObject();
            }
            else
            {
                TryPickup();
            }

        }
    }

    private Collider GetClosestPickupInView()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange, pickupLayer);

        Collider closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Pickup"))
            {
                Vector3 dir = (col.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, dir);

                if (angle <= pickupAngleThreshold)
                {
                    float dist = Vector3.Distance(transform.position, col.transform.position);
                    if (dist < closestDist)
                    {
                        closest = col;
                        closestDist = dist;
                    }
                }
            }
        }
        return closest;
    }

    public void TryPickup()
    {
        Collider closestObject = GetClosestPickupInView();

        if (closestObject != null)
        {
            heldObject = closestObject.gameObject;
            heldObjectRb = heldObject.GetComponent<Rigidbody>();
            heldObject.layer = LayerMask.NameToLayer("HeldObject");
            SoundManager.PlaySound(SoundType.HOLDITEM, 0.45f);
            int playerLayerIndex = (int)Mathf.Log(playerLayer.value, 2);
            int heldObjectLayerIndex = heldObject.layer;
            Physics.IgnoreLayerCollision(heldObjectLayerIndex, playerLayerIndex, true);

            // Reset physics
            heldObjectRb.linearVelocity = Vector3.zero;
            heldObjectRb.angularVelocity = Vector3.zero;
            heldObjectRb.useGravity = false;
            heldObjectRb.constraints = RigidbodyConstraints.FreezeAll;

            heldObject.transform.rotation = holdPosition.rotation;
            heldObject.transform.position = holdPosition.position;
            heldObject.transform.SetParent(holdPosition);
        }
    }


    public void DropObject()
    {
        if (heldObject != null)
        {
            SoundManager.PlaySound(SoundType.DROPITEM, 0.45f);
            // Detach from player
            heldObject.transform.SetParent(null);

            // Re-enable physics
            heldObjectRb.useGravity = true;
            heldObjectRb.isKinematic = false;
            heldObject.layer = (int)Mathf.Log(pickupLayer.value, 2);
            heldObjectRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            heldObject = null;
        }
    }

    private void CheckForNearbyPickups()
    {
        Collider closestObject = GetClosestPickupInView();

        if (closestObject != null)
        {
            if (currentlyHighlighted != closestObject.gameObject)
            {
                ClearHighlightedPickup();

                Transform existing = closestObject.transform.Find("PickupEffect");
                if (existing == null)
                {
                    GameObject effect = Instantiate(pickableItemEffectPrefab, closestObject.transform.position, Quaternion.identity, closestObject.transform);
                    effect.name = "PickupEffect";
                }
                currentlyHighlighted = closestObject.gameObject;
            }
        }
        else
        {
            ClearHighlightedPickup();
        }
    }


    private void ClearHighlightedPickup()
    {
        if (currentlyHighlighted != null)
        {
            Transform effect = currentlyHighlighted.transform.Find("PickupEffect");
            if (effect != null)
            {
                Destroy(effect.gameObject);
            }

            currentlyHighlighted = null;
        }
    }
}

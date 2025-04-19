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
    private Rigidbody heldObjectRb;

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

    public void TryPickup()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.transform.position, pickupRange, pickupLayer);

        Collider closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.transform.CompareTag("Pickup"))
            {
                Vector3 directionToObject = (collider.transform.position - transform.transform.position).normalized;
                float angleToObject = Vector3.Angle(transform.transform.forward, directionToObject);

                // Ensure object is within forward view range
                if (angleToObject <= pickupAngleThreshold)
                {
                    float distance = Vector3.Distance(transform.transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestObject = collider;
                    }
                }
            }
        }

        if (closestObject != null)
        {
            SoundManager.PlaySound(SoundType.HOLDITEM, 0.45f);
            heldObject = closestObject.transform.gameObject;
            heldObjectRb = heldObject.GetComponent<Rigidbody>();
            heldObject.layer = LayerMask.NameToLayer("HeldObject");
            int playerLayerIndex = (int)Mathf.Log(playerLayer.value, 2);
            int heldObjectLayerIndex = heldObject.layer;
            Physics.IgnoreLayerCollision(heldObjectLayerIndex, playerLayerIndex, true);

            // Reset velocity to prevent unintended motion
            heldObjectRb.linearVelocity = Vector3.zero;
            heldObjectRb.angularVelocity = Vector3.zero;

            // Disable physics while holding the object
            heldObjectRb.useGravity = false;
            heldObjectRb.constraints = RigidbodyConstraints.FreezeAll;

            // Attach the object to the hold position
            heldObject.transform.rotation = holdPosition.transform.rotation;
            heldObject.transform.position = holdPosition.transform.position;
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
}

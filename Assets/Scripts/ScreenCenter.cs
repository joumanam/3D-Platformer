using UnityEngine;

public class CameraCenterMarker : MonoBehaviour
{
    public Camera cam;
    public float distanceFromCamera = 2f;

    void Update()
    {
        if (cam != null)
        {
            transform.position = cam.transform.position + cam.transform.forward * distanceFromCamera;
            transform.rotation = cam.transform.rotation;
        }
    }
}

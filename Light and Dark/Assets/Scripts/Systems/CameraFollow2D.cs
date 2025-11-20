using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField]private Transform target;
    [SerializeField]private Vector3 offset;
    [SerializeField]private float damping;
    [SerializeField]private bool ignoreY;
    private Vector3 refVelocity;

    void FixedUpdate()
    {
        if(target == null) {return;}
        Vector3 newPosition = target.position + offset;
        if(ignoreY) {newPosition.y = offset.y;}

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref refVelocity, damping);
    }
}

using UnityEngine;

public class SurfaceSlider : MonoBehaviour
{
    private Vector3 _normal = Vector3.up;

    public Vector3 Project(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _normal).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _normal = collision.contacts[0].normal;
    }
    private void OnCollisionStay(Collision collision)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.3f)
            {
                sum += contact.normal;
                count++;
            }
        }

        if (count > 0)
            _normal = (sum / count).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + _normal * 3);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Project(transform.forward) * 3);
    }
}
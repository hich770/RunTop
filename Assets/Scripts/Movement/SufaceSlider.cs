using UnityEngine;

public class SufaceSlider : MonoBehaviour
{
    private Vector3 _normal;

    public Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(Vector3.forward, _normal) * _normal;
    }
}

using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;
    
    [SerializeField] private Transform _orientation;
    
    private float _xRotation;
    private float _yRotation;
    
    
    public Transform Orientation => _orientation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensY * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        _orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}

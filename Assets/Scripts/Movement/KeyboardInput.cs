using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PhysicsMovement _movement;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Animator _animator;
    
    private void Update()
    {
        float horizontal = Input.GetAxis(Axis.Horizontal);
        float vertical = Input.GetAxis(Axis.Vertical);

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);
        Vector3 moveDirection = _orientation.forward * inputDirection.z + _orientation.right * inputDirection.x;
        
        bool isMoving = inputDirection.magnitude > 0.1f;
        _animator.SetBool("IsRunning", isMoving);

        _movement.Move(moveDirection);
    }
}
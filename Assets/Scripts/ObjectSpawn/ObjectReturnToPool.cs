using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectReturnToPool : MonoBehaviour, IPooledObject
{
    [SerializeField] private float _minDistanceToDisable = 50f; 
    [SerializeField] private string _finishTag = "EndWall";

    private Transform _playerTransform;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void OnObjectSpawn()
    {
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {

        if (_playerTransform != null && 
            Vector3.Distance(transform.position, _playerTransform.position) > _minDistanceToDisable)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_finishTag))
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_finishTag))
        {
            ReturnToPool();
        }
    }
}

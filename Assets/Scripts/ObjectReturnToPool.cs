using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectReturnToPool : MonoBehaviour, IPooledObject
{
    [SerializeField] private float lifeTime = 10f; 
    [SerializeField] private float minDistanceToDisable = 50f; 
    [SerializeField] private string finishTag = "EndWall";

    private Transform playerTransform;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void OnObjectSpawn()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {

        if (playerTransform != null && 
            Vector3.Distance(transform.position, playerTransform.position) > minDistanceToDisable)
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
        if (collision.gameObject.CompareTag(finishTag))
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(finishTag))
        {
            ReturnToPool();
        }
    }
}

using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Параметры спавна")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(2, 0, 2);
    [SerializeField] private Color gizmosColor = new Color(0f, 1f, 0f, 0.3f);

    [Header("Настройки объектов")]
    [SerializeField] private string[] poolTags;
    [SerializeField] private bool useRandomRotation = true; //
    [SerializeField] private Vector2 rotationRange = new Vector2(0, 360); 
    [SerializeField] private bool useRandomScale = false;
    [SerializeField] private Vector2 scaleRange = new Vector2(0.8f, 1.2f);

    [Header("Оптимизация")]
    [SerializeField] private bool disableWhenNotVisible = true; 

    private float timer;
    private Camera mainCamera;
    private bool isVisible = true;
    private Plane[] cameraFrustum;
    private Bounds spawnBounds;

    private void Awake()
    {
        mainCamera = Camera.main;
        spawnBounds = new Bounds(spawnPoint.position, spawnAreaSize);
    }

    private void Start()
    {
        // Проверяем, есть ли ObjectPool на сцене
        if (ObjectPool.Instance == null)
        {
            Debug.LogError("На сцене нет ObjectPool! Добавьте компонент ObjectPool на пустой GameObject.");
        }
    }

    private void Update()
    {
        if (disableWhenNotVisible && mainCamera != null)
        {
            cameraFrustum = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            isVisible = GeometryUtility.TestPlanesAABB(cameraFrustum, spawnBounds);
            
            if (!isVisible) return;
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        if(poolTags == null || poolTags.Length == 0 || ObjectPool.Instance == null) return;
        
        string randomTag = poolTags[Random.Range(0, poolTags.Length)];
        
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Vector3 spawnPosition = spawnPoint.position + randomOffset;
        
        Quaternion rotation = Quaternion.identity;
        if (useRandomRotation)
        {
            float randomYRotation = Random.Range(rotationRange.x, rotationRange.y);
            rotation = Quaternion.Euler(0, randomYRotation, 0);
        }
        GameObject spawnedObject = ObjectPool.Instance.SpawnFromPool(randomTag, spawnPosition, rotation);


        if (useRandomScale && spawnedObject != null)
        {
            float randomScale = Random.Range(scaleRange.x, scaleRange.y);
            spawnedObject.transform.localScale = Vector3.one * randomScale;
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnPoint == null) return;
        
        Gizmos.color = gizmosColor;


        Gizmos.DrawCube(spawnPoint.position, spawnAreaSize);
        
        Gizmos.color = new Color(gizmosColor.r, gizmosColor.g, gizmosColor.b, 1f);
        Gizmos.DrawWireCube(spawnPoint.position, spawnAreaSize);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(spawnPoint.position, Vector3.down * 2);
    }
}
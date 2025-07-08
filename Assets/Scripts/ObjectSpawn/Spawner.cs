using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Параметры спавна")]
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(2, 0, 2);
    [SerializeField] private Color _gizmosColor = new Color(0f, 1f, 0f, 0.3f);

    [Header("Настройки объектов")]
    [SerializeField] private string[] _poolTags;
    [SerializeField] private bool _useRandomRotation = true; //
    [SerializeField] private Vector2 _rotationRange = new Vector2(0, 360); 
    [SerializeField] private bool _useRandomScale = false;
    [SerializeField] private Vector2 _scaleRange = new Vector2(0.8f, 1.2f);

    [Header("Оптимизация")]
    [SerializeField] private bool _disableWhenNotVisible = true; 

    private float _timer;
    private Camera _mainCamera;
    private bool _isVisible = true;
    private Plane[] _cameraFrustum;
    private Bounds _spawnBounds;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _spawnBounds = new Bounds(_spawnPoint.position, _spawnAreaSize);
    }

    private void Start()
    {
        if (ObjectPool.Instance == null)
        {
            Debug.LogError("На сцене нет ObjectPool! Добавьте компонент ObjectPool на пустой GameObject.");
        }
    }

    private void Update()
    {
        if (_disableWhenNotVisible && _mainCamera != null)
        {
            _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
            _isVisible = GeometryUtility.TestPlanesAABB(_cameraFrustum, _spawnBounds);
            
            if (!_isVisible) return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            SpawnObject();
            _timer = 0f;
        }
    }

    void SpawnObject()
    {
        if(_poolTags == null || _poolTags.Length == 0 || ObjectPool.Instance == null) return;
        
        string randomTag = _poolTags[Random.Range(0, _poolTags.Length)];
        
        Vector3 randomOffset = new Vector3(
            Random.Range(-_spawnAreaSize.x / 2, _spawnAreaSize.x / 2),
            0,
            Random.Range(-_spawnAreaSize.z / 2, _spawnAreaSize.z / 2)
        );

        Vector3 spawnPosition = _spawnPoint.position + randomOffset;
        
        Quaternion rotation = Quaternion.identity;
        if (_useRandomRotation)
        {
            float randomYRotation = Random.Range(_rotationRange.x, _rotationRange.y);
            rotation = Quaternion.Euler(0, randomYRotation, 0);
        }
        GameObject spawnedObject = ObjectPool.Instance.SpawnFromPool(randomTag, spawnPosition, rotation);


        if (_useRandomScale && spawnedObject != null)
        {
            float randomScale = Random.Range(_scaleRange.x, _scaleRange.y);
            spawnedObject.transform.localScale = Vector3.one * randomScale;
        }
    }

    private void OnDrawGizmos()
    {
        if (_spawnPoint == null) return;
        
        Gizmos.color = _gizmosColor;


        Gizmos.DrawCube(_spawnPoint.position, _spawnAreaSize);
        
        Gizmos.color = new Color(_gizmosColor.r, _gizmosColor.g, _gizmosColor.b, 1f);
        Gizmos.DrawWireCube(_spawnPoint.position, _spawnAreaSize);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_spawnPoint.position, Vector3.down * 2);
    }
}
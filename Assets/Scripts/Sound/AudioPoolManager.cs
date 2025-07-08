using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

class AudioPoolManager : MonoBehaviour
{
    public static AudioPoolManager Instance;

    [SerializeField] private AudioSource _pooledSourcePrefab; 
    [SerializeField] private int _poolSize = 10;
    private Queue<AudioSource> _pool = new Queue<AudioSource>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < _poolSize; i++)
        {
            var src = Instantiate(_pooledSourcePrefab, transform);
            src.playOnAwake = false;
            _pool.Enqueue(src);
        }
    }

    public void PlayAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (_pool.Count == 0) return;

        AudioSource src = _pool.Dequeue();
        src.transform.position = position;
        src.clip = clip;
        src.volume = volume;
        src.pitch = pitch;
        src.Play();

        StartCoroutine(ReturnToPool(src, clip.length));
    }

    private IEnumerator ReturnToPool(AudioSource src, float delay)
    {
        yield return new WaitForSeconds(delay);
        _pool.Enqueue(src);
    }
}
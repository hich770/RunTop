using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class AudioPoolManager : MonoBehaviour
{
    public static AudioPoolManager Instance;

    public AudioSource pooledSourcePrefab;
    public int poolSize = 10;
    private Queue<AudioSource> pool = new Queue<AudioSource>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            var src = Instantiate(pooledSourcePrefab, transform);
            src.playOnAwake = false;
            pool.Enqueue(src);
        }
    }

    public void PlayAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (pool.Count == 0) return;

        AudioSource src = pool.Dequeue();
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
        pool.Enqueue(src);
    }
}
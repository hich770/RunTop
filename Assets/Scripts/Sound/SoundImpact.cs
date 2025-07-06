using UnityEngine;

public class SoundImpact : MonoBehaviour
{
    public AudioClip[] hitClips;
    public float minVelocityToPlay = 2f;
    public float maxDistanceToPlayer = 30f;
    private static Transform _player;

    private bool hasPlayed = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasPlayed) return;

        if (collision.relativeVelocity.magnitude < minVelocityToPlay) return;
        
        if (_player == null)
            _player = GameObject.FindWithTag("Player")?.transform;

        if (_player == null) return;

        if (Vector3.Distance(transform.position, _player.position) > 30f) return;
        
        float dist = Vector3.Distance(transform.position, _player.position);
        if (dist > maxDistanceToPlayer) return;

        var clip = hitClips[Random.Range(0, hitClips.Length)];
        float pitch = Random.Range(0.9f, 1.1f);

        AudioPoolManager.Instance.PlayAtPosition(clip, transform.position, 1f, pitch);
        hasPlayed = true;
    }
}

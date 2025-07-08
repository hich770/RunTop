using UnityEngine;
using UnityEngine.Serialization;

public class SoundImpact : MonoBehaviour
{
    [FormerlySerializedAs("hitClips")] [SerializeField] private AudioClip[] _hitClips;
    [FormerlySerializedAs("minVelocityToPlay")] [SerializeField] private float _minVelocityToPlay = 2f;
    [FormerlySerializedAs("maxDistanceToPlayer")] [SerializeField] private float _maxDistanceToPlayer = 30f;
    private static Transform _player;

    private bool _hasPlayed = false;

    void OnCollisionEnter(Collision collision)
    {
        if (_hasPlayed) return;

        if (collision.relativeVelocity.magnitude < _minVelocityToPlay) return;
        
        if (_player == null)
            _player = GameObject.FindWithTag("Player")?.transform;

        if (_player == null) return;

        if (Vector3.Distance(transform.position, _player.position) > 30f) return;
        
        float dist = Vector3.Distance(transform.position, _player.position);
        if (dist > _maxDistanceToPlayer) return;

        var clip = _hitClips[Random.Range(0, _hitClips.Length)];
        float pitch = Random.Range(0.9f, 1.1f);

        AudioPoolManager.Instance.PlayAtPosition(clip, transform.position, 1f, pitch);
        _hasPlayed = true;
    }
}

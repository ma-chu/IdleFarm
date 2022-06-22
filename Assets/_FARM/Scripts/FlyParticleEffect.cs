using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FlyParticleEffect : MonoBehaviour
{
    [SerializeField] private float minSpeed = 0.8f;
    [SerializeField] private float maxSpeed = 1.3f;
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private float minbarrel = -2f;
    [SerializeField] private float maxbarrel = 2f;
    [SerializeField] private bool dynamicTarget = false;
    
    private Image _image;
    private SpriteRenderer _spriteRenderer;
    private float _currTime;
    private Vector3 _source;
    private Vector3 _target;
    private Transform _targetTransform;
    private Vector3 _mid;

    private enum Mode { idle, flight, fade }
    private Mode _mode = Mode.idle;
    private float _speed;
    private Action _onComplete;

    public void Init(Transform source, Transform target, Action onComplete = null)
    {
        _image = GetComponent<Image>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _speed = Random.Range(minSpeed, maxSpeed);
        _source = source.position;
        _target = target.position;
        if (dynamicTarget) _targetTransform = target;
        _mid = (_source + _target) / 2 + Vector3.up*Random.Range(minbarrel, maxbarrel);
        _mode = Mode.flight;
        _onComplete = onComplete;
    }

    private void Update()
    {
        if (_mode == Mode.flight)
        {
            if (_currTime < 1f)
            {
                if (dynamicTarget) _target = _targetTransform.position;
                transform.position = EaseB(_source, _mid, _target, _currTime);
                _currTime += Time.deltaTime * _speed;
                if (_currTime >= 1f)
                {
                    FadeOut();
                }
            }
        }

        if (_mode == Mode.fade)
        {
            if (_currTime < 1f)
            {
                if (_image != null)
                {
                    var c = _image.color;
                    c.a = 1f - _currTime;
                    _image.color = c;
                }
                if (_spriteRenderer != null)
                {
                    var c = _spriteRenderer.color;
                    c.a = 1f - _currTime;
                    _spriteRenderer.color = c;
                }

                _currTime += Time.deltaTime * fadeSpeed;
                if (_currTime >= 1f)
                {
                    _mode = Mode.idle;
                    _onComplete?.Invoke();
                    Destroy(gameObject);
                }
            }
        }
    }
    private void FadeOut()
    {
        _mode = Mode.fade;
        _currTime = 0;
    }
    private static Vector3 EaseB(Vector3 v1, Vector3 v2, Vector3 v3, float t)
    {
        var omt = 1f - t;
        var q= omt * omt * v1 + 2f * omt * t * v2 + t * t * v3;
        return q;
    }
}

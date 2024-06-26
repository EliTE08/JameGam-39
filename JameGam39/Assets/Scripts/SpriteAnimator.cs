using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private bool playOnAwake;

    [Space(3)] [SerializeField, FormerlySerializedAs("spriteAnimationSets")]
    private List<SpriteSet> animations;

    private SpriteSet _currentAnimation;
    private int _animationIndex;
    private float _timer;

    private bool _isPlaying;

    private SpriteRenderer _spriteRenderer;

    public Action OnComplete;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SwitchAnimation(animations[0]);
        if (playOnAwake)
            Play();
    }

    private void Update()
    {
        if (_isPlaying)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                if (_animationIndex < _currentAnimation.animationFrames.Count - 1)
                    _animationIndex++;
                else if (_animationIndex >= _currentAnimation.animationFrames.Count - 1)
                {
                    if (_currentAnimation.loop)
                        _animationIndex = 0;
                    else
                    {
                        if(_currentAnimation.nextAnimation != "")
                            SwitchAnimation(_currentAnimation.nextAnimation);
                    }
                    OnComplete?.Invoke();
                }

                ResetTimer();
            }

            if (_animationIndex < _currentAnimation.animationFrames.Count)
                _spriteRenderer.sprite = _currentAnimation.animationFrames[_animationIndex];
        }
    }

    public void SwitchAnimation(int index)
    {
        if (_currentAnimation == animations[index])
            return;
        SwitchAnimation(animations[index]);
    }

    public void SwitchAnimation(SpriteSet animation)
    {
        if (_currentAnimation == animation)
            return;
        ResetTimer(animation);
        _animationIndex = 0;
        _currentAnimation = animation;
    }

    public void SwitchAnimationSeamless(SpriteSet animation)
    {
        if (_currentAnimation == animation)
            return;
        ResetTimer(animation);
        _currentAnimation = animation;
    }

    public void SwitchAnimation(string animation)
    {
        if (_currentAnimation.name == animation)
            return;
        foreach (var anim in animations)
        {
            if (anim.name == animation)
            {
                SwitchAnimation(anim);
                break;
            }
        }
    }

    public void SwitchAnimationSeamless(string animation)
    {
        if (_currentAnimation.name == animation)
            return;
        foreach (var anim in animations)
        {
            if (anim.name == animation)
            {
                SwitchAnimationSeamless(anim);
                break;
            }
        }
    }

    public string GetCurrentAnimation()
    {
        return _currentAnimation.name;
    }

    public void Play() => _isPlaying = true;
    public void Pause() => _isPlaying = false;

    private void ResetTimer()
    {
        ResetTimer(_currentAnimation);
    }

    private void ResetTimer(SpriteSet spriteSet)
    {
        _timer = 1f / spriteSet.speed;
    }
}

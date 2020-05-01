using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float animationSpeed = 0.1f;
    [SerializeField] private List<Sprite> directionArrowAnimation;
    private int _animationIndex = 0;

    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _animationIndex = 0;
        coroutine = PlayAnimation();
        if (coroutine != null) StartCoroutine(coroutine);
    }

    private void OnDisable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    private IEnumerator PlayAnimation()
    {
        _spriteRenderer.sprite = directionArrowAnimation[_animationIndex];

        _animationIndex++;
        if (_animationIndex >= directionArrowAnimation.Count)
        {
            _animationIndex = 0;
        }
        
        yield return new WaitForSecondsRealtime(animationSpeed);
        
        StartCoroutine(PlayAnimation());
    }
}

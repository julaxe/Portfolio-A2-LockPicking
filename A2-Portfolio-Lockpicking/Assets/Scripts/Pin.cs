using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pin : MonoBehaviour
{
    public enum PinState
    {
        Unchanged,
        Green,
        Red
    }
    public Transform initialPosition;
    public Transform greenPosition;
    public Transform topLimit;
    public PinState state;

    private float _greenMargin;
    [SerializeField] private float _maxGreenMargin = 0.1f;
    [SerializeField] private float _minGreenMargin = 0.01f;
    private SpriteRenderer _spriteRenderer;
    private PlayerSkill _playerSkill;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerSkill = FindObjectOfType<PlayerSkill>();

        
        SetGreenSpot();
        InitialState();
    }

    // Update is called once per frame
    void Update()
    {
        SetGreenMargin();
        // (greenPos - margin, greenPos + margin)
        if (transform.position.y >= greenPosition.position.y - _greenMargin && transform.position.y <= greenPosition.position.y + _greenMargin)
        {
            SetState(PinState.Green);
        }
        else if (transform.position.y > greenPosition.position.y + _greenMargin)
        {
            SetState(PinState.Red);
        }
        else
        {
            SetState(PinState.Unchanged);
        }
    }

    private void LateUpdate()
    {
        if (transform.position.y >= topLimit.position.y)
        {
            transform.position = topLimit.position;
        }
    }

    private void SetGreenMargin()
    {
        _greenMargin = _minGreenMargin + (_playerSkill.skill * (_maxGreenMargin-_minGreenMargin)); // min -> 0.01f / max -> 0.1f
    }
    private void SetGreenSpot()
    {
        //is going to be a random spot for the green position, between the initial pos and the top limit.
        float min = initialPosition.position.y;
        float max = topLimit.position.y;
        float randomY = Random.Range(min + _maxGreenMargin, max - _maxGreenMargin);
        greenPosition.position = new Vector3(greenPosition.position.x, randomY, greenPosition.position.z);
    }
    public void InitialState()
    {
        SetState(PinState.Unchanged);
        transform.position = initialPosition.position;
    }

    private void SetState(PinState newState)
    {
        state = newState;
        switch (state)
        {
            case PinState.Green:
                _spriteRenderer.color = Color.green;
                break;
            case PinState.Red:
                _spriteRenderer.color = Color.red;
                break;
            default:
                _spriteRenderer.color = Color.white;
                break;
        }
    }
}

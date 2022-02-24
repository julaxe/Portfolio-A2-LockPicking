using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickController : MonoBehaviour
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    public GameDifficulty difficulty;
    public GameObject pickTool;
    public GameObject gameOverScreen;
    public GameObject unlockedScreen;

    private readonly int _socketQuantity = 5;
    private GameObject _pinSocketRef;
    private List<GameObject> _pinSockets;
    private List<Pin> _pins;
    private int _currentPinsInUse;
    private int _currentPin = 0;
    private float _pinVelocity = 0.0f;
    private bool _isPicking;
    private TimeLimit _timeLimit;
    void Start()
    {
        _pinSocketRef = Resources.Load<GameObject>("Prefabs/PinSocket");
        _pins = new List<Pin>();
        _pinSockets = new List<GameObject>();
        _timeLimit = FindObjectOfType<TimeLimit>();
        pickTool.SetActive(false);
        
        InitializePinSockets();
        SetDifficulty(GameDifficulty.Easy);
    }

    private void Update()
    {
        if (_isPicking)
        {
            UpdateCurrentPin();
            UpdatePickTool();
        }

        if (_timeLimit.timer <= 0)
        {
            //gameover
            gameOverScreen.SetActive(true);
        }
    }

    private void InitializePinSockets()
    {
        float spaceBetweenSockets = 1.5f;
        for (int i = 0; i < _socketQuantity; i++)
        {
            var temp = Instantiate(_pinSocketRef, transform);
            temp.transform.position += new Vector3(-spaceBetweenSockets * i, 0, 0);
            _pins.Add(temp.GetComponentInChildren<Pin>());
            _pinSockets.Add(temp);
        }
    }

    public void OnMove(InputValue value)
    {
        var velocity = value.Get<Vector2>();
        _pinVelocity = velocity.y <= 0 ? 0.0f : velocity.y;
    }

    public void OnSwitch(InputValue value)
    {
        if (!_isPicking) return;
        //it has to be picking to switch
        if (_pins[_currentPin].state == Pin.PinState.Green)
        {
            NextPin();
            return;
        }
        PreviousPin();
    }
    public void OnPicking(InputValue value)
    {
        _isPicking = value.isPressed;
        Cursor.visible = !_isPicking;
        pickTool.SetActive(_isPicking);
        
        if (_isPicking) return;
        //check if all of them are green.
        if (_currentPin == _currentPinsInUse - 1)
        {
            //we are in the last pin
            if (_pins[_currentPin].state == Pin.PinState.Green)
            {
                //is green
                unlockedScreen.SetActive(true);
            }
            else
            {
                ResetPins();
            }
        }
        else
        {
            //we are not in the last pin so restart.
            ResetPins();
        }
    }
    private void UpdateCurrentPin()
    {
        _pins[_currentPin].transform.position += new Vector3(0.0f, _pinVelocity * Time.deltaTime, 0.0f);
    }

    private void UpdatePickTool()
    {
        //pick position = pin - y off set.
        float yOffSet = 1.0f;
        Vector3 newPickPosition = new Vector3(_pins[_currentPin].transform.position.x,
            _pins[_currentPin].transform.position.y - yOffSet, 0.0f);
        pickTool.transform.position = newPickPosition;
    }

    private void NextPin()
    {
        if (_currentPin + 1 >= _currentPinsInUse) return;
        _currentPin += 1;
    }

    private void PreviousPin()
    {
        //reset actual pin
        _pins[_currentPin].InitialState();
        //go to previous and reset it
        if (_currentPin == 0) return;
        _currentPin -= 1;
        _pins[_currentPin].InitialState();
    }

    private void ResetPins()
    {
        for (int i = 0; i < _currentPinsInUse; i++)
        {
            _pins[i].InitialState();
        }

        _currentPin = 0;
    }
    public void SetDifficulty(GameDifficulty newDifficulty)
    {
        difficulty = newDifficulty;
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                _currentPinsInUse = 3;
                break;
            case GameDifficulty.Normal:
                _currentPinsInUse = 4;
                break;
            case GameDifficulty.Hard:
                _currentPinsInUse = 5;
                break;
        }

        for (int i = 0; i < _socketQuantity; i++)
        {
            if (i < _currentPinsInUse)
            {
                _pinSockets[i].SetActive(true);
                continue;
            }
            _pinSockets[i].SetActive(false);
        }
    }
}

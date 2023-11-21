using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private PlayerHand _playerHand;

    public Camera MainCamera => _mainCamera;

    public GameObject Player => _player;

    public PlayerHand PlayerHand => _playerHand;

    private void Awake()
    {
        Instance = this;
    }
}

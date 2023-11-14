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

    public Camera MainCamera => _mainCamera;

    public GameObject Player => _player;

    private void Awake()
    {
        Instance = this;
    }
}

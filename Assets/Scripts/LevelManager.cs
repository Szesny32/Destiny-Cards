using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private Camera _mainCamera;

    public Camera MainCamera => _mainCamera;

    private void Awake()
    {
        Instance = this;
    }
}

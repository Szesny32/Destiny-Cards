using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractiveObjectType
{
    Box,
    Chest,
    Door,
    Barrel,
}

public class InteractiveObject : MonoBehaviour
{
    public InteractiveObjectType Type;
}

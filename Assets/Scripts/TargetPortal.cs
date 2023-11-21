using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MagePlayerController magePlayer = other.gameObject.GetComponent<MagePlayerController>();
        if (magePlayer != null)
        {
            GameManager.Instance.NextLevel();
        }
    }
}

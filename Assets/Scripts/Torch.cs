using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField]
    private GameObject _firePointLight;

    [SerializeField]
    private ParticleSystem _fireLightEffect;

    [SerializeField]
    private bool _turnOnStart = false;

    private void Start()
    {
        if (_turnOnStart)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void TurnOn()
    {
        _firePointLight.SetActive(true);
        _fireLightEffect.Play();
    }

    public void TurnOff()
    {
        _firePointLight.SetActive(false);
        _fireLightEffect.Stop();
    }
}

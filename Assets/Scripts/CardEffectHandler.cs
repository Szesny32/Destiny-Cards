using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardEffectHandler
{
    void Handle(GameObject targetGameObject, InteractiveObjectType targetObjectType);
}

public class PullEffectHandler : ICardEffectHandler
{
    public void Handle(GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        Rigidbody rb = targetGameObject.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 500, 0));
    }
}

public class FireballEffectHandler : ICardEffectHandler
{
    public void Handle(GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {

    }
}

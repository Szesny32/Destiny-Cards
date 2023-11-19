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
        if(targetObjectType != InteractiveObjectType.Player){
            Object.Destroy(targetGameObject, 0.5f);
        }
    }
}

public class ResizeUpEffectHandler : ICardEffectHandler
{
    public void Handle(GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        targetGameObject.transform.localScale = 2f * targetGameObject.transform.localScale;
    }
}


public class ResizeDownEffectHandler : ICardEffectHandler
{
    public void Handle(GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        targetGameObject.transform.localScale = 0.5f * targetGameObject.transform.localScale;
    }
}

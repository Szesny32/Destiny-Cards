using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICardEffectHandler 
{
    abstract public void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType);
    public void ActivateSpell(GameObject spellPrefab, GameObject targetGameObject, float effectScale = 1f){
        var effectObject = GameObject.Instantiate(spellPrefab);
        effectObject.transform.position = targetGameObject.transform.position;
        effectObject.transform.localScale = effectScale * targetGameObject.transform.localScale;
        GameObject.Destroy(effectObject, effectObject.GetComponent<ParticleSystem>().main.duration);
    }
}


public class PullEffectHandler : ICardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        if(targetObjectType != InteractiveObjectType.Door){
            base.ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
            Rigidbody rb = targetGameObject.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0, 500, 0));
        }
    }
}

public class FireballEffectHandler : ICardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        if(targetObjectType != InteractiveObjectType.Player){
            base.ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
            base.ActivateSpell(card.CardDescriptor.SecondEffectPrefab, targetGameObject, 0.2f);
            Object.Destroy(targetGameObject, 4f);
        }
    }
}

public class ResizeUpEffectHandler : ICardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        base.ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
        targetGameObject.GetComponent<InteractiveObject>().Rescale(2f);
    }
}


public class ResizeDownEffectHandler : ICardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        base.ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
        targetGameObject.GetComponent<InteractiveObject>().Rescale(0.5f);
    }
}

public class SpectralVisionEffectHandler : ICardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        InteractiveObject[] objects = GameObject.FindObjectsOfType<InteractiveObject>();

        foreach (InteractiveObject interactiveObject in objects){
            interactiveObject.TurnSpectralVisionEffect();
        }
    }
}


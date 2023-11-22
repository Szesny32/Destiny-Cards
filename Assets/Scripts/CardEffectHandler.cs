using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffectHandler 
{
    abstract public void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType);
    public void ActivateSpell(GameObject spellPrefab, GameObject targetGameObject, float effectScale = 1f){
        var effectObject = GameObject.Instantiate(spellPrefab);
        effectObject.transform.position = targetGameObject.transform.position;
        effectObject.transform.localScale = effectScale * targetGameObject.transform.localScale;
        GameObject.Destroy(effectObject, effectObject.GetComponent<ParticleSystem>().main.duration);
    }
}


public class PullEffectHandler : CardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        if(targetObjectType != InteractiveObjectType.Door){
            base.ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
            Rigidbody rb = targetGameObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, 10.0f, rb.velocity.z);
        }
    }
}

public class FireballEffectHandler : CardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        if (targetObjectType == InteractiveObjectType.Torch)
        {
            Torch torch = targetGameObject.GetComponent<Torch>();
            ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
            torch.TurnOn();
        }
        else if (targetObjectType != InteractiveObjectType.Player){
            base.ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
            base.ActivateSpell(card.CardDescriptor.SecondEffectPrefab, targetGameObject, 0.2f);
            Object.Destroy(targetGameObject, 4f);
        }
    }
}

public class ResizeUpEffectHandler : CardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        base.ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
        targetGameObject.GetComponent<InteractiveObject>().Rescale(2f);

        Rigidbody targetRigidbody = targetGameObject.GetComponent<Rigidbody>();
        if (targetRigidbody != null)
        {
            targetRigidbody.mass *= 10.0f;
        }
    }
}


public class ResizeDownEffectHandler : CardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        base.ActivateSpell(card.CardDescriptor.EffectPrefab, targetGameObject);
        targetGameObject.GetComponent<InteractiveObject>().Rescale(0.5f);

        Rigidbody targetRigidbody = targetGameObject.GetComponent<Rigidbody>();
        if (targetRigidbody != null)
        {
            targetRigidbody.mass *= 0.1f;
        }
    }
}

public class SpectralVisionEffectHandler : CardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        InteractiveObject[] objects = GameObject.FindObjectsOfType<InteractiveObject>();

        foreach (InteractiveObject interactiveObject in objects){
            interactiveObject.TurnSpectralVisionEffect();
        }
    }
}

public class OpenEffectHandler : CardEffectHandler
{
    public override void Handle(CardInHand card, GameObject targetGameObject, InteractiveObjectType targetObjectType)
    {
        if (targetObjectType == InteractiveObjectType.Chest)
        {
            var chest = targetGameObject.GetComponent<Chest>();
            chest.Open();
        }
        else if (targetObjectType == InteractiveObjectType.Door)
        {
            var door = targetGameObject.GetComponent<Door>();
            door.Open();
        }
    }
}
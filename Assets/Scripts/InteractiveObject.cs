using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractiveObjectType
{
    Box,
    Chest,
    Door,
    Barrel,
    Player,
}

public enum HiglightType
{
    None,
    Target,
    All,
}


public class InteractiveObject : MonoBehaviour
{
    HiglightType highlightState = HiglightType.None;

    private float sustainingTargetingTime = 0.125f;
    private float targetingTimeElapsed = 0f;
    private bool displayAllTargets = false; 

    public InteractiveObjectType Type;
    private GameObject targetEffect;
    private GameObject altEffect;
    private GameObject effect;
    public float effectScale = 0.5f;

    public void Start(){
        targetEffect = Resources.Load<GameObject>("Magic circle");
        altEffect = Resources.Load<GameObject>("Magic circle 2");
    }


    public void Update(){
        displayAllTargets = Input.GetKey(KeyCode.LeftAlt);

        VerifyHighlightState();
        targetingTimeElapsed -= Time.deltaTime;
    }

    public void TurnOutlineEffect(){
        targetingTimeElapsed = sustainingTargetingTime;
    }

    private void VerifyHighlightState(){
        if(highlightState != HiglightType.Target && targetingTimeElapsed > 0f){
            highlightState = HiglightType.Target;
            Destroy(effect);
            effect = Instantiate(targetEffect, this.transform);
            effect.transform.localScale = effectScale * Vector3.one;
        } 
        else if(highlightState == HiglightType.Target && targetingTimeElapsed < 0f){
            highlightState = displayAllTargets ? HiglightType.All : HiglightType.None;
            Destroy(effect);
            if(highlightState == HiglightType.All){
                effect = Instantiate(altEffect, this.transform);
                effect.transform.localScale = effectScale * Vector3.one;
            }
            
        } 
        else if(highlightState == HiglightType.None && displayAllTargets){

            highlightState = HiglightType.All;
            Destroy(effect);
            effect = Instantiate(altEffect, this.transform);
            effect.transform.localScale = effectScale * Vector3.one;
            
        }
        else if(highlightState == HiglightType.All && !displayAllTargets){
            highlightState = HiglightType.None;
            Destroy(effect);
        }
    }
}

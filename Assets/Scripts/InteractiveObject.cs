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
    public float targetingTimeElapsed = 0f;
    public bool displayAllTargets = false; 

    public InteractiveObjectType Type;

    private Material[] baseMaterials;
    private Material[] normalMaterials;
    private Material[] altMaterials;
    private MeshRenderer meshRenderer;

    public void Start(){
        meshRenderer =  GetComponent<MeshRenderer>();
        baseMaterials = meshRenderer.materials;
        normalMaterials = SetMaterial("Materials/Outline");
        altMaterials = SetMaterial("Materials/AltOutline");
    }

    private Material[] SetMaterial(string materialPath){
        Material[] materials = new Material[baseMaterials.Length + 1];
        System.Array.Copy(baseMaterials, materials, baseMaterials.Length);
        Material outline = Resources.Load<Material>(materialPath);
        materials[materials.Length - 1] = outline;
        return materials;
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
            meshRenderer.materials = normalMaterials;
            highlightState = HiglightType.Target;
        } 
        else if(highlightState == HiglightType.Target && targetingTimeElapsed < 0f){
            meshRenderer.materials = displayAllTargets ? altMaterials : baseMaterials;
            highlightState = displayAllTargets ? HiglightType.All : HiglightType.None;
        } 
        else if(highlightState == HiglightType.None && displayAllTargets){
            meshRenderer.materials = altMaterials;
            highlightState = HiglightType.All;
        }
        else if(highlightState == HiglightType.All && !displayAllTargets){
            meshRenderer.materials = baseMaterials;
            highlightState = HiglightType.None;
        }
    }
}

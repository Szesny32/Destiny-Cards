using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float rotSpeed = 8f;
    public Camera camera;
    public Vector3 cameraOffset = new Vector3(0f, 5f, -5f);
    private CardManager heldCard = null;
    public Material outline;

    public float followSpeed = 2f;
    public float unfollowSpeed = 5f;

    void Start()
    {
        MeshRenderer x = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)){
            move += Vector3.forward;
        } 
        if (Input.GetKey(KeyCode.A)){
            move += Vector3.left;
        } 
        if (Input.GetKey(KeyCode.S)){
            move  += Vector3.back;
        } 
        if (Input.GetKey(KeyCode.D)){
            move += Vector3.right;
        } 
        transform.position += move.normalized * walkSpeed * Time.deltaTime;



        Quaternion current = transform.rotation;
        transform.LookAt(transform.position+move.normalized);
        Quaternion target = transform.rotation;
        transform.rotation = Quaternion.Slerp(current, target, Time.deltaTime * rotSpeed);


        camera.transform.position = transform.position + cameraOffset;


        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        float rayDistance = 100f; 
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

  
        if (heldCard == null){ 
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance, LayerMask.GetMask("Cards"))){
                    CardManager card = hit.collider.GetComponent<CardManager>();
                    card.isSelected = Input.GetMouseButtonDown(0)? false :  true;
                    if(card.isSelected == false){
                        heldCard = card;   
                        heldCard.isHolded = true;
                    }   
            }
        } else if(Input.GetMouseButtonUp(0)){
            heldCard.isHolded = false;
            heldCard = null;  
        } else{
            RaycastHit hit;


            bool successHit = Physics.Raycast(ray, out hit, rayDistance, ~LayerMask.GetMask("Cards"));

            if(successHit){
                float deepth = (hit.point.z -1f) -  camera.transform.position.z;
                Vector3 cardNewPos = Camera.main.ScreenToWorldPoint( new Vector3(mousePosition.x, mousePosition.y, deepth));
                heldCard.transform.position = Vector3.Lerp(heldCard.transform.position, cardNewPos, followSpeed * Time.deltaTime);

                MeshRenderer targetRenderer = hit.collider.GetComponent<MeshRenderer>();
                bool hasOutlineMaterial = false;
                foreach (Material mat in targetRenderer.materials)
                {
                    if (mat.name == outline.name + " (Instance)")
                    {
                        hasOutlineMaterial = true;
                        break;
                    }
                }

                if (hasOutlineMaterial == false){
                    Material[] exisitingMaterials = targetRenderer.materials;
                    Material[] updatedMaterials = new Material[exisitingMaterials.Length + 1];
                    for (int i = 0; i < exisitingMaterials.Length; i++){
                        updatedMaterials[i] = exisitingMaterials[i];
                    }
                    updatedMaterials[exisitingMaterials.Length] = outline;
                    targetRenderer.materials = updatedMaterials;
                }

            } else {
                 Vector3 cardNewPos =   camera.transform.position 
                                        + camera.transform.forward * heldCard.deepth
                                        + camera.transform.right * heldCard.offset.x
                                        + camera.transform.up * heldCard.offset.y
                                        + camera.transform.forward * heldCard.offset.z;
                                        
                heldCard.transform.position = Vector3.Slerp(heldCard.transform.position, cardNewPos, unfollowSpeed * Time.deltaTime);
            }
        }
    }
}

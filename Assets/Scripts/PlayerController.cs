using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float rotSpeed = 8f;
    public Camera camera;
    public Vector3 cameraOffset = new Vector3(0f, 5f, -5f);
    void Start()
    {
        MeshRenderer x = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        float rayDistance = 100f; 
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);


        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, LayerMask.GetMask("Cards"))){
            Debug.Log("Trafiono obiekt z warstwą 'card'!");
            CardManager card = hit.collider.GetComponent<CardManager>();
            if (card != null){
                card.isSelected = true;
            }
        }
            
        

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
    }
}

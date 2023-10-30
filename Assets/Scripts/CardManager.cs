using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Camera camera;
    public float deepth = 4f;
    public Vector3 offset= new Vector3(0f, -1.4f, 0f);
    public Vector3 flowOrigin; 
    public float amplitude = 0.05f;

    public Vector3 s = new Vector3( 1.5f, 1.5f, 1.5f);
    public bool isSelected = false;
    public float growDelta = 2f;
    public float decreaseDelta = 0.5f;

    public bool isHolded = false;
    void Start()
    {
        transform.rotation = camera.transform.rotation;
        flowOrigin.x = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        flowOrigin.y = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        flowOrigin.z = Random.Range(0f, 360f) * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHolded == false){
            float d = isSelected? growDelta : - decreaseDelta;
            d *= Time.deltaTime;
            transform.localScale = new Vector3( 
                Mathf.Clamp(transform.localScale.x + d, 1.0f, s.x),
                Mathf.Clamp(transform.localScale.y + d, 1.0f, s.y),
                Mathf.Clamp(transform.localScale.z + d, 1.0f, s.z));


            transform.position = camera.transform.position + camera.transform.forward * deepth;
            transform.position += camera.transform.right * offset.x;
            transform.position += camera.transform.up * offset.y;
            transform.position += camera.transform.forward * offset.z;
            transform.position +=  new Vector3(
                                            amplitude *(Mathf.Sin(Time.time + flowOrigin.x) - 1f), 
                                            amplitude *(Mathf.Sin(Time.time + flowOrigin.y) - 1f),
                                            amplitude *(Mathf.Sin(Time.time + flowOrigin.z) - 1f));
        
        }

        isSelected = false;
        


        Debug.DrawRay( camera.transform.position, transform.position - camera.transform.position, Color.blue);
        //transform.LookAt(camera.transform.position);
    }
}

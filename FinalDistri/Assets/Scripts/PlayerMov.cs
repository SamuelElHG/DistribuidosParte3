using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(Input.GetAxis("Horizontal")*3.0f*Time.deltaTime, Input.GetAxis("Vertical")*3.0f*Time.deltaTime, 0);      
    }
}

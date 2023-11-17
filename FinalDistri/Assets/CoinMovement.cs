using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void move()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(1, 1, 1), 5f);
    }
}

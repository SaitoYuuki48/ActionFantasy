using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = rd.velocity;

        float moveSpeed = 3.0f;

        float stickH = Input.GetAxis("Horizontal");
        float stickV = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.RightArrow) || stickH > 0)
        {
            v.x = moveSpeed;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || stickH < 0)
        {
            v.x = -moveSpeed;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            v.x = 0;
        }

    }
}

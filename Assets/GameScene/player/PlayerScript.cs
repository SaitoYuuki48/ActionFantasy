using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rd;

    //ïœêî
    public Animator animator;
    //public float moveSpeed = 5.0f;
    //public float rotationSpeed = 1200.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            animator.SetBool("Run", false);
        }
        else
        {
            animator.SetBool("Run", true);
        }

        //if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        //{
        //    animator.SetBool("Run", false);
        //}
        //else
        //{
        //    var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //    Vector3 direction = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
        //    animator.SetBool("Run", true);
        //    rd.MovePosition(rd.position + transform.TransformDirection(direction) * moveSpeed * Time.deltaTime);

        //    ChangeDirection(direction);
        //}
    }

    //void ChangeDirection(Vector3 direction)
    //{
    //    Quaternion q = Quaternion.LookRotation(direction);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
    //}
}

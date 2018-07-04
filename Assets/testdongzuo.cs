using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testdongzuo : MonoBehaviour {

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            animator.SetInteger("status", 1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            animator.SetInteger("status", 2);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            animator.SetInteger("status", 3);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            animator.SetInteger("status", 4);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            animator.SetInteger("status", 5);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            animator.SetInteger("status", 6);
        }
    }
}
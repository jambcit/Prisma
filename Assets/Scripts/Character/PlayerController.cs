﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller.
///  - Controls characters by Rigidbody
///  - Inputs: Mouse horizontal moves(rotates the character),
/// 			WASD(or arrows) for forward, sides, backward 
/// Author: Shawn(Dongwon) Kim
/// </summary>
public class PlayerController : MonoBehaviour
{

    public static float SPEED_REGULAR = 12.0f;
    public static float SPEED_WITH_GRAB = 6.0f;

    public float speed;
    public float mouseSensitivity;

    public bool isGrabbing;

    private Rigidbody rb;

    public GameObject collidedObject;

    public float delay;

    public float lastTime;
    public float curTime;
    void Start()
    {
        speed = SPEED_REGULAR;
        mouseSensitivity = 5.0f;
        isGrabbing = false;
        lastTime = 0;
        delay = 0.5f;
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        curTime += Time.deltaTime;
        if (delay < curTime)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!isGrabbing)
                {
                    Grab(collidedObject);
                }
                curTime = 0;
            }

            if (Input.GetKey(KeyCode.C))
            {
                if (isGrabbing)
                {
                    Release(collidedObject);
                }
                curTime = 0;
            }
        }

        AnimateCharacter();
        Move();
        Rotate();
    }


    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // Future, when its grabed apply the speed with grabing
        if (isGrabbing)
        {
            speed = SPEED_WITH_GRAB;
        }
        else
        {
            speed = SPEED_REGULAR;
        }

        // moving
        Vector3 moveDirection = (transform.forward * moveVertical + transform.right * moveHorizontal) * speed;

        rb.velocity = moveDirection;
    }


    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");

        Vector3 direction = new Vector3(0f, mouseSensitivity * mouseX, 0f);

        if (direction != Vector3.zero)
        {
            // rotation while moving
            transform.rotation = Quaternion.Euler(rb.rotation.eulerAngles + direction);
        }

        if (rb.velocity != Vector3.zero)
        {
            // rotation while moving mouse only
            rb.rotation.SetLookRotation(rb.velocity);
        }
    }


    private void AnimateCharacter()
    {
        // when nothing happens, idle
        if (!Input.anyKeyDown)
        {
            GetComponent<Animator>().SetTrigger("Idle");
        }
        // when E input pressed
        if (Input.GetKey(KeyCode.E))
        {
            if (isGrabbing)
            {
                GetComponent<Animator>().SetTrigger("Push");
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Push");
            }
        }

        // When moving key input pressed
        if (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.RightArrow))
        {
            if (isGrabbing)
            {
                if (Input.GetKey(KeyCode.W)
                    || Input.GetKey(KeyCode.UpArrow))
                {
                    GetComponent<Animator>().SetTrigger("Push");
                }
                else if (Input.GetKey(KeyCode.S)
                    || Input.GetKey(KeyCode.DownArrow))
                {
                    GetComponent<Animator>().SetTrigger("Pull");
                }
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Walk");
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        collidedObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        collidedObject = null;
    }

    private void Grab(GameObject anObject)
    {
        if (anObject != null)
        {

            if (anObject.tag == "Box"
                && transform.name == "Character01")
            {
                anObject.transform.parent = transform;
                anObject.transform.localPosition = new Vector3(0.0f, 2.0f, 2.6f);
                GetComponent<BoxCollider>().enabled = false;
                isGrabbing = true;
            }

            if (anObject.tag == "Mirror"
            && transform.name == "Character01")
            {

                Transform anObjectRot = anObject.transform;

                if (anObjectRot.eulerAngles.y == 0)
                {
                    anObjectRot.Rotate(0f, 90f, 0f);

                }
                else
                {
                    anObjectRot.Rotate(0f, -90f, 0f);
                }
                //TODO Mirror: grab and make it Rotate
            }
        }
    }

    //TODO Release function
    private void Release(GameObject anObject)
    {
        if (anObject != null)
        {
            if (anObject.tag == "Box"
            && transform.name == "Character01")
            {
                anObject.transform.parent = null;
                isGrabbing = false;
            }

            if (anObject.tag == "Mirror"
            && transform.name == "Character01")
            {
                //TODO Mirror: release
                anObject.transform.parent = null;
                isGrabbing = false;
            }

            GetComponent<BoxCollider>().enabled = true;
            collidedObject = null;
        }

    }

}
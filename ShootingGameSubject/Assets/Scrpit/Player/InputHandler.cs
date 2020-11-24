﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float vertical;
    public float horizontal;

    public float r_vertical;
    public float r_horizontal;
    public Vector3 lookDir;
    public bool fire;
    public bool dodge;
    public bool x_input;
    public bool r1_input;
    public bool r2_input;
    public float r2_axis;
    public float delta;
    private bool XButtonDown;
    private bool R1ButtonDown;
    private bool R2ButtonDown;
    private float smoothSpeed = 0.15f;
    private Vector3 mouse_pos;
    
    void Update()
    {
        delta = Time.deltaTime;
            
    }
    private void FixedUpdate() 
    {
        GetInput();
        UpdateStates();
    }
    void GetInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float c_h = Input.GetAxis("ControllerHorizontal");
        float c_v = Input.GetAxis("ControllerVertical");

        
        float rc_h = Input.GetAxis("RightAxis X");
        float rc_v = Input.GetAxis("RightAxis Y");

        if(c_h!=0 || c_v!=0)
        {
            h = c_h;
            v = c_v;
            smoothSpeed = 0.15f;
        }
        else
        {
            smoothSpeed = 0.05f;
        }

        if(rc_h!=0 || rc_v!=0)
        {
            
            r_horizontal = rc_h;
            r_vertical = rc_v;
            lookDir = new Vector3(rc_h,rc_v,0);
        }
        else if(mouse_pos!=Input.mousePosition)
        {
            mouse_pos = Input.mousePosition;
            lookDir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        }

        vertical = v;
        horizontal = h;

        R2Input();
        FireInput();
        DodgeInput();
        XInput();
    }
    void UpdateStates()
    {
        R1Input();
        
    }
    void XInput()
    {
        if(Input.GetButton("X") && !XButtonDown)
        {
            XButtonDown = true;
            x_input = true;
        }
        else if(!Input.GetButton("X") && XButtonDown)
        {
            XButtonDown = false;
            x_input = false;
        }
        else
        {
            x_input = false;
        }
    }
    void DodgeInput()
    {
        if(x_input || r1_input)
            dodge = true;
        else
            dodge = false;

    }
    void FireInput()
    {
        if(r2_axis < 0 || Input.GetButton("Fire2"))
            fire = true;
        else
            fire =false;
        
    }
    void R1Input()
    {
        if(Input.GetButton("R1") && !R1ButtonDown)
        {
            R1ButtonDown = true;
            r1_input = true;
        }
        else if(!Input.GetButton("R1") && R1ButtonDown)
        {
            R1ButtonDown = false;
            r1_input = false;
        }
        else
        {
            r1_input = false;
        }
    }
    void R2Input()
    {
        r2_axis = Input.GetAxisRaw("R2");
        if(r2_axis  <0 && !R2ButtonDown)
        {
            r2_input = true;
            R2ButtonDown = true;
        }
        else if(r2_axis  >=0 &&R2ButtonDown)
        {
            r2_input = false;
            R2ButtonDown = false;
        }
        else
        {
            r2_input = false;
        }
    }

}

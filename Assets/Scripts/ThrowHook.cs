﻿using UnityEngine;
using System.Collections;

public class ThrowHook : MonoBehaviour
{
    public GameObject hook;
    public bool rope_active;

    GameObject curHook;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        if (Input.GetMouseButtonDown(0))
        {
            if(!rope_active)
            {
                Vector2 destiny = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);

                curHook.GetComponent<RopeScript>().destiny = destiny;

                rope_active = true;
            }
            else
            {
                Destroy(curHook);

                rope_active = false;

            }
            
        }
    }
}
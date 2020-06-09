using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCaster : MonoBehaviour
{
    public GameObject hook;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        if (Input.GetMouseButtonDown(0))
        {
            hook.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.right * 17, ForceMode2D.Impulse);
        }
    }
}

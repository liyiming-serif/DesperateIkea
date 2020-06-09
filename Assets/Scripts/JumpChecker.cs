using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("touching: " + LayerMask.LayerToName(other.gameObject.layer));
        if (other.gameObject.layer == LayerMask.NameToLayer("terrain"))
        {
            //Debug.Log("can jump");
            TankController.Instance().SetJump(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("terrain"))
        {
            //Debug.Log("can't jump");
            TankController.Instance().SetJump(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("touching: " + LayerMask.LayerToName(other.gameObject.layer));
        //if (other.gameObject.layer == LayerMask.NameToLayer("terrain"))
        //{
        //    //Debug.Log("can jump");
        //    TankController.Instance().SetJump(true);
        //}

    }
}

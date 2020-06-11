using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McGuffinController : MonoBehaviour
{
    HingeJoint2D hookPoint;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttachToHook(Collision2D col)
    {
        // First time attached to hook
        gameObject.layer = LayerMask.NameToLayer("grabbed");
        gameObject.GetComponent<HingeJoint2D>().enabled = true;
        hookPoint = gameObject.GetComponent<HingeJoint2D>();
        hookPoint.connectedBody = col.otherRigidbody;
        //hookPoint.anchor =  transform.InverseTransformPoint(col.GetContact(0).point);
    }

    public void TransferMcGuffin(Rigidbody2D rb)
    {
        // Transfering between rings; pass hingejoint to another rigidbody
        hookPoint.connectedBody = rb;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            Debug.Log("You're winner!");
            col.collider.enabled = false;
        }
    }
}

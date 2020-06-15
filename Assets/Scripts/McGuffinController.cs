using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McGuffinController : MonoBehaviour
{
    HingeJoint2D hookPoint;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("grabbable");

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        rigidBody.freezeRotation = false;

        hookPoint = GetComponent<HingeJoint2D>();
        hookPoint.autoConfigureConnectedAnchor = true;
        hookPoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AttachToHook(Collision2D col)
    {
        // First time attached to hook
        gameObject.layer = LayerMask.NameToLayer("grabbed");
        hookPoint.enabled = true;
        
        hookPoint.connectedBody = col.otherRigidbody;
        hookPoint.autoConfigureConnectedAnchor = false;
        SoundManager.Instance().mcguffinTrigger.Play();
    }

    public void YankedByHook()
    {
        rigidBody.isKinematic = false;
        //rigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void TransferMcGuffin(Rigidbody2D otherRB)
    {
        // Transfering between rings; pass hingejoint to another rigidbody
        hookPoint.connectedBody = otherRB;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            Debug.Log("You're winner!");

            GetComponent<Rigidbody2D>().freezeRotation = true;
            GameManager.Instance().Win(transform);
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}

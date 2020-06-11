using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    //Rope-Ring-Anchor: linked list pointers
    GameObject anchorChild;

    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anchorChild = transform.Find("Anchor").gameObject;
        
        anchorChild.GetComponent<AnchorPoint>().parentRing = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(anchorChild.GetComponent<AnchorPoint>().isAnchorActive)
        {
            rotateTowardsMouse();
        }
    }

    public void DisableCollision(GameObject lastHook)
    {
        gameObject.layer = LayerMask.NameToLayer("takenAnchor");
        anchorChild.GetComponent<AnchorPoint>().ActivateAnchor(lastHook);
    }

    public void EnableCollision()
    {
        gameObject.layer = LayerMask.NameToLayer("activeAnchor");
    }

    void rotateTowardsMouse()
    {
        anchorChild.transform.right = getMouseAngle();
    }

    Vector2 getMouseAngle()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Vector2 pointDirection = new Vector2(
            mousePosition.x-anchorChild.transform.position.x,
            mousePosition.y-anchorChild.transform.position.y);
        pointDirection /= pointDirection.magnitude;

        return pointDirection;
    }
}

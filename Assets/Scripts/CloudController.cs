using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controls a cloud drifting in the background. Will screen wrap when outside the camera.
 * Drifts only horizontally, -moveSpeed = left; +moveSpeed = right.
 * Requires: SpriteRenderer
 */

public class CloudController : MonoBehaviour
{
    public float moveSpeedPerSec = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right*moveSpeedPerSec*Time.deltaTime, Space.World);
        WrapIfOutsideView();
    }

    void WrapIfOutsideView()
    {
        Bounds bounds = gameObject.GetComponent<SpriteRenderer>().sprite.bounds;
        float cameraX = Camera.main.transform.position.x;
        float cameraExtentX = Camera.main.orthographicSize*Screen.width/Screen.height;

        float spriteLeftBound = transform.position.x - bounds.extents.x;
        float spriteRightBound = transform.position.x + bounds.extents.x;
        float cameraLeftBound = cameraX-cameraExtentX;
        float cameraRightBound = cameraX+cameraExtentX;

        if (spriteLeftBound > cameraRightBound)
        {
            // out of view on right side:
            // wrap to the left
            Vector3 newPos = new Vector3(cameraLeftBound-bounds.extents.x, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
        else if (spriteRightBound < cameraLeftBound)
        {
            // out of view on left side:
            // wrap to the right
            Vector3 newPos = new Vector3(cameraRightBound+bounds.extents.x, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Provides hitbox for launched hooks to attach to
 * Requires: HookLauncher prefab as child
 */

public class RingController : MonoBehaviour
{
    //Rope-Ring-Launcher: linked list pointers
    GameObject hookLauncherChild;

    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        hookLauncherChild = transform.Find("HookLauncher").gameObject;
        hookLauncherChild.GetComponent<HookLauncherController>().parentRing = gameObject;

        hookLauncherChild.GetComponent<HookLauncherController>().DeactivateHookLauncher();
    }

    // Update is called once per frame
    void Update()
    {
        if(hookLauncherChild.GetComponent<HookLauncherController>().IsHookLauncherActive())
        {
            rotateTowardsMouse();
        }
    }

    public void DisableCollision(GameObject lastHook)
    {
        gameObject.layer = LayerMask.NameToLayer("takenRing");
        hookLauncherChild.GetComponent<HookLauncherController>().ActivateHookLauncher(lastHook);
    }

    public void EnableCollision()
    {
        gameObject.layer = LayerMask.NameToLayer("activeRing");
    }

    void rotateTowardsMouse()
    {
        hookLauncherChild.transform.right = getMouseAngle();
    }

    Vector2 getMouseAngle()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Vector2 pointDirection = new Vector2(
            mousePosition.x-hookLauncherChild.transform.position.x,
            mousePosition.y-hookLauncherChild.transform.position.y);
        pointDirection /= pointDirection.magnitude;

        return pointDirection;
    }
}

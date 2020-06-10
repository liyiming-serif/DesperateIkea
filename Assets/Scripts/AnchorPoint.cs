using UnityEngine;
using System.Collections;

/*
 * Defines behaviour for any object a hook can be attached to and fired from.
 * Hook's transform and fire angle is controlled by this obj.
 * Requires: Parent with a collider
 */

public class AnchorPoint : MonoBehaviour
{
    public GameObject hookPrefab;
    public bool rope_active;

    GameObject curHook;
    public GameObject prevAnchor;
    GameObject parentHitbox;

    //EMPTY: anchor has no rope through it; hooks can be attached
    //PASSED: hook has already passed through this point
    //CURRENT: anchor that the player is aiming and firing with
    public enum AnchorState { EMPTY, PASSED, CURRENT }
    public AnchorState currAnchorState = AnchorState.EMPTY;

    AudioSource src;

    public ParticleSystem poof;
    public ParticleSystem pew;

    // Use this for initialization
    void Start()
    {
        src = GetComponent<AudioSource>();

        parentHitbox = gameObject.transform.parent.gameObject;
        if (currAnchorState==AnchorState.CURRENT)
        {
            parentHitbox.layer = LayerMask.NameToLayer("takenAnchor");
        }
        else
        {
            parentHitbox.layer = LayerMask.NameToLayer("activeAnchor");
        }
    }

    // Update is called once per frame
    void Update()
    {   
        //Left click to fire hook
        if (Input.GetMouseButtonDown(0)) //&& currAnchorState==AnchorState.CURRENT)
        {
            if(curHook == null)
            {
                curHook = (GameObject)Instantiate(hookPrefab, transform.position, Quaternion.identity);

                src.Play();
                curHook.GetComponent<Rigidbody2D>().AddForce(transform.right * TankController.Instance().shootForce, ForceMode2D.Impulse);
                curHook.transform.eulerAngles = transform.eulerAngles; //TankController.Instance().gun.transform.eulerAngles;

                poof.gameObject.SetActive(true);
                poof.Play();

                pew.gameObject.SetActive(true);
                pew.Play();

                curHook.GetComponent<HookController>().anchor = gameObject;
            }
        }

        //Right click to retract hook
        if (Input.GetMouseButtonDown(1))// && currAnchorState==AnchorState.CURRENT)
        {
            if(curHook != null)
            {   
                HookController hookController = curHook.GetComponent<HookController>();
                if (!hookController.retractingHook)
                {
                    hookController.StartCoroutine(hookController.RetractHook());
                }
            }
        }
    }


}
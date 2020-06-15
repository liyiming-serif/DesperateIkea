using UnityEngine;
using System.Collections;

/*
 * Defines behaviour for any object a hook can be fired from.
 * Hook's transform and fire angle is controlled by this obj.
 * Requires: AudioSource
 */

public class HookLauncherController : MonoBehaviour
{
    public GameObject hookPrefab;

    //Hook-Ring-Laucher: linked list pointers
    GameObject curHook;
    GameObject prevHook;
    public GameObject parentRing;
    public GameObject grabbedItem;

    public bool isHookLauncherActive;

    AudioSource src;
    GameObject loadedHook;

    public ParticleSystem poof;
    public ParticleSystem pew;

    // Use this for initialization
    void Start()
    {
        src = GetComponent<AudioSource>();

        loadedHook = transform.Find("LoadedHook").gameObject;

        poof.gameObject.SetActive(false);
        poof.Play();

        pew.gameObject.SetActive(false);
        pew.Play();
    }

    // Update is called once per frame
    void Update()
    {   
        if (!isHookLauncherActive)
        {
            loadedHook.SetActive(false);
            return;
        }

        //Left click to fire hook
        if (Input.GetMouseButtonDown(0))
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

                curHook.GetComponent<HookController>().startingHookLauncher = gameObject;
            }
        }

        //Right click to retract hook
        if (Input.GetMouseButtonDown(1))
        {
            if(curHook != null)
            {   
                //retract hook to current launcher
                HookController hookController = curHook.GetComponent<HookController>();
                if (!hookController.retractingHook)
                {
                    hookController.StartCoroutine(hookController.RetractHook());
                }
            }
            else if (prevHook != null)
            {
                //retract hook back to last launcher
                HookController hookController = prevHook.GetComponent<HookController>();
                DeactivateHookLauncher();

                //pass along grabbed item
                if(grabbedItem != null)
                {
                    prevHook.GetComponent<HookController>().grabbedItem = grabbedItem;
                    grabbedItem.GetComponent<McGuffinController>().TransferMcGuffin(prevHook.GetComponent<Rigidbody2D>());
                }
                hookController.StartCoroutine(hookController.RetractHook());
            }
        }

        if(curHook==null)
        {
            loadedHook.SetActive(true);
        }
        else
        {
            loadedHook.SetActive(false);
        }
    }

    public void ActivateHookLauncher(GameObject lastHook)
    {
        isHookLauncherActive = true;
        prevHook = lastHook;

        SoundManager.Instance().ringTrigger.Play();
    }

    public void ReactivateHookLauncher()
    {
        isHookLauncherActive = true;
    }

    public void PassThroughHookLauncher()
    {
        isHookLauncherActive = false;
    }

    void DeactivateHookLauncher()
    {
        isHookLauncherActive = false;
        if(parentRing != null)
        {
            parentRing.GetComponent<RingController>().EnableCollision();
        }
        
    }
}
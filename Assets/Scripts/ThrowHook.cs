using UnityEngine;
using System.Collections;

public class ThrowHook : MonoBehaviour
{
    public GameObject hookPrefab;
    public bool rope_active;

    GameObject curHook;

    AudioSource src;

    public ParticleSystem poof;
    public ParticleSystem pew;

    // Use this for initialization
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {       
        if (Input.GetMouseButtonDown(0))
        {
            if(!rope_active)
            {
                Vector2 destiny = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                curHook = (GameObject)Instantiate(hookPrefab, transform.position, Quaternion.identity);

                src.Play();
                curHook.GetComponent<Rigidbody2D>().AddForce(transform.right * TankController.Instance().shootForce, ForceMode2D.Impulse);
                curHook.transform.eulerAngles = TankController.Instance().gun.transform.eulerAngles;

                poof.gameObject.SetActive(true);
                poof.Play();

                pew.gameObject.SetActive(true);
                pew.Play();

                curHook.GetComponent<HookController>().anchor = gameObject;
                rope_active = true;
            }
            else
            {
                Destroy(curHook);

                rope_active = false;
            }
        }

        if(rope_active)
        {

        }
    }


}
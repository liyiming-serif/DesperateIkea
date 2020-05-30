using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        Ball obj = col.GetComponent<Ball>();
        if (obj != null)
        {
            Debug.Log("got ball!");
            TankController.Instance().SetBall(obj);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Ball obj = col.GetComponent<Ball>();
        if (obj != null)
        {
            Debug.Log("unsetting ball!");
            TankController.Instance().UnsetBall(obj);
        }
    }
}

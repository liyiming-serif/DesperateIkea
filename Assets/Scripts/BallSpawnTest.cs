using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnTest : MonoBehaviour
{
    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            GameObject newBall = Instantiate(ball);
            Vector3 pos = this.transform.position;
            pos.x += Random.Range(-1.5f, 1.5f);
            newBall.transform.position = pos;

        }
    }
}

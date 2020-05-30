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
            pos.x = Random.Range(1.3f, 8.0f);
            pos.y = Random.Range(-3.3f, -2.3f);
            newBall.transform.position = pos;

            Vector2 angle = new Vector2(
                Random.Range(-1.0f, 0.0f),
                Random.Range(0.0f, 1.0f)
                );

            newBall.GetComponent<Rigidbody2D>().AddForce(angle.normalized * Random.Range(5.5f, 12.5f), ForceMode2D.Impulse);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController m_instance;

    public static PlayerController Instance()
    {
        if (m_instance == null)
        {
            m_instance = new PlayerController();
        }
        return m_instance;
    }

    public GameObject body;
    private Rigidbody2D bodyRb;
    public GameObject gun;
    private Rigidbody2D gunRb;
    public Transform gunPivot;
    public Transform gunHitBox;
    public Vector2 gunAngleLimits;

    public float aimRate = 3;
    public float moveRate = 5;

    public Rigidbody2D ball;

    void Awake()
    {
        if (!m_instance)
            m_instance = this;

        bodyRb = body.GetComponent<Rigidbody2D>();
        //gunRb = gun.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // move the tank
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("left");
            bodyRb.AddForce(Vector2.left * moveRate);
        } else if(Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("right");
            bodyRb.AddForce(Vector2.right * moveRate);
        }

        // rotate the gun
        if(Input.GetKey(KeyCode.UpArrow))
        {
            clampRotation(aimRate);
            //gunRb.rotation += (aimRate);
        } else if(Input.GetKey(KeyCode.DownArrow))
        {
            clampRotation(-aimRate);
            //gunRb.rotation += -(aimRate);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Instantiate(ball);
            ball.transform.position = gunHitBox.transform.position;
            ball.transform.rotation = gunHitBox.transform.rotation;
            ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(10,10),ForceMode2D.Impulse);
        }
    }
    
    void clampRotation(float angle)
    {
        float rot = gunPivot.transform.localRotation.eulerAngles.z;
        rot += angle;
        Debug.Log("preclamp: " + rot);
        rot = Mathf.Clamp(rot, gunAngleLimits.x, gunAngleLimits.y);
        Debug.Log("postclamp: " + rot);
        gunPivot.eulerAngles = new Vector3(0, 0, rot);

        //gunPivot.transform.Rotate(0, 0, aimRate);
    }

}

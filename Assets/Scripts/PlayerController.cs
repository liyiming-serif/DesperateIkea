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

    private Ball ball;

    public enum TankState { EMPTY, LOADED, FIRING }
    public TankState currTankState = TankState.EMPTY;

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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Debug.Log("left");
            bodyRb.AddForce(Vector2.left * moveRate);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //Debug.Log("right");
            bodyRb.AddForce(Vector2.right * moveRate);
        }

        // rotate the gun
        if (Input.GetKey(KeyCode.UpArrow))
        {
            clampRotation(aimRate);
            //gunRb.rotation += (aimRate);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            clampRotation(-aimRate);
            //gunRb.rotation += -(aimRate);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currTankState == TankState.LOADED)
            {
                currTankState = TankState.FIRING;
                ball.transform.position = gunHitBox.transform.position;
                Debug.Log(gunPivot.right);
                ball.gameObject.SetActive(true);
                ball.GetComponent<Rigidbody2D>().AddForce(gunPivot.right * 10, ForceMode2D.Impulse);

                //currTankState = TankState.FIRING;
                //ball = null;
            }
        }
    }

    void clampRotation(float angle)
    {
        float rot = gunPivot.transform.localRotation.eulerAngles.z;
        //rot += angle;
        
        rot += angle;
        rot = Mathf.Clamp(rot + Mathf.Abs(gunAngleLimits.x), 0 + Mathf.Abs(gunAngleLimits.x), gunAngleLimits.y + Mathf.Abs(gunAngleLimits.x));

        rot = (rot - Mathf.Abs(gunAngleLimits.x)) % 360;
        
        gunPivot.transform.localEulerAngles = new Vector3(0, 0, rot);

        //gunPivot.transform.Rotate(0, 0, aimRate);
    }

    public void setBall(Ball theBall)
    {
        if(currTankState == TankState.EMPTY)
        {
            ball = theBall;
            ball.gameObject.SetActive(false);

            currTankState = TankState.LOADED;
        }
    }

    public void unsetBall(Ball theBall)
    {
        if (currTankState != TankState.EMPTY)
        {
            if(theBall == ball)
            {
                ball = null;
                currTankState = TankState.EMPTY;
            }
        }
    }
}

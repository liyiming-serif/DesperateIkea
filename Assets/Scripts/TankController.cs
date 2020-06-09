using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankController : MonoBehaviour
{
    private static TankController m_instance;

    public static TankController Instance()
    {
        if (m_instance == null)
        {
            m_instance = new TankController();
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
    public float jumpForce = 10;

    public float shootForce = 10;
    
    private Ball ball;

    public enum TankState { EMPTY, LOADED, FIRING }
    public TankState currTankState = TankState.EMPTY;

    bool canJump = false;

    private bool enableControls = false;

    public AudioSource turretAimSound;

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
        if(enableControls)
        {
            // move the tank
            if (Input.GetKey(KeyCode.A))
            {
                bodyRb.AddForce(Vector2.left * moveRate);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                bodyRb.AddForce(Vector2.right * moveRate);
            }

            // rotate the gun
            clampRotation(getMouseAngle());

            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                Debug.Log("trying to jump, bruh");
                bodyRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    float getMouseAngle()
    {
        Vector2 temp = Input.mousePosition;
        temp -= (Vector2) gunPivot.transform.position;
        float mouseAngle = Mathf.Atan(temp.y / temp.x);
        #region atan BS
        if (temp.x > 0)
        {

        }
        else if (temp.x < 0)
        {
            if (temp.y >= 0)
            {
                mouseAngle += Mathf.PI;
            }
            else
            {
                mouseAngle += -Mathf.PI;
            }
        }
        else if (Mathf.Abs(temp.x) <= 0.001 && temp.y > 0)
        {
            mouseAngle = Mathf.PI / 2f;
        }
        else if (Mathf.Abs(temp.x) <= 0.001 && temp.y < 0)
        {
            mouseAngle = -Mathf.PI / 2f;
        }
        #endregion

        if (float.IsNaN(mouseAngle))
            mouseAngle = 0;

       
        return mouseAngle * Mathf.Rad2Deg;
    }

    void clampRotation(float angle)
    {
        float rot = gunPivot.transform.localRotation.eulerAngles.z;
        //rot += angle;
        
        rot = Mathf.Clamp(angle, 0, 111);

        rot = (rot - Mathf.Abs(gunAngleLimits.x)) % 360;

        float angleDiff = Mathf.Abs(gunPivot.transform.localEulerAngles.z - rot);
        Debug.Log("angle: " + angleDiff);
        angleDiff = Mathf.Clamp(angleDiff, 0, 4);
        turretAimSound.volume = Utils.Map(angleDiff, 0, 4, 0, 0.3f);

        Debug.Log(rot + " vs " + gunAngleLimits.x);
        turretAimSound.pitch = Mathf.Lerp(turretAimSound.pitch, Utils.Map(rot, 0, gunAngleLimits.y, 0.3f, 2), 0.3f);

        gunPivot.transform.localEulerAngles = new Vector3(0, 0, rot);
    }

    public void SetBall(Ball theBall)
    {
        if(currTankState == TankState.EMPTY)
        {
            ball = theBall;
            ball.gameObject.SetActive(false);

            currTankState = TankState.LOADED;
        }
    }

    public void UnsetBall(Ball theBall)
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


    public void SetJump(bool b)
    {
        canJump = b;
    }

    public void EnableControls(bool b)
    {
        enableControls = b;
    }
}

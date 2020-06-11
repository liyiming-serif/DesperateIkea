using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HookController : MonoBehaviour
{
    //Rope-Ring-Anchor: linked list pointers
    public GameObject startingAnchor;
    public GameObject endRing;

    public int startingSegments;
    public float maxDistance = 2f;
    float tightenedMaxDistance;
    public float retractSpeedSec = .25f;
    public float retractFromRingSpeedSec = 0.02f;
    public float yankDuration = 0.1f;
    public float yankForce = 100f;
    public float quickRetractDistance = 0.2f;

    public GameObject ropePrefab;
    public GameObject lastSegment;

    public LineRenderer lr;
    int num_vertices = 1;
    public List<GameObject> ropeSegments = new List<GameObject>();

    public bool retractingHook = false;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lastSegment = transform.gameObject;
        //lastSegment.GetComponent<DistanceJoint2D>().connectedBody = anchor.GetComponent<Rigidbody2D>();
        ropeSegments.Add(lastSegment);
        num_vertices++;

        //AppendNode();
        for (int i = 0; i < startingSegments; i++)
        {
            AppendNode(i);
        }
        lastSegment.GetComponent<DistanceJoint2D>().connectedBody = startingAnchor.GetComponent<Rigidbody2D>();
        //StartCoroutine(TrackAngle());
    }

    // Update is called once per frame
    void Update()
    {
        DrawLine();
    }

    void DrawLine()
    {
        for(int i = 0; i < num_vertices - 1; i++)
        {
            lr.SetPosition(i, ropeSegments[i].transform.position);
        }

        lr.SetPosition(num_vertices - 1, startingAnchor.transform.position); ;
    }

    void AppendNode(int idx)
    {
        Vector2 pos2Create = startingAnchor.transform.position - lastSegment.transform.position;
        pos2Create.Normalize();
        pos2Create *= maxDistance;
        pos2Create += (Vector2)lastSegment.transform.position;

        //create new rope segment from the anchor point
        GameObject go = (GameObject)Instantiate(ropePrefab, pos2Create, Quaternion.identity);

        go.transform.SetParent(transform);

        lastSegment.GetComponent<DistanceJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();
        lastSegment.GetComponent<DistanceJoint2D>().distance = maxDistance;

        lastSegment = go;

        num_vertices++;
        go.name = "Node " + (idx + 1);
        lr.positionCount = (num_vertices);
        ropeSegments.Add(lastSegment);
    }

    IEnumerator TrackAngle()
    {
        Vector2 v;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        while (true)
        {
            v = body.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, angle), 0.3f);
            transform.eulerAngles = new Vector3(0, 0, angle);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator RetractHook()
    {
        retractingHook = true;
        float elapsedTime = 0f;
        float curRetractSpeed = retractSpeedSec;

        //Yanking from an attached ring. Give it some impulse so it doesn't feel limp
        HingeJoint2D endJoint = gameObject.GetComponent<HingeJoint2D>();
        if(endJoint.enabled)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<HingeJoint2D>().enabled = false;
            Vector2 dir = startingAnchor.transform.position - transform.position;
            gameObject.GetComponent<Rigidbody2D>().AddForce(dir/dir.magnitude*yankForce, ForceMode2D.Impulse);
            endRing.layer = LayerMask.NameToLayer("activeAnchor");
            elapsedTime = yankDuration; 
            curRetractSpeed = retractFromRingSpeedSec;

            //reactivate the previous anchor
            startingAnchor.GetComponent<AnchorPoint>().ReactivateAnchor();
        }

        while (num_vertices > 1)
        {
            //update 2nd to last segment
            GameObject secondToLastSegment = ropeSegments[num_vertices-2];
            secondToLastSegment.GetComponent<DistanceJoint2D>().connectedBody = startingAnchor.GetComponent<Rigidbody2D>();

            //remove rope segment starting from the anchor point
            ropeSegments.Remove(lastSegment);
            num_vertices--;
            lr.positionCount = (num_vertices);
            Destroy(lastSegment);

            lastSegment = secondToLastSegment;
            if(num_vertices == 1)
            {
                gameObject.GetComponent<DistanceJoint2D>().distance = 0;
            }

            //under the influence of the yank force
            float dist = Vector2.Distance(startingAnchor.transform.position, transform.position);
            if (elapsedTime > 0)
            {
                curRetractSpeed = retractFromRingSpeedSec;
                elapsedTime -= curRetractSpeed;
            }
            else if (dist <= quickRetractDistance)
            {
                curRetractSpeed = retractFromRingSpeedSec;
            }
            else
            {
                curRetractSpeed = retractSpeedSec;
            }
            yield return new WaitForSeconds(curRetractSpeed);
        }
        retractingHook = false;
        Destroy(gameObject);
    }

    public void TightenRope()
    {
        startingAnchor.GetComponent<AnchorPoint>().PassThroughAnchor();

        //attach to end ring
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        HingeJoint2D endJoint = gameObject.GetComponent<HingeJoint2D>();
        endJoint.enabled = true;
        endJoint.connectedBody = endRing.GetComponent<Rigidbody2D>();

        //tighten max distance on each rope's distance joint
        //float dist = Vector2.Distance(endRing.transform.position, anchor.transform.position);
        tightenedMaxDistance = 0; //dist/(num_vertices);

        for(int i = 0; i < num_vertices-1; i++)
        {
            ropeSegments[i].GetComponent<DistanceJoint2D>().distance = tightenedMaxDistance;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (
            col.gameObject.layer != LayerMask.NameToLayer("player") &&
            col.gameObject.layer != LayerMask.NameToLayer("rope") &&
            col.gameObject.layer != LayerMask.NameToLayer("gun")
            )
        {
            //GameObject sparks = Instantiate(PFXController.Instance().sparks).gameObject;
            //sparks.transform.position = col.contacts[0].point;
        }


        if (col.gameObject.layer == LayerMask.NameToLayer("activeAnchor") && !retractingHook)
        {
            endRing = col.gameObject;
            endRing.GetComponent<RingController>().DisableCollision(gameObject);
            TightenRope();
        }
    }

}
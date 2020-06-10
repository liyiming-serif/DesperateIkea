using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HookController : MonoBehaviour
{
    public GameObject anchor;
    public int startingSegments;
    public float ropeMaxLength = 10f;
    public float maxDistance = 2f;
    public float minVelocity = 0.3f;

    public GameObject ropePrefab;
    public GameObject lastSegment;

    public LineRenderer lr;
    int num_vertices = 1;
    public List<GameObject> ropeSegments = new List<GameObject>();

    bool hasFired = false;
    bool done = false;
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
        lastSegment.GetComponent<DistanceJoint2D>().connectedBody = anchor.GetComponent<Rigidbody2D>();
        //StartCoroutine(TrackAngle());
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity && hasFired)
        {
            done = true;
        }

        if (Vector2.Distance(anchor.transform.position, lastSegment.transform.position) > maxDistance && !done)
        {
            //AppendNode();
        }
        */
        DrawLine();
    }

    void DrawLine()
    {
        for(int i = 0; i < num_vertices - 1; i++)
        {
            lr.SetPosition(i, ropeSegments[i].transform.position);
        }

        lr.SetPosition(num_vertices - 1, anchor.transform.position); ;
    }

    void AppendNode(int idx)
    {
        Vector2 pos2Create = anchor.transform.position - lastSegment.transform.position;
        pos2Create.Normalize();
        pos2Create *= maxDistance;
        pos2Create += (Vector2)lastSegment.transform.position;

        GameObject go = (GameObject)Instantiate(ropePrefab, pos2Create, Quaternion.identity);

        go.transform.SetParent(transform);

        lastSegment.GetComponent<DistanceJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();
        lastSegment.GetComponent<DistanceJoint2D>().distance = maxDistance;

        lastSegment = go;

        num_vertices++;
        go.name = "Node " + (idx + 1);
        lr.positionCount = (num_vertices);
        ropeSegments.Add(lastSegment);

        //Debug.Log(go.name + " is connected to " + lastSegment.GetComponent<DistanceJoint2D>().connectedBody);
        //Debug.Log("hey: " + idx + ", " + (idx + 1));
        if (num_vertices == startingSegments + 1)
        {
            //Debug.Log("hey");
            //DistanceJoint2D finalJoint = anchor.GetComponent<DistanceJoint2D>();
            //finalJoint.enabled = true;
            //finalJoint.connectedBody = lastSegment.GetComponent<Rigidbody2D>();
            //finalJoint.distance = maxDistance;

        }
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (
            col.gameObject.layer != LayerMask.NameToLayer("player") &&
            col.gameObject.layer != LayerMask.NameToLayer("rope") &&
            col.gameObject.layer != LayerMask.NameToLayer("gun")
            )
        {
            GameObject sparks = Instantiate(PFXController.Instance().sparks).gameObject;
            sparks.transform.position = col.contacts[0].point;
        }
    }

}
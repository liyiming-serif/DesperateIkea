using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HookController : MonoBehaviour
{
    public GameObject anchor;
    public int startingSegments;
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
        lastSegment.GetComponent<DistanceJoint2D>().connectedBody = anchor.GetComponent<Rigidbody2D>();
        ropeSegments.Add(lastSegment);
        num_vertices++;

        for(int i = 0; i < startingSegments; i++){
            AppendNode();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity && hasFired)
        {
            done = true;
        }

        if (Vector2.Distance(anchor.transform.position, lastSegment.transform.position) > maxDistance && !done)
        {
            AppendNode();
        }

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

    void AppendNode()
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
        go.name = "Node " + num_vertices;
        lr.positionCount = (num_vertices);
        ropeSegments.Add(lastSegment);
    }
}
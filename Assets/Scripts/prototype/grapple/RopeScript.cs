using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RopeScript : MonoBehaviour
{
    public Vector2 destiny;

    public float speed = 1;
    public float min_distance = 0.04f;
    public float distance = 2;

    public GameObject nodePrefab;
    public GameObject player;
    public GameObject lastNode;

    public LineRenderer lr;
    int num_vertices = 1;
    public List<GameObject> Nodes = new List<GameObject>();

    bool done = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        lr = GetComponent<LineRenderer>();
        lastNode = transform.gameObject;
        Nodes.Add(lastNode);
        num_vertices++;        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destiny, speed);

        if ((Vector2)transform.position != destiny)
        {
            if (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }
        else if (done == false)
        {
            done = true;
            while (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }
            //lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
            lastNode.GetComponent<DistanceJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }

        DrawLine();
    }

    void DrawLine()
    {
        for(int i = 0; i < num_vertices - 1; i++)
        {
            lr.SetPosition(i, Nodes[i].transform.position);
            Nodes[i].GetComponent<DistanceJoint2D>().distance = distance;
        }

        lr.SetPosition(num_vertices - 1, player.transform.position); ;
    }

    void CreateNode()
    {
        Vector2 pos2Create = player.transform.position - lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= distance;
        pos2Create += (Vector2)lastNode.transform.position;

        GameObject go = (GameObject)Instantiate(nodePrefab, pos2Create, Quaternion.identity);

        go.transform.SetParent(transform);

        //lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();
        lastNode.GetComponent<DistanceJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();

        lastNode = go;

        num_vertices++;
        go.name = "Node " + num_vertices;
        lr.positionCount = (num_vertices);
        Nodes.Add(lastNode);
    }
}
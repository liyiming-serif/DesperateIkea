using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float startSize = 6.3f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().orthographicSize = startSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

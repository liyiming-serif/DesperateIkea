using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public float startRot, endRot;
    public float startSize, endSize;

    public float animTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Animate()
    {
        float startTime = Time.fixedTime;
        float lerper = 0;


        while(lerper < 1)
        {
            lerper = (Time.fixedTime - startTime) / animTime;

            transform.localScale = Vector2.one * Mathf.Lerp(startSize, endSize, lerper);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(startRot, endRot, lerper));

            yield return new WaitForFixedUpdate();
        }
    }
}

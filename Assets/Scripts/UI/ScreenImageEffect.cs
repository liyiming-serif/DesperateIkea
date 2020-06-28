using System.Collections;
using UnityEngine;

public class ScreenImageEffect : MonoBehaviour
{
    [SerializeField] Material material;

    public float animTime = 1.2f;
    public bool fading;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(Animate());
        //}
    }

    public void FadeIn()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        fading = true;
        float startTime = Time.timeSinceLevelLoad;
        material.SetFloat("_Type", Random.Range(0, 9));

        float roll = Random.Range(0, 11);
        Debug.Log("rolled: " + roll);
        if (roll > 5)
        {
            Debug.Log("stuff");
            material.SetFloat("_DispOn", 1);
            material.SetFloat("_DispType", Random.Range(0, 10));
        } else
        {
            Debug.Log("no stuff");
            material.SetFloat("_DispOn", 0);
        }

        int funcType = Random.Range(0, 3);
        if (funcType == 0)
        {
            animTime = 1.8f;
        }
        else if (funcType == 1)
        {
            animTime = 1.2f;
        }
        else
        {
            animTime = 3;
        }

        float lerper = 0;
        float lerpFunc = 0;
        while (lerper <= 1)
        {
            lerper =(Time.timeSinceLevelLoad - startTime) / animTime;
            if(funcType == 0)
            {
                lerpFunc = Mathf.SmoothStep(0, 1, lerper);
            } else if(funcType == 1)
            {
                lerpFunc = lerper;
            } else
            {
                lerpFunc = Mathf.Lerp(1 - Mathf.Abs(Mathf.Cos(2 * Mathf.PI * 3 * lerper *  lerper)), 1, Mathf.SmoothStep(0, 1, lerper));
            }
            material.SetFloat("_AnimKnob", lerpFunc);
   
            yield return new WaitForSecondsRealtime(0.015f);
        }

        material.SetFloat("_AnimKnob", 1.0f);

        fading = false;
        Time.timeScale = 1f;

        TankController.Instance().EnableControls(true);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            Graphics.Blit(source, destination, material);
        }
    }
}

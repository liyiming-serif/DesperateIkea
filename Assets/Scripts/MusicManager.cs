using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager m_instance;

    public static MusicManager Instance()
    {
        if (m_instance == null)
        {
            m_instance = new MusicManager();
        }
        return m_instance;
    }

    void Awake()
    {
        if (!m_instance)
            m_instance = this;

    }
    public AudioClip[] musics;
    public float[] probabilities;
    float[] cdf;
    float totProb = 0;

    public AudioSource A;
    public AudioSource B;
    AudioSource currSource, nextSource;

    public float maxVolume = 0.38f;
    float currVolume;

    // Start is called before the first frame update
    void Start()
    {
        totProb = 0;

        cdf = new float[probabilities.Length];

        for (int i = 0; i < probabilities.Length; i++)
        {
            totProb += probabilities[i];
            cdf[i] = totProb;
        }
        //cdf[probabilities.Length] = 1;
        StartCoroutine(LoopRandomly());
    }

    // Update is called once per frame
    void Update()
    {
          currVolume = A.isPlaying ? A.volume : B.volume;
    }

    int RollRandom()
    {
        float roll = Random.Range(0f, 1f) * totProb;
        int idx = 0;
        while (roll >= cdf[idx])
        {
            idx++;
        }
        idx = Mathf.Clamp(idx, 0, cdf.Length);

        //Debug.Log(roll + " gets me " + idx);

        return idx;
    }

    IEnumerator LoopRandomly()
    {
        A.volume = B.volume = maxVolume;
        int currClip;

        currClip = RollRandom();
        A.clip = musics[currClip];

        while (true)
        {
            A.Play();

            currClip = RollRandom();
            B.clip = musics[currClip];

            yield return new WaitForSecondsRealtime(A.clip.length);
            B.Play();

            currClip = RollRandom();
            A.clip = musics[currClip];

            yield return new WaitForSecondsRealtime(B.clip.length);
        }
    }

    public void FadeTo(float v, float fadeTime)
    {
        StartCoroutine(FadeMusic(currVolume, v, fadeTime));
    }

    IEnumerator FadeMusic(float startVol, float endVol, float fadeTime)
    {
        A.volume = B.volume = startVol;

        float startTime = Time.fixedTime;
        float lerper = 0;
        while(Time.fixedTime - startTime < fadeTime)
        {
            lerper = (Time.fixedTime - startTime) / fadeTime;

            A.volume = B.volume = Mathf.Lerp(startVol, endVol, lerper);
            yield return new WaitForFixedUpdate();
        }
    }
}

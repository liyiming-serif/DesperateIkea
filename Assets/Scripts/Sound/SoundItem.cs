using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundItem : MonoBehaviour
{

    public bool playOnAwake = false;
    public bool playing = false;
    public bool loop = false;

    public Vector2 FadeInNOut = new Vector2(0.01f, 0.01f);

    public AudioSource A;
    public AudioSource B;

    public List<AudioClip> clips = new List<AudioClip>();

    public bool rateLimited = false;
    public float rateLimit = 0.1f;
    float lastPlayed = -999f;

    public float volume = 0.75f;
    public float pitch = 1f;

    public Vector2 DelayBetween = new Vector2(0.03f, 0.4f);

    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        SoundManager.Instance().AddSoundItem(this);

        if (playOnAwake)
            PlayAudio();
    }

    // Update is called once per frame
    void Update()
    {
        //if(A.time >= A.clip.length)
        //      {
        //          playing = false;
        //      }
    }

    public void StopAudio()
    {
        //Debug.Log("stopping");
        playing = false;
        
        A.Stop();
        B.Stop();
    }

    public void PlayAudio()
    {
        if (!playing)
        {
            if (rateLimited && Time.time - lastPlayed >= rateLimit || !rateLimited)
            {
                playing = true;

                A.volume = volume;
                
                // one-shot, choose randomly from all clips
                if (!loop)
                {
                    A.clip = clips[Random.Range(0, clips.Count - 1)];
                    SetPitch(pitch);
                    A.Play();
                    
                    lastPlayed = Time.time;
                    StartCoroutine(RateWait(rateLimit));
                }
                else
                {
                    //Debug.Log("gonna loop");
                    StartCoroutine(Looper());
                }
            }
        }
    }

    IEnumerator RateWait(float wait)
    {
        yield return new WaitForSeconds(wait);
        playing = false;
    }

    IEnumerator Looper()
    {
        float startTime = Time.time;
        float lerper = 0;

        A.volume = volume;
        A.clip = GetRandomClip();
        

  

        while(playing)
        {
            A.volume = volume;
            A.clip = GetRandomClip();
            SetPitch(Random.Range(0.8f, 1.6f));
            A.Play();
            
            yield return new WaitForSecondsRealtime(A.clip.length - FadeInNOut.y);
            yield return new WaitForSecondsRealtime(Random.Range(DelayBetween.x, DelayBetween.y));

            B.volume = volume;
            B.clip = GetRandomClip();
            SetPitch(Random.Range(0.8f, 1.6f));
            B.Play();
            yield return new WaitForSecondsRealtime(B.clip.length - FadeInNOut.x);
            yield return new WaitForSecondsRealtime(Random.Range(DelayBetween.x, DelayBetween.y));
        }
    }

    void XFade(AudioSource s1, AudioSource s2)
    {

    }

    public void SetPitch(float _pitch)
    {
        pitch = _pitch;
        A.pitch = B.pitch = pitch;
    }

    AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Count - 1)];
    }
}

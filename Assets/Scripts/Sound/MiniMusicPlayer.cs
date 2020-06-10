using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMusicPlayer : MonoBehaviour
{
    private static MiniMusicPlayer _instance;
    public static MiniMusicPlayer Instance()
    {
        return _instance;
    }
    AudioSource music;

    public float deathPitch = 0.1f;
    public float dieRepitchTime = 3f;
    public float restartRepitchTime = 1.2f;
    public float fadeTime = 1.2f;

    public float pauseFadeTime = 0.3f;
    public float pauseVolume = 0.12f;
    public float orig_volume;
    bool init = false;

    public bool isPlaying = false;

    Queue<Coroutine> coroutines = new Queue<Coroutine>();
    Coroutine diePitchRoutine, restartPitchRoutine, fadeRoutine, pauseRoutine;

    string[] audioclips = { "Start screen", "Hotel", "Yonk_s Kitchen" , "Underworld" };
    string audio_file = "";

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (!_instance)
        {
            //Debug.Log("first instance");
            _instance = this;

            music = GetComponent<AudioSource>();            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Initialize();
    }

    private void OnLevelWasLoaded(int level)
    {
        //Debug.Log("loaded!");
        Initialize();
    }

    public void Initialize()
    {
        //for (int i = 0; i < coroutines.Count; i++)
        //{
        //    StopCoroutine(coroutines.Dequeue());
        //}
        //StopAllCoroutines();
        if(SceneManager.GetActiveScene().name.Contains("title"))
        {
            Debug.Log("title");
            audio_file = audioclips[0];
        }
        else if (SceneManager.GetActiveScene().name.Contains("level_1"))
        {
            Debug.Log("level_1");
            audio_file = audioclips[1];
        }
        else if (SceneManager.GetActiveScene().name.Contains("level_2"))
        {
            Debug.Log("level_2");
            audio_file = audioclips[2];
        }
        else if (SceneManager.GetActiveScene().name.Contains("level_3"))
        {
            Debug.Log("level_3");
            audio_file = audioclips[3];
        }
        else if (SceneManager.GetActiveScene().name.Contains("cutscene"))
        {
            Debug.Log("cutscene");
            music.Stop();
        }

        AudioClip clip1 = (AudioClip)Resources.Load("Music/" + audio_file);

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        if(diePitchRoutine != null)
            StopCoroutine(diePitchRoutine);
        if(pauseRoutine != null)
            StopCoroutine(pauseRoutine);

        if (!init)
        {
            if (!music)
            {
                music = GetComponent<AudioSource>();
            }
            music.clip = clip1;

            music.Play();
            init = true;
        }

        music.pitch = 1f;
        music.volume = orig_volume;

        //Debug.Log("music volume: " + orig_volume);

        if(restartPitchRoutine != null)
            StopCoroutine(restartPitchRoutine);        
    }

    // Update is called once per frame
    void Update()
    {
        isPlaying = music.isPlaying;
    }

    public void DiePitch()
    {
        ChangePitch(1f, deathPitch, dieRepitchTime);
    }

    public void RestartPitch()
    {
        if(music.pitch < 1)
            ChangePitch(deathPitch, 1f, restartRepitchTime);
    }

    public void ChangePitch(float pitch1, float pitch2, float pitchTime)
    {
        if (restartPitchRoutine != null)
            StopCoroutine(restartPitchRoutine);
        if (diePitchRoutine != null)
            StopCoroutine(diePitchRoutine);        
        
        if (pitch1 < 1)
            restartPitchRoutine = StartCoroutine(PitchRoutine(pitch1, pitch2, pitchTime));
        else
            diePitchRoutine = StartCoroutine(PitchRoutine(pitch1, pitch2, pitchTime));              
    }

    IEnumerator PitchRoutine(float pitch1, float pitch2, float pitchTime)
    {
        float startTime = Time.time;
        float lerper;
        while(Time.time < startTime + pitchTime)
        {
            lerper = (Time.time - startTime) / pitchTime;
            music.pitch = Mathf.SmoothStep(pitch1, pitch2, lerper);
            //SoundManager.Instance().SetGlobalPitch(music.pitch);
            yield return new WaitForEndOfFrame();
        }
    }

    public void DestroyMe()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(MusicFadeOut());
    }

    IEnumerator MusicFadeOut()
    {
        float startTime = Time.time;
        float lerper;
        init = false;
        while (Time.time < startTime + fadeTime)
        {
            lerper = (Time.time - startTime) / fadeTime;
            music.volume = Mathf.SmoothStep(orig_volume, 0f, lerper);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Destroying!");
        //Destroy(this.gameObject);   
    }

    public void FadeMusicOnPause(bool paused)
    {
        if (pauseRoutine != null)
            StopCoroutine(pauseRoutine);

        if(this)
            pauseRoutine = StartCoroutine(PauseFade(paused));
    }

    IEnumerator PauseFade(bool paused)
    {
        float startTime = Time.unscaledTime;
        float lerper;
        
        while (Time.unscaledTime < startTime + pauseFadeTime)
        {
            lerper = (Time.unscaledTime - startTime) / pauseFadeTime;

            music.volume = paused ? Mathf.SmoothStep(orig_volume, pauseVolume, lerper) : Mathf.SmoothStep(pauseVolume, orig_volume, lerper);  
            
            yield return new WaitForSecondsRealtime(0.016f);
        }
        music.volume = paused ? pauseVolume : orig_volume;
    }
}

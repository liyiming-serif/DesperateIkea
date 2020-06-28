using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    public static GameManager Instance()
    {
        if (m_instance == null)
        {
            m_instance = new GameManager();
        }
        return m_instance;
    }

    public bool isTitle;
    public Text countdownText;
    public Text itemText;

    bool gameOver = false;
    Transform winObject;

    private bool pause;
    public Image pauseWindow;

    void Awake()
    {
        if (!m_instance)
            m_instance = this;

        countdownText.gameObject.SetActive(false);
        itemText.gameObject.SetActive(false);

        if (isTitle)
        {

            StartCoroutine("CountDown");
        }


        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("rope"), LayerMask.NameToLayer("grabbed"));

        gameOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!isTitle)
        {
            //TankController.Instance().EnableControls(true);
        }
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        Camera.main.GetComponent<ScreenImageEffect>().FadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CountDown()
    {
        /*
        countdownText.gameObject.SetActive(true);
        TankController.Instance().EnableControls(false);
        //Time.timeScale = 0;
        for (var i = 0; i < 3; i ++)
        {
            countdownText.text = "" + (3 - i);
            yield return new WaitForSecondsRealtime(1.0f);
        }
        countdownText.text = "G";

        int os = Random.Range(3, 8);
        int excs = Random.Range(3, 8);

        Debug.Log("ticks: " + (os + excs));


        float tick = 0.035f;
        for (int i = 0; i < os; i++)
        {
            countdownText.text += Random.Range(0.0f, 1.0f) > 0.6f ? "o" : "O";
            yield return new WaitForSecondsRealtime(tick);
        }


        for (int i = 0; i < excs; i++)
        {
            if(i == 0)
            {
                countdownText.text += "!";
            }
             else
            {
                countdownText.text += Random.Range(0.0f, 1.0f) > 0.7f ? "!" : "1";
            }
            yield return new WaitForSecondsRealtime(tick);
        }

        
        yield return new WaitForSecondsRealtime(1f - tick * (os + excs));
        countdownText.gameObject.SetActive(false);
        TankController.Instance().EnableControls(true);
        //   Time.timeScale = 1;
        */
        yield return new WaitForEndOfFrame();
        TankController.Instance().EnableControls(true);
    }

    public void Win(Transform target)
    {
        if (!gameOver)
        {
            gameOver = true;
            TankController.Instance().EnableControls(false);
            
            winObject = target;

            StartCoroutine(WinAnimation());
        }
    }

    IEnumerator WinAnimation()
    {
        float startOrtho = Camera.main.orthographicSize;
        float endOrtho = 1.5f;

        float startTime = Time.fixedTime;
        float animTime = 0.3f + SoundManager.Instance().mcguffinRetrieve.clip.length;

        MusicManager.Instance().FadeTo(0.01f, 0.3f);

        SoundManager.Instance().mcguffinRetrieve.Play();

        string targetText = winObject.GetComponent<McGuffinGenerator>().GetMcGuffinName();
        targetText = targetText.ToLower();
        if (targetText[0] == 'a' || targetText[0] == 'e' || targetText[0] == 'i' || targetText[0] == 'o' || targetText[0] == 'u')
        {
            countdownText.text += "n";
        }
        countdownText.gameObject.SetActive(true);



        float lerper = 0;
        while (lerper <= 1f)
        {
            lerper = (Time.fixedTime - startTime) / animTime;

            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, winObject.position, 0.3f);
            Camera.main.orthographicSize = Mathf.Lerp(startOrtho, endOrtho, lerper);

            winObject.transform.rotation = Quaternion.Lerp(winObject.transform.rotation, Quaternion.Euler(0, 0, 0), 0.3f);

            yield return new WaitForFixedUpdate();
        }

        

        itemText.text = "";
        itemText.gameObject.SetActive(true);

        animTime = 0.3f + SoundManager.Instance().mcguffinOpen.clip.length;
        SoundManager.Instance().mcguffinOpen.Play();


        int idx = 0;
        float tick = animTime / targetText.Length;

        startTime = Time.fixedTime;

        while (idx < targetText.Length)
        {
            itemText.text += targetText.Substring(idx, 1);
            idx++;
            yield return new WaitForSecondsRealtime(tick * (1f - (1f * idx / targetText.Length)));
        }

        if (Random.Range(0, 10) > 8)
        {
            while (Time.fixedTime - startTime < animTime)
            {
                itemText.text += Random.Range(0, 10) > 3 ? "!" : "1";
                yield return new WaitForSecondsRealtime(tick);
            }
        }

        MusicManager.Instance().FadeTo(MusicManager.Instance().maxVolume, 0.3f);
    }

    IEnumerator PauseCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
            {
                Pauser();
            }
            yield return new WaitForSecondsRealtime(0.016f);
        }
    }

    public void Pauser()
    {
        pause = !pause;
        MiniMusicPlayer.Instance().FadeMusicOnPause(pause);
        pauseWindow.gameObject.SetActive(pause);

        Time.timeScale = pause ? 0 : 1;
        //Debug.Log("paused? " + pause);
        //Debug.Log("Timescale " + Time.timeScale);
    }

    public bool IsPaused()
    {
        return pause;
    }
}

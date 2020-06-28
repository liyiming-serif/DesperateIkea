using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniSceneManager : MonoBehaviour
{

    private static MiniSceneManager _instance;
    public static MiniSceneManager Instance()
    {
        return _instance;
    }

    public string titleScreen;
    public string nextLevel;

    private void Awake()
    {
        if (!_instance)
            _instance = this;
    }

    public void Reload()
    {
        if (GameManager.Instance().IsPaused())
            GameManager.Instance().Pauser();

        MiniMusicPlayer.Instance().RestartPitch();
        MiniMusicPlayer.Instance().Initialize();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        if (GameManager.Instance())
        {
            if (GameManager.Instance().IsPaused())
                GameManager.Instance().Pauser();
        }

        if (MiniMusicPlayer.Instance())
            MiniMusicPlayer.Instance().DestroyMe();
        SceneManager.LoadScene(titleScreen);
    }

    public void NextLevel(string theNextLevel)
    {
        if(GameManager.Instance())
        {
            if (GameManager.Instance().IsPaused())
                GameManager.Instance().Pauser();
        }
        if(MiniMusicPlayer.Instance())
            MiniMusicPlayer.Instance().DestroyMe();

        SceneManager.LoadScene(theNextLevel);
    }

    public void NextLevel()
    {
        if (GameManager.Instance())
        {
            if (GameManager.Instance().IsPaused())
                GameManager.Instance().Pauser();
        }
        if (MiniMusicPlayer.Instance())
            MiniMusicPlayer.Instance().DestroyMe();
        Debug.Log("nextLevel: " + nextLevel);
        SceneManager.LoadScene(nextLevel);
    }

    public void NextLevel(float delay)
    {
        if (GameManager.Instance())
        {
            if (GameManager.Instance().IsPaused())
                GameManager.Instance().Pauser();
        }
        if (MiniMusicPlayer.Instance())
            MiniMusicPlayer.Instance().DestroyMe();

        StartCoroutine(NextLevelRoutine(delay));
        //Debug.Log("nextLevel: " + nextLevel);        
    }

    IEnumerator NextLevelRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(nextLevel);
    }
}

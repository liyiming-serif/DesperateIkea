using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anyKeyStart : MonoBehaviour {

    public string levelName;

    private void Start()
    {
        if(MiniMusicPlayer.Instance() != null)
            MiniMusicPlayer.Instance().DestroyMe();
    }
    // Update is called once per frame
    void Update () {
		if (Input.anyKey) {
			SceneManager.LoadScene(levelName);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PressToSkip : MonoBehaviour
{
	public PlayableAsset timeline;
	public PlayableDirector director;
	//public string sceneName;


    // Update is called once per frame
    void Update()
    {
		//if (Input.anyKey) 
		//{
		//	if (timeline != null) 
		//	{
		//		director.Stop ();
		//		director.Play (timeline);
		//	}
		//	if (sceneName != null) 
		//	{
		//		SceneManager.LoadScene (sceneName);
		//	}
		//	else 
		//	{
		//		Debug.Log ("everything is null, why'd you leave my spaces blank, ya jerk?");	
		//	}
		//}
    }

    public void StopIt()
    {
        if (timeline != null)
        {
            director.Stop();
            director.Play(timeline);
        }
    }
}

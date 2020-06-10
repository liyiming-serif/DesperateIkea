using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class test_ambience_randomizer : MonoBehaviour {

    AudioSource src;

    public Vector2 pitch = new Vector2(-1.4f, 1.6f);
    public Vector2 startDelay = new Vector2(0.04f, 1.5f);
    // Use this for initialization
    void Start () {
        src = GetComponent<AudioSource>();

        src.pitch = Random.Range(pitch.x, pitch.y);
        StartCoroutine(DelayPlay());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DelayPlay()
    {
        float delay = Random.Range(startDelay.x, startDelay.y);
        yield return new WaitForSeconds(delay);

        src.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    
    private static SoundManager _instance;
    public static SoundManager Instance()
    {
        return _instance;
    }

    //public AudioSource

    public List<SoundItem> soundItems = new List<SoundItem>();
    public AudioSource cableRetract, cableRetractLOOP, cableSnap, ringTrigger, mcguffinTrigger, mcguffinRetrieve, mcguffinOpen;
    private void Awake()
    {
        if (!_instance)
            _instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddSoundItem(SoundItem item)
    {
        soundItems.Add(item);
    }

    public void SetGlobalPitch(float pitch)
    {
        for(int i = 0; i < soundItems.Count; i++)
        {
            soundItems[i].SetPitch(pitch);
        }
    }
}


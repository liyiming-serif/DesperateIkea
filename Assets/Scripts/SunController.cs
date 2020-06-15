using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public float easterEggChance = 0.09f;
    public Sprite alternateSprite;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.value < easterEggChance)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = alternateSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

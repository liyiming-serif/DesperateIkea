﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McGuffinGenerator : MonoBehaviour
{
    string[] ADVERBS = {
        "extremely",  "alarmingly",  "frankly",  "boastfully",  "absentmindedly",  "subtly",  "knowingly",  "constantly",  "sometimes",  "twice",  "unethically",  "positively",  "majestically",  "solidly",  "kind of",  "questionably",  "somewhat",  "terribly",  "currently",  "dreamily",  "fascinatingly",  "slightly",  "unequivocally",  "horrifyingly"
    };

    string[] ADJECTIVES = {
        "worthless", "cherry-picked", "headless", "bean-filled", "half-empty", "aromatic", "smelly", "salty", "economic", "hypnotic", "chubby", "spotty", "tender", "flavourful", "mundane", "enchanted", "strange", "snobby", "soft", "hardened", "sour", "impractical", "crooked", "tough", "puffy", "climate-controlled", "half-working", "shiny", "rusty", "polished", "plastic", "painted", "fake", "peaceful", "belligerent", "automatic", "electronic", "nuclear-powered", "wooden", "rusted", "iron", "pasty", "lacey", "rotund", "noisy",  "caked up",  "rotting",  "sharpened",  "presumptuous",  "funny-tasting",  "toasted",  "fashionable",  "large",  "tiny",  "gargantuan",  "bus-sized",  "microscopic", "cobbled together", "fuzzy"
    };

    string[] MODIFIERS = {
        "fish", "dog", "bag of", "flying", "left-handed", "ambidextrous", "tomato", "cake", "hair", "nut-flavoured"
    };

    string[] NOUNS = {
        "skull", "bone", "seat", "toy", "board", "box", "keychain", "diamond", "jacket", "bag", "swatter", "picker-upper", "detector", "purse", "brush", "magazine", "sword", "underwear", "sack", "carriage", "alarm", "smacker", "scooper"
    };
    string[] POST_MODIFIERS = {
        "filled with"
    };

    string[] SMALL_OBJECTS = {
        "seeds", "ribbons", "sparkles", "dogs", "babies", "gum", "Adam's apples",  "oranges", "coins", "wood chips", "sea urchins",  "mushrooms", "toothpaste", "glue"
    };

    string McGuffinName = "";
    // Start is called before the first frame update
    void Start()
    {
        McGuffinName = GenerateMcGuffinName();
        Debug.Log(ADJECTIVES.Length);
        Debug.Log("Try to get the " + McGuffinName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            McGuffinName = GenerateMcGuffinName();

            Debug.Log("Try to get the " + McGuffinName);
        }
    }

    string GenerateMcGuffinName()
    {
        string mcguffy = "";
        int roll;
        roll = Random.Range(0, 10);
        if (Random.Range(0, 10) >= 5)
        {
            mcguffy += ADVERBS[Random.Range(0, ADVERBS.Length - 1)] + " ";
        }
        mcguffy += ADJECTIVES[Random.Range(0, ADJECTIVES.Length - 1)] + " ";
        if (Random.Range(0, 10) >= 3)
        {
            mcguffy += MODIFIERS[Random.Range(0, MODIFIERS.Length - 1)] + " ";
        }
        mcguffy += NOUNS[Random.Range(0, NOUNS.Length - 1)];

        if (mcguffy.Contains(" of ") && !mcguffy.Contains("kind of"))
        {
            mcguffy += "s";
        }

        return mcguffy;
    }

    public string GetMcGuffinName()
    {
        return McGuffinName;
    }
}

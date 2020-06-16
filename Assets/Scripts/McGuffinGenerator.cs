using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McGuffinGenerator : MonoBehaviour
{
    string[] ADVERBS = {
        "extremely",  "alarmingly",  "frankly",  "boastfully",  "absentmindedly",  "subtly",  "knowingly",  "constantly",  "sometimes",  "twice",  "unethically",  "positively",  "majestically",  "solidly",  "kind of",  "questionably",  "somewhat",  "terribly",  "currently",  "dreamily",  "fascinatingly",  "slightly",  "unequivocally",  "horrifyingly"
    };

    string[] ADJECTIVES = {
        "worthless", "cherry-picked", "headless", "bean-filled", "half-empty", "aromatic", "smelly", "salty", "economic", "hypnotic", "chubby", "spotty", "tender", "flavourful", "mundane", "enchanted", "strange", "snobby", "soft", "hardened", "sour", "impractical", "crooked", "tough", "puffy", "climate-controlled", "half-working", "shiny", "rusty", "polished", "plastic", "painted", "fake", "peaceful", "belligerent", "automatic", "electronic", "nuclear-powered", "wooden", "rusted", "iron", "pasty", "lacey", "rotund", "noisy",  "caked up",  "rotting",  "sharpened",  "presumptuous",  "funny-tasting",  "toasted",  "fashionable",  "large",  "tiny",  "gargantuan",  "bus-sized",  "microscopic", "cobbled together", "fuzzy", "crystal"
    };

    string[] MODIFIERS = {
        "fish", "dog", "bag of", "flying", "left-handed", "ambidextrous", "tomato", "cake", "hair", "nut-flavoured"
    };

    string[] NOUNS = {
        "skull", "bone", "seat", "toy", "board", "box", "keychain", "diamond", "jacket", "bag", "swatter", "picker-upper", "detector", "purse", "brush", "magazine", "sword", "underwear", "sack", "carriage", "alarm", "smacker", "scooper", "trimmer"
    };
    string[] POST_MODIFIERS = {
        "filled with", "for", "from"
    };

    string[] SMALL_OBJECTS = {
        "seeds", "ribbons", "sparkles", "dogs", "babies", "gum", "Adam's apples",  "oranges", "coins", "wood chips", "sea urchins",  "mushrooms", "toothpaste", "glue"
    };


    string McGuffinName = "";
    // Start is called before the first frame update
    void Start()
    {
        McGuffinName = GenerateMcGuffinName();
        //Debug.Log(ADJECTIVES.Length);
        //Debug.Log("Try to get the " + McGuffinName);
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
            mcguffy += getRandomFromArray(ADVERBS) + " ";
        }
        mcguffy += getRandomFromArray(ADJECTIVES) + " ";
        if (Random.Range(0, 10) >= 3)
        {
            mcguffy += getRandomFromArray(MODIFIERS) + " ";
        }
        mcguffy += getRandomFromArray(NOUNS);

        if (mcguffy.Contains(" of ") && !mcguffy.Contains("kind of"))
        {
            mcguffy += "s";
        }

        if(Random.Range(0, 10) >= 7)
        {
            roll = Random.Range(0, 10);
            string postModifier;
            if(roll >= 6)
            {
                postModifier = POST_MODIFIERS[0];
            } else if(roll >= 2 && roll < 6)
            {
                postModifier = POST_MODIFIERS[1];
            } else
            {
                postModifier = POST_MODIFIERS[2];
            }


            if(postModifier.Equals("from"))
            {
                postModifier += " ";
                roll = Random.Range(0, 10);
                if (roll >= 6)
                {
                    postModifier += "the " + Random.Range(0, 195) * 10 + "'s";
                } else if(roll >= 2 && roll < 6)
                {
                    postModifier += Random.Range(0, 300) * 10 + " B.C.";
                } else {
                    postModifier += (Random.Range(0.3f, 150f)).ToString("##.#") + " million years ago";
                }
            } else
            {
                postModifier += " " + getRandomFromArray(SMALL_OBJECTS);
            }
            mcguffy += " " + postModifier;
        }


        return mcguffy;
    }

    string getRandomFromArray(string[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }

    public string GetMcGuffinName()
    {
        return McGuffinName;
    }
}

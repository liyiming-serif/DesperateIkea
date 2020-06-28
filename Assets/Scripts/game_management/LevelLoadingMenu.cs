using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoadingMenu : MonoBehaviour
{
    public List<string> levels;
    [SerializeField] Button theButton;
    [SerializeField] GameObject levelSelectMenu;
    // Start is called before the first frame update
    void Start()
    {
        theButton.gameObject.SetActive(false);
        levelSelectMenu.SetActive(false);

        float buttonsPerRow = 7;
        float gapWidth = 30;
        float rowWidth = theButton.GetComponent<RectTransform>().localPosition.x * 2;
        
        for (int k = 0; k < levels.Count; k++)
        {
            Button img = Instantiate(theButton, theButton.transform.parent);
            Vector2 pos = img.GetComponent<RectTransform>().localPosition;
            pos.x = -(k % buttonsPerRow - (buttonsPerRow / 2f) + 0.5f) * ((rowWidth - buttonsPerRow * gapWidth) / buttonsPerRow - gapWidth);
            pos.y -= Mathf.Floor(k / (buttonsPerRow)) * 90;

            img.GetComponent<RectTransform>().localPosition = pos;
            img.gameObject.transform.GetChild(0).GetComponent<Text>().text = "" + (k + 1);
            img.gameObject.name = "" + (k + 1);
            

            int b = System.Int16.Parse(img.gameObject.name);
            img.onClick.AddListener(() => GetThatLevel(b - 1));
            img.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetThatLevel(int n)
    {
        MiniSceneManager.Instance().NextLevel(levels[n]);
        //Debug.Log("you got; " + n);
    }
}

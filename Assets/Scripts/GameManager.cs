using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text countdownText;
    public GameObject net;

    void Awake()
    {
        if (!m_instance)
            m_instance = this;

        countdownText.gameObject.SetActive(false);
        StartCoroutine("CountDown");
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void EndGame(float xPos)
    {
        int player = xPos < net.transform.position.x ? 2 : 1;
        Debug.Log("player " + player + " wins!");
    }
}

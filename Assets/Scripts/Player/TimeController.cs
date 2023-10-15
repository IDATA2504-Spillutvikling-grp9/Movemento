using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeController : MonoBehaviour
{
    public float timer = 0f;
    public GameObject timeHolderGameObject;
    public TMP_Text timerText; // Reference to the TextMeshPro component
    public TMP_Text endTimerTex; // Reference to the TextMeshPro component

    void Start()
    {
        if (gameObject.tag == "TimeHolder")
        {
            DontDestroyOnLoad(timeHolderGameObject);
        }
    }

    void Update()
    {
        LevelTimer();
        UpdateTimerText();
    }

    void LevelTimer()
    {
        timer += Time.deltaTime;
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");

            if (gameObject.tag == "TimeHolder")
            {
                timerText.text = "Total Time: " + minutes + ":" + seconds;
            }
            else
            {
                timerText.text = "Level Time: " + minutes + ":" + seconds;
                endTimerTex.text = "Level Time: " + minutes + ":" + seconds;
            }
        }
        else
        {
            Debug.LogWarning("TextMeshPro component is not assigned. Please assign a TextMeshPro component to the timerText variable.");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] AudioClip sound_click;
    [SerializeField] AudioClip sound_hover;
    [Space(10)] [SerializeField] AudioSource audioSource;
    public GameObject pauseScreen;

    public string sceneName;

    private bool isPaused = false;

    public GameObject optionMenuScreen;

    void Start()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        HideOptionScreen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
    }
    
    public void UIClick()
    {
        audioSource.PlayOneShot(sound_click);
    }

    public void UIHover()
    {
        audioSource.PlayOneShot(sound_hover);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // You can add logic here if you want to stop the sound on hover exit.
    }

    public void NextLevel() {
        StartCoroutine(LoadSceneWithDelay(sceneName, 0.5f));
    }

    IEnumerator LoadSceneWithDelay(string sceneName, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(sceneName);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void HideOptionScreen()
    {
        optionMenuScreen.SetActive(false);
    }

    public void OpenOptionScreen()
    {
        optionMenuScreen.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void BackButton(GameObject gameObject)
    {
        gameObject.SetActive(false);
        pauseScreen.SetActive(true);
    }
}

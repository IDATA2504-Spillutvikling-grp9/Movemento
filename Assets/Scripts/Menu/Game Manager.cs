using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The audio clip to be played on click.
    /// </summary>
    [SerializeField] private AudioClip sound_click;

    [Space(10)] 
    /// <summary>
    /// The audio source component for playing sounds.
    /// </summary>
    [SerializeField] private AudioSource audioSource;

    public AudioClip sound_hover;

    /// <summary>
    /// The pause screen GameObject.
    /// </summary>
    [SerializeField] private GameObject pauseScreen;

    /// <summary>
    /// The name of the scene to be loaded.
    /// </summary>
    public string sceneName;

    private bool isPaused = false;

    /// <summary>
    /// The option menu screen GameObject.
    /// </summary>
    [SerializeField] private GameObject optionMenuScreen;

    /// <summary>
    /// The audio source component for playing music.
    /// </summary>
    [SerializeField] public AudioMixer musicAudioSource;

    /// <summary>
    /// The Game audio source
    /// </summary>
    [SerializeField] public AudioMixer gameAudioSource;

    private Resolution[] resolutions;

    public Dropdown resolutionDropdown;
    

    void Start()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
        
        HideOptionScreen();
        setUpResolutions();
    }

    private void setUpResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
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

    /// <summary>
    /// Plays the click sound.
    /// </summary>
    public void UIClick()
    {
        audioSource.PlayOneShot(sound_click);
    }

    public void UIHover()
    {
        audioSource.PlayOneShot(sound_hover);
    }

    public void NextLevel()
    {
        StartCoroutine(LoadSceneWithDelay(sceneName, 0.5f));
    }

    IEnumerator LoadSceneWithDelay(string sceneName, float delayTime)
    {
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
    }

    public void BackButton()
    {
        optionMenuScreen.SetActive(false);

    }

    public void SetVolumeMusic(float volume)
    {
        musicAudioSource.SetFloat("musicVolume", volume);
    }

    public void SetVolumeGame(float volume)
    {
        gameAudioSource.SetFloat("gameVolume", volume);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

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

    public Slider musicVolumeSlider;
    public Slider gameVolumeSlider;

    public GameObject endLevelScreen;

    public float timer = 0f;
    public GameObject timeHolderGameObject;
    public TMP_Text timerText; // Reference to the TextMeshPro component
    public TMP_Text endTimerTex; // Reference to the TextMeshPro component
    

    void Start()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
        
        HideOptionScreen();
        setUpResolutions();
        SetSliderValueMusic();
        SetSliderValueGame();
        if (gameObject.tag == "TimeHolder")
        {
            DontDestroyOnLoad(timeHolderGameObject);
        }
        endLevelScreen.SetActive(false);
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
        LevelTimer();
        UpdateTimerText();
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
        HideOptionScreen();
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
        SceneManager.LoadScene(sceneName);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = Vector3.zero;
        }
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

    public bool getIsPause() {
        return this.isPaused;
    }

    void SetSliderValueMusic() {
        float currentMusicVolume;
        if (musicAudioSource.GetFloat("musicVolume", out currentMusicVolume))
        {
            musicVolumeSlider.value = currentMusicVolume;
        }
    }

     void SetSliderValueGame() {
        float currentGameVolume;
        if (gameAudioSource.GetFloat("gameVolume", out currentGameVolume))
        {
            gameVolumeSlider.value = currentGameVolume;
        }
    }

    public void EndLevel() {
        Time.timeScale = 0f;
        isPaused = true;
        if (endLevelScreen != null)
        {
            endLevelScreen.SetActive(true);
        }
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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{

    [Header("Sound")]
    /// <summary>
    /// The audio source component for playing sounds.
    /// </summary>
    [SerializeField] private AudioSource audioSource;
        
    /// <summary>
    /// The audio clip to be played on hover.
    /// </summary>
    [SerializeField] public AudioClip sound_hover;
    
    /// <summary>
    /// The audio clip to be played on click.
    /// </summary>
    [SerializeField] private AudioClip sound_click;
    
    /// <summary>
    /// The audio source component for playing music.
    /// </summary>
    [SerializeField] public AudioMixer musicAudioSource;

    /// <summary>
    /// The Game audio source
    /// </summary>
    [SerializeField] public AudioMixer gameAudioSource;

    [Space(5)] 
    
    [Header("Screens")]

    /// <summary>
    /// The pause screen GameObject.
    /// </summary>
    [SerializeField] private GameObject pauseScreen;

    /// <summary>
    /// The option menu screen GameObject.
    /// </summary>
    [SerializeField] private GameObject optionMenuScreen;

    /// <summary>
    /// The ende of level screen
    /// </summary>
    [SerializeField] public GameObject endLevelScreen;

    /// <summary>
    /// The healing ability screen
    /// </summary>
    [SerializeField] public GameObject healingAiblityScreen;
    /// <summary>
    /// The name of the scene to be loaded.
    /// </summary>
    public string sceneName;

    [Space(5)]

    [Header("UI settings")]

    /// <summary>
    /// The resolution drop down menu
    /// </summary>
    public Dropdown resolutionDropdown;

    /// <summary>
    /// The slider for controlling music volume
    /// </summary>
    public Slider musicVolumeSlider;

    /// <summary>
    /// The slider for controlling game volume
    /// </summary>
    public Slider gameVolumeSlider;

    /// <summary>
    /// Reference to the TextMeshPro component that show the timer in the level
    /// </summary>
    public TMP_Text timerText;

    /// <summary>
    ///  Reference to the TextMeshPro component that shows the end time for the level
    /// </summary>
    public TMP_Text endTimerTex;

    /// <summary>
    ///  Reference to the TextMeshPro component that shows the next level name
    /// </summary>
    public TMP_Text nextLevelText;

    /// <summary>
    ///  The timer
    /// </summary>
    public float timer = 0f;

    private Resolution[] resolutions; //List of resolutions.

    private bool isPaused = false; // Bool that holds the value if the game is paused or not.

    private Dictionary<string, float> levelDataDictionary = new Dictionary<string, float>(); //Dictonary to store data
    

    /*
        Start is called before the first frame update.
        Sets up the requierd UI elements.
    */
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
        endLevelScreen.SetActive(false);
        healingAiblityScreen.SetActive(false);
    }

    /*
        Set up the resolution drop down.
        Populates the dropw down with the resolution that the user pc have.
        Also set the resolution tho what he user have on the pc.
    */
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

    /*
        Update is called once per frame
    */
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

    /*
        Pauses the game
    */
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }
    }

    /*
        Resume the game
    */
    public void ResumeGame()
    {
        //isPaused = false;
        Time.timeScale = 1f;
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
            isPaused = false;
        }
        HideOptionScreen();
    }

    /*
        Play the click sound
    */
    public void UIClick()
    {
        audioSource.PlayOneShot(sound_click);
    }

    /*
        Play the hover sound
    */
    public void UIHover()
    {
        audioSource.PlayOneShot(sound_hover);
    }

    /*
        Metod goes to next scene that are given in sceneName
    */
    public void NextLevel()
    {
        SceneManager.LoadScene(sceneName);
        isPaused = false;
        Time.timeScale = 1f;
    }

    /*
        Returns to the main menu
    */
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /*
        Hides the option screen
    */
    public void HideOptionScreen()
    {
        optionMenuScreen.SetActive(false);
    }

    /*
        Shows the option screen
    */
    public void OpenOptionScreen()
    {
        optionMenuScreen.SetActive(true);
    }

    /*
        Back button that goes back from option screen to pause screen
    */
    public void BackButton()
    {
        optionMenuScreen.SetActive(false);

    }

    /*
       Set the audio mixer music to the value of the slider music
    */
    public void SetVolumeMusic(float volume)
    {
        musicAudioSource.SetFloat("musicVolume", volume);
    }

    /*
        Set the audio mixer game to the value of the slider game
    */
    public void SetVolumeGame(float volume)
    {
        gameAudioSource.SetFloat("gameVolume", volume);
    }

    /*
        turn on and of full screen
    */
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    /*
        When this method is called the resolution on the game changes
    */
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /*
        Return the value if the game is paused or not.
        True if game is paused, false if it is not.
    */
    public bool getIsPause() {
        return this.isPaused;
    }

    /*
        Sets up the music slider to what the the audio mixer music value has.
    */
    void SetSliderValueMusic() {
        float currentMusicVolume;
        if (musicAudioSource.GetFloat("musicVolume", out currentMusicVolume))
        {
            musicVolumeSlider.value = currentMusicVolume;
        }
    }

    /*
        Sets up the game slider to what the the audio mixer game value has.
    */
     void SetSliderValueGame() {
        float currentGameVolume;
        if (gameAudioSource.GetFloat("gameVolume", out currentGameVolume))
        {
            gameVolumeSlider.value = currentGameVolume;
        }
    }

    /*
        Gets called in the end of the level.
        Shows the end level screen and pauses the game.
    */
    public void EndLevel() {
        Time.timeScale = 0f;
        isPaused = true;
        endLevelScreen.SetActive(true);
        UpdateNexLevelText();
    }

    /*
       Calculate the timer on the level
    */
    void LevelTimer()
    {
        timer += Time.deltaTime;
    }

    /*
        Update the UI elements that holds the timer
    */
    void UpdateTimerText()
    {
        if (timerText != null)
        {
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");
            timerText.text = "Level Time: " + minutes + ":" + seconds;
            endTimerTex.text = "Level Time: " + minutes + ":" + seconds;
        }
    }

    /*
        Method that shows what is the next level name on the end level screen.
    */
    void UpdateNexLevelText() {
        nextLevelText.text = "Next level is: " + sceneName;
    }

    /*
        Turns on and of the screen that shows info about the healing ability
    */
    public void TurnOnAndOfHealingAbilityScreen() {
        healingAiblityScreen.SetActive(true);
        StartCoroutine(HealingAbilityScreenColldown());
    }

    /*
        Timer that turn of the healt ability screen afther 5 secunds.
    */
    IEnumerator HealingAbilityScreenColldown() {
        yield return new WaitForSeconds(5f);
        healingAiblityScreen.SetActive(false);
    }
}
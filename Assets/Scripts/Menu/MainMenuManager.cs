using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{

    private Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// The audio source component for playing music.
    /// </summary>
    [SerializeField] public AudioMixer musicAudioSource;

    /// <summary>
    /// The Game audio source
    /// </summary>
    [SerializeField] public AudioMixer gameAudioSource;

    public AudioClip sound_hover;
    [SerializeField] private AudioClip sound_click;

    
    // Start is called before the first frame update
    void Start()
    {
        setUpResolutions();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

        public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolumeMusic(float volume)
    {
        musicAudioSource.SetFloat("musicVolume", volume);
    }

    public void SetVolumeGame(float volume)
    {
        gameAudioSource.SetFloat("gameVolume", volume);
    }

    public void UIClick()
    {
        audioSource.PlayOneShot(sound_click);
    }

    public void UIHover()
    {
        audioSource.PlayOneShot(sound_hover);
    }

    public void QuitGame()
    {
        // This will only work in standalone builds (Windows, Mac, Linux)
        #if UNITY_STANDALONE
            Application.Quit();
        #endif

        // This will work in the editor play mode
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void loadLevel(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}

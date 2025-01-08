using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    public Slider musicSlider;
    public Slider sfxSlider;

    private AudioManager audioManager;

    public GameObject controlsMenuUI;


    private void Start()
    {
        audioManager = Object.FindFirstObjectByType<AudioManager>();

        if (audioManager != null)
        {
            musicSlider.value = audioManager.musicSource.volume;
            sfxSlider.value = audioManager.SFXSource.volume;
        }

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void ControlsMenu()
    {
        controlsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void CloseControlsMenu()
    {
        controlsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    void SetMusicVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume);
        }
    }

    void SetSFXVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetSFXVolume(volume);
        }
    }
}

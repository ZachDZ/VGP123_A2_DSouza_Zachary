using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class CanvasManager : MonoBehaviour
{
    [Header("Audo Mixer")]
    public AudioMixer audioMixer;

    [Header("Button")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;
    public Button yesButton;
    public Button noButton;
    public Button returnToMenuButton;
    public Button returnToGameButton;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public GameObject quitMenu;

    [Header("Text")]
    public Text healthText;
    public Text scoreText;
    public TextMeshProUGUI masterVolText;
    public TextMeshProUGUI musicVolText;
    public TextMeshProUGUI sfxVolText;

    [Header("Slider")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    [Header("Screen Overlays")]
    public GameObject grayScreen;

    [HideInInspector] public bool isPaused = false;

    [HideInInspector] public static float masterVolume = 100;
    [HideInInspector] public static float musicVolume = 100;
    [HideInInspector] public static float sfxVolume = 100;

    // Start is called before the first frame update
    void Start()
    {
        if (playButton)
        {
            playButton.onClick.AddListener(() => GameManager.ChangeScene(1));
        }

        if (settingsButton)
        {
            settingsButton.onClick.AddListener(ShowSettingsMenu);
        }

        if (returnToMenuButton)
        {
            ResumeGame();
            returnToMenuButton.onClick.AddListener(() => GameManager.ChangeScene(0));
        }

        if (returnToGameButton)
        {
            ResumeGame();
            returnToGameButton.onClick.AddListener(ResumeGame);
        }

        if (quitButton)
        {
            quitButton.onClick.AddListener(ShowQuitMenu);
        }

        if (backButton)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                backButton.onClick.AddListener(ShowMainMenu);
            }

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                backButton.onClick.AddListener(PauseGame);
            }
        }

        if (noButton)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                noButton.onClick.AddListener(ShowMainMenu);
            }

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                noButton.onClick.AddListener(PauseGame);
            }
        }

        if (yesButton)
        {
            yesButton.onClick.AddListener(Quit);
        }

        if (healthText)
        {
            GameManager.Instance.OnHealthValueChanged.AddListener((value) => UpdateHealthText(value));
            healthText.text = "HP: " + GameManager.Instance.health.ToString() + "❤️";
        }

        if (scoreText)
        {
            GameManager.Instance.OnScoreValueChanged.AddListener((value) => UpdateScoreText(value));
            scoreText.text = "Score: " + GameManager.Instance.score.ToString();
        }

        if (masterVolSlider)
        {
            masterVolSlider.onValueChanged.AddListener((value) => OnMaxVolChanged(value));

            float newValue;
            audioMixer.GetFloat("MasterVol", out newValue);
            masterVolSlider.value = newValue + 100;

            if (masterVolText)
            {
                masterVolText.text = Mathf.Ceil(newValue + 100).ToString();
            }

        }

        if (musicVolSlider)
        {
            musicVolSlider.onValueChanged.AddListener((value) => OnMusicVolChanged(value));

            float newValue;
            audioMixer.GetFloat("MusicVol", out newValue);
            musicVolSlider.value = newValue + 100;

            if (musicVolText)
            {
                musicVolText.text = Mathf.Ceil(newValue + 100).ToString();
            }
        }

        if (sfxVolSlider)
        {
            sfxVolSlider.onValueChanged.AddListener((value) => OnSFXVolChanged(value));

            float newValue;
            audioMixer.GetFloat("SFXVol", out newValue);
            sfxVolSlider.value = newValue + 100;

            if (sfxVolText)
            {
                sfxVolText.text = Mathf.Ceil(newValue + 100).ToString();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void OnMaxVolChanged(float value)
    {
        masterVolText.text = value.ToString();
        audioMixer.SetFloat("MasterVol", value - 100);
    }

    void OnMusicVolChanged(float value)
    {
        musicVolText.text = value.ToString();
        audioMixer.SetFloat("MusicVol", value - 100);
    }

    void OnSFXVolChanged(float value)
    {
        sfxVolText.text = value.ToString();
        audioMixer.SetFloat("SFXVol", value - 100);
    }

    void ResumeGame()
    {
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
        StopGrayScreen();
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    void PauseGame()
    {
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
        ShowGrayScreen();
        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }

    void ShowGrayScreen()
    {
        grayScreen.SetActive(true);
    }

    void StopGrayScreen()
    {
        grayScreen.SetActive(false);
    }

    void UpdateHealthText(int value)
    {
        if (healthText)
        {
            healthText.text = "HP: " + value.ToString() + "❤️";
        }
    }

    void UpdateScoreText(int value)
    {
        if (scoreText)
        {
            scoreText.text = "Score: " + value.ToString();
        }
    }

    void ShowSettingsMenu()
    {
        if (isPaused)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
        }

        settingsMenu.SetActive(true);
    }

    void ShowQuitMenu()
    {
        if (isPaused)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
        }

        quitMenu.SetActive(true);
    }

    void ShowMainMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            mainMenu.SetActive(true);
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            quitMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
    }

    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

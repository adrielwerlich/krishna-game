using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    private int currentSceneIndex;
    private bool isPaused = false;
    private bool sliderActive;



    [Header("UI")]
    public GameObject CompletedLevelMessage;
    public GameObject PlayerRotationSpeedSlider;

    [SerializeField] private bool _hasNextLevel = true;
    [SerializeField] private string _mainMenuName = "Menu";

    private string MainMenuName { get {
            if (string.IsNullOrEmpty(_mainMenuName)) return "Menu";
            return _mainMenuName;
     } 
        set => _mainMenuName = value; }

    void Start()
    {

        UpdateCurrentSceneIndex();
        if (CompletedLevelMessage != null) CompletedLevelMessage.gameObject.SetActive(false);

        bool shouldDisplay = Application.isMobilePlatform; // if is mobile, should display
        PlayerRotationSpeedSlider.gameObject.SetActive(shouldDisplay);
        sliderActive = shouldDisplay;

        InputManager.ShowRotationScrollBar += ToggleSlider;
        InputManager.ToggleShowGameMenu += TogglePause;
        InputManager.GoToMainMenu += GoToMainMenu;
        InputManager.ReloadLevel  += ReloadScene;

    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
    private void RemoveListeners()
    {
        InputManager.ShowRotationScrollBar -= ToggleSlider;
        InputManager.ToggleShowGameMenu -= TogglePause;
        InputManager.GoToMainMenu -= GoToMainMenu;
        InputManager.ReloadLevel -= ReloadScene;
    }
    private void TogglePause(bool shouldPause)
    {
        if (shouldPause)
        {
            Pause();
        } else
        {
            Unpause();
        }
    }

    void Update()
    {
        if (_hasNextLevel)
        {
            // get enemy count
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCount == 0)
            {
                // if enemy count is 0 then load next level
                CompletedLevelMessage.gameObject.SetActive(true);
                StartCoroutine(LoadNextLevel());
            }
        }

        CheckKeyPresses();
    }

    private void CheckKeyPresses()
    {

        if (Input.GetKey(KeyCode.P))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }

    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ContinueGame();
    }

    private void ContinueGame()
    {
        if (isPaused)
        {
            Unpause();
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(MainMenuName);
        ContinueGame();
    }

    private void ToggleSlider()
    {
        if (!sliderActive)
        {
            PlayerRotationSpeedSlider.gameObject.SetActive(true);
            sliderActive = true;
        }
        else
        {
            PlayerRotationSpeedSlider.gameObject.SetActive(false);
            sliderActive = false;
        }
    }

    private void UpdateCurrentSceneIndex()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(5);

        UpdateCurrentSceneIndex();
        currentSceneIndex += 1;

        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            currentSceneIndex = 1; // reset to first level
        }

        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
    }
}

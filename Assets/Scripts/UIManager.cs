using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject InGameUI;
    public GameObject DialogueBox;
    public static bool isPaused;
    public static bool isDead;

    private void Awake()
    {
        // Get the game canvas
        gameCanvas = FindObjectOfType<Canvas>();
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        
    }

    private void OnEnable()
    {
        // Subscribe to events
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        // Create text when character receives damage
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthReceived)
    {
        // Create text when character receives healing
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = healthReceived.ToString();
    }

    public void CurrentGameScene()
    {
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                break;
            case "Level0":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level0");
                break;
            case "Level1":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
                break;
            case "Level2":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
                break;
            case "Level3":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
                break;
            case "QuitScene":
                UnityEngine.SceneManagement.SceneManager.LoadScene("QuitScene");
                break;
        }
    }

    public void Update()
    {
        Damageable.DeathMenuActivate += DeathMenuActive;
    }

        public void PressedEsc()
        {
        if (isDead == false)
        {
            if (isPaused)
            {
                ResumeGame();
                InGameUI.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                PauseGame();
                InGameUI.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        }

        public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CurrentGameScene();
        if (isDead)
        {
            deathMenu.SetActive(false);
            InGameUI.SetActive(true);
            isDead = false;
        }
        
    }

    public void QuitGame()
    {
        {
            #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
                Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            #if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;
            #elif (UNITY_STANDALONE)
                Application.Quit();
            #elif (UNITY_WEBGL)
                SceneManager.LoadScene("QuitScene");
            #endif
            }
    }

    

    public void DeathMenuActive()
    {   
        isDead = true;
        StartCoroutine(DeathMenuTimer());
    }

    void DeathMenu()
    {
        deathMenu.SetActive(true);
        InGameUI.SetActive(false);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StopCoroutine(DeathMenuTimer());
    }

    IEnumerator DeathMenuTimer()
    {
        yield return new WaitForSeconds(1);
        DeathMenu();
    }

    public void DialogueBoxActive()
    {
        if (DialogueBox != null)
        {
            DialogueBox.SetActive(true);
            Debug.Log("Dialogue Box Active");
        }
    }
}


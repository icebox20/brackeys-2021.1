using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public static bool isPlaying, isPaused;

    [SerializeField]
    AudioClip buttonPressSound, maximiseSound, minimiseSound, playSound;
    AudioSource audioSource;

    [Header("Animations")]
    [SerializeField]
    Animator mainMenuanim;
    [SerializeField]
    Animator pauseMenuanim, creditsanim, gameOveranim, inGameUIanim;

    //public GameObject uiMoney;
    
    string OpenMenu_animString = "OpenMenu", CloseMenu_animString = "CloseMenu";

    void Start() {
        isPlaying = false;
        isPaused = false;
        audioSource = GetComponent<AudioSource>();
        Debug.Log(isPlaying);
        AudioManager.Instance.Play("mainMenuMusic");
    }
    void Update() {
        CheckForPause();
    }

    public void GameOver() {
        isPlaying = false;
        SFX_MaximiseMenu();
        gameOveranim.gameObject.SetActive(true);
        inGameUIanim.Play(CloseMenu_animString);
    }
    void StopTime() => Time.timeScale = 0;
    void ResumeTime() => Time.timeScale = 1;
    void CheckForPause() {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlaying && !isPaused)
            Pause();

        if (Input.GetKeyDown(KeyCode.Escape) && !isPlaying && isPaused)
            Resume();
    }
    void DisableObjectWithDelay(GameObject obj, float delay = .15f) {
        StartCoroutine(_DisableObjectWithDelay(obj, delay));
    }

    IEnumerator SetPausedToTrue() {
        yield return new WaitForSecondsRealtime(.1f);
        isPaused = true;
    }
    IEnumerator _DisableObjectWithDelay(GameObject obj, float delay = .15f) {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
    
    #region UI Buttons
    public void SFX_ButtonPressed() {
        audioSource.PlayOneShot(buttonPressSound);
    }
    public void SFX_MaximiseMenu() {
        audioSource.PlayOneShot(maximiseSound);
    }
    public void SFX_MinimiseMenu() {
        audioSource.PlayOneShot(minimiseSound);
    }

    public void Play() {
        SFX_ButtonPressed();
        isPlaying = true;
        inGameUIanim.gameObject.SetActive(true);
        audioSource.PlayOneShot(playSound);
        mainMenuanim.Play(CloseMenu_animString);
        DisableObjectWithDelay(mainMenuanim.gameObject);
        GameLogic.Instance.paused = false;
        AudioManager.Instance.Play("inGameMusic");
        //uiMoney.SetActive(true);
    }
    public void Retry() {
        SFX_ButtonPressed();
        SceneManager.LoadScene(0);
        Invoke("Play", 1f);
    }
    public void Pause() {
        StopTime();
        SFX_MaximiseMenu();
        StartCoroutine(SetPausedToTrue());
        GameLogic.Instance.paused = true;
        isPlaying = false;
        pauseMenuanim.gameObject.SetActive(true);
        pauseMenuanim.Play(OpenMenu_animString);
        GameLogic.Instance.paused = true;
        AudioManager.Instance.Play("mainMenuMusic");
        //uiMoney.SetActive(false);
    }
    public void Resume() {
        ResumeTime();
        SFX_ButtonPressed();
        SFX_MinimiseMenu();
        isPaused = false;
        GameLogic.Instance.paused = false;
        isPlaying = true;
        pauseMenuanim.Play(CloseMenu_animString);
        DisableObjectWithDelay(pauseMenuanim.gameObject, .1f);
        AudioManager.Instance.Play("inGameMusic");
        //uiMoney.SetActive(true);
    }
    public void Quit() {
        Application.Quit();
    }
    public void Open_Credits() {
        SFX_ButtonPressed();
        SFX_MaximiseMenu();
        creditsanim.gameObject.SetActive(true);
    }
    public void Close_Credits() {
        SFX_ButtonPressed();
        SFX_MinimiseMenu();

        creditsanim.Play(CloseMenu_animString);
        DisableObjectWithDelay(creditsanim.gameObject);
    }
    public void Open_MainMenu() {
        ResumeTime();
        SFX_ButtonPressed();
        AudioManager.Instance.Play("mainMenuMusic");
        GameLogic.Instance.Reset2();
        SceneManager.LoadScene(0);
    }
    #endregion

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLogic : MonoBehaviour {

    public int playerHealth = 100;

    private string storedGesture = null;

    private int gesturesActive = 0;

    public GameObject deathCanvas;

    public GameObject successCanvas;

    float timeLeft;

    bool timerRunning = false;

    void StartTimer() {
        timeLeft = 60.0f; // 1 minute
        timerRunning = true;
    }

    // Update is called once per frame
    void Update() {
        if (timerRunning) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0) {
                StageSurvived();
            }

        }
    }

    // Start is called before the first frame update
    void Start() {
        StartTimer();
    }

    void StageSurvived() {
        Debug.Log("Player survived");
        timeLeft = 0;
        timerRunning = false;
        successCanvas.SetActive(true);
    }

    public void ReceiveDamage(int damage) {
        if (timerRunning) {
            playerHealth = Math.Max(playerHealth - damage, 0);
            if (playerHealth == 0) {
                Die();
            }
        }
    }

    public void ReceiveHeal(int heal) {
        playerHealth = Math.Min(playerHealth + heal, 100);
    }

    public int IncreaseGestureActiveCount() {
        gesturesActive++;
        return gesturesActive;
    }

    public int DecreaseGestureActiveCount() {
        gesturesActive--;
        return gesturesActive;
    }

    public void SetStoredGesture(string gesture) {
        storedGesture = gesture;
    }

    public string GetAndClearStoredGesture() {
        string gesture = storedGesture;
        storedGesture = null;
        return gesture;
    }

    void Die() {
        deathCanvas.SetActive(true);
        timerRunning = false;
    }

    public bool IsAlive() {
        return playerHealth > 0;
    }

    public bool HasSurvived() {
        return timeLeft == 0;
    }

    public void DoRestart() {
        deathCanvas.SetActive(false);
        successCanvas.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        playerHealth = 100;
        gesturesActive = 0;
        StartTimer();
    }

    public void DoQuit() {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void NextLevel() {
        Debug.Log("Next Level");

        GenerateEnemy[] playerScripts = FindObjectsOfType<GenerateEnemy>();
        foreach (GenerateEnemy playerScript in playerScripts)
        {
            playerScript.IncreaseLevel();
        }
        
        DoRestart();
    }
}

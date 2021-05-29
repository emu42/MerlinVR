using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour {

    public int playerHealth = 100;

    private string storedGesture = null;

    private int gesturesActive = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ReceiveDamage(int damage) {
        playerHealth = Math.Max(playerHealth - damage, 0);
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
}
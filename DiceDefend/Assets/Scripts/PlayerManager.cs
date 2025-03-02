using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public event Action<float> OnWaveUpdate, OnGoldUpdate;
    public event Action OnRoll;
    public float CurrentGold {get{return currentGold;}}
    public float CurrentWave {get{return currentWave;}}

    [SerializeField]
    private float currentGold, currentWave;
    private void Awake() 
    {
        Instance = this;
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollDice();
        }
    }
    public void Init()
    {
        // currentGold = 0; 
        currentWave = 0;
        OnGoldUpdate?.Invoke(currentGold);
        OnWaveUpdate?.Invoke(currentWave);
    }
    public void UpdateGold(float amount)
    {
        currentGold += amount;
        OnGoldUpdate?.Invoke(currentGold);
    }
    public void UpdateWave(float waveCount)
    {
        currentWave = waveCount;
        OnWaveUpdate?.Invoke(currentWave);
    }
    public void RollDice()
    {
        OnRoll?.Invoke();
    }

}

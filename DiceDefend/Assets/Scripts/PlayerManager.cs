using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public event Action<float> OnWaveUpdate, OnGoldUpdate;

    [SerializeField]
    private float currentGold, currentWave;
    private void Awake() 
    {
        Instance = this;
    }
    public void Init()
    {
        currentGold = 0; currentWave = 0;
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

}

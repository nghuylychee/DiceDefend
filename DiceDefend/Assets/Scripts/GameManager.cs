using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField]
    private EnumConst.GameState CurrentState { get; set; }


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ChangeState(EnumConst.GameState.Init);
    }

    private void ChangeState(EnumConst.GameState newState)
    {
        CurrentState = newState;
        
        switch (newState)
        {
            case EnumConst.GameState.Init:
                InitGame();
                break;
            case EnumConst.GameState.InGame:
                StartGame();
                break;
            case EnumConst.GameState.Paused:
                PauseGame();
                break;
            case EnumConst.GameState.EndGame:
                EndGame();
                break;
        }
    }

    private void InitGame()
    {
        //Subscribe to events
        EnemyManager.Instance.OnWaveSpawn += PlayerManager.Instance.UpdateWave;
        EnemyManager.Instance.OnEnemyKilled += (goldEarn, enemyPos) =>
        { 
            PlayerManager.Instance.UpdateGold(goldEarn);
            FXManager.Instance.PlayEffectGainResource(goldEarn, enemyPos);
        };
        EnemyManager.Instance.OnWaveClear += SpawnWave;
        
        PlayerManager.Instance.OnWaveUpdate += UIManager.Instance.UpdateUIWave;
        PlayerManager.Instance.OnGoldUpdate += (currentGold) =>
        {
            UIManager.Instance.UpdateUIGold(currentGold);
            // ShopManager.Instance.CheckRollPrice(currentGold);
            ShopManager.Instance.CheckShopItemPrice(currentGold);
        };
        PlayerManager.Instance.OnRoll += DiceManager.Instance.RollDice;

        ShopManager.Instance.OnRollComplete += PlayerManager.Instance.UpdateGold;
        ShopManager.Instance.OnShopItemBuy += (diceID, dicePrice) => 
        {
            PlayerManager.Instance.UpdateGold(dicePrice); 
            DiceManager.Instance.AddDice(diceID);
        };

        DiceManager.Instance.OnDiceSpawn += (dice) =>
        {
            GridManager.Instance.PlaceDice(dice, 0, 0);
        };
        DiceManager.Instance.OnDiceDie += (dice) =>
        {
            GridManager.Instance.ReleaseGrid(dice.GridX, dice.GridY);
        };

        GridManager.Instance.Init();
        ShopManager.Instance.Init();
        DiceManager.Instance.Init();
        EnemyManager.Instance.Init();
        UIManager.Instance.Init();
        PlayerManager.Instance.Init();
        FXManager.Instance.Init();

        ChangeState(EnumConst.GameState.InGame);
    }

    private void StartGame()
    {
        EnemyManager.Instance.SpawnWave();
    }
    private void SpawnWave(float waveCount)
    {
        EnemyManager.Instance.SpawnWave();
    }

    private void PauseGame()
    {
        // Dừng tất cả logic game
        Time.timeScale = 0;
    }

    private void EndGame()
    {
        // Xử lý khi game kết thúc
        Time.timeScale = 1;
    }
}

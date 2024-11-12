using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;
    public event Action<Dice> OnDiceSpawn, OnDiceDie;
    public event Action OnUpdateDicePool;
    
    [SerializeField]
    private GameObject dicePrefab;
    [SerializeField]
    private List<Dice> diceList = new List<Dice>();
    [SerializeField]
    public List<DiceConfig> dicePool = new List<DiceConfig>();
    private void Awake() 
    {
        Instance = this;    
    }
    public void Init()
    {
        diceList = GetComponentsInChildren<Dice>().ToList();
        foreach (var dice in diceList)
        {
            dice.Init(dice.DiceTypeID);
        }
    }
    public void AddDice(int diceTypeID)
    {
        var pos = Vector3.zero;
        var dice = Instantiate(dicePrefab, pos, Quaternion.identity);
        dice.transform.SetParent(this.transform);
        diceList.Add(dice.GetComponent<Dice>());
        dice.GetComponent<Dice>().Init(diceTypeID);
        
        OnDiceSpawn?.Invoke(dice.GetComponent<Dice>());
    }
    public void RollDice()
    {
        foreach (var dice in diceList)
        {
            dice.RollDice();
        }
    }
    public void RemoveDice()
    {
        for(int i = 0; i < diceList.Count; ++i)
        {
            if (!diceList[i].IsAlive)
            {
                var dice = diceList[i];
                OnDiceDie?.Invoke(dice);
                diceList.RemoveAt(i);
                GameObject.Destroy(dice.gameObject);
            }
        }
    }
    public void UpdateDicePool()
    {
        OnUpdateDicePool?.Invoke();
    }
}

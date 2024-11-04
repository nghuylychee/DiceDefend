using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;
    
    [SerializeField]
    private List<Dice> diceList = new List<Dice>();
    [SerializeField]
    public List<DiceConfig> diceConfig = new List<DiceConfig>();
    private void Awake() 
    {
        Instance = this;    
    }
    public void Init()
    {
        diceList = GetComponentsInChildren<Dice>().ToList();
        foreach (var dice in diceList)
        {
            dice.Init();
        }
    }
}

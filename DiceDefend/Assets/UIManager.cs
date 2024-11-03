using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField]
    private TextMeshProUGUI UIGold, UIWave;
    private void Awake() 
    {
        Instance = this;    
    }

    public void Init()
    {
    
    }
    public void UpdateUIGold(float value)
    {
        UIGold.text = value.ToString();
    }
    public void UpdateUIWave(float value)
    {
        UIWave.text= "Wave " + value.ToString();
    }
}

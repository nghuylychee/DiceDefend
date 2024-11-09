using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using Orc1M;
using System.Numerics;

public class FXManager : MonoBehaviour
{
    [FoldoutGroup("FX Pool")]
    [SerializeField]
    private List<GainResourceEffectPlayer> fxGainResourcePool;

    public static FXManager Instance;
    private void Awake() 
    {
        Instance = this;
    }
    public void Init()
    {

    }
    public void PlayEffectGainResource(float value, UnityEngine.Vector3 startPos)
    {
        UnityEngine.Vector3 uiStartPos = Camera.main.WorldToScreenPoint(startPos);
        foreach (var fx in fxGainResourcePool)
        {
            if (!fx.isPlaying)
            {
                Debug.Log("zzz");
                fx.PlayEffect(uiStartPos, value, EnumConst.CurrencyType.Gold);
                return;
            }
        }
    }
}

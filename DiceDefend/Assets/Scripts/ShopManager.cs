using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.AI;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public event Action<float> OnRollComplete;
    [SerializeField]
    private Button buttonRoll;

    [SerializeField]
    private Transform UIShop;
    private Vector3 ShopOpenPos, ShopClosePos;
    [SerializeField]
    private float rollPrice;
    [SerializeField]
    private bool isShop, isShopScrolling;
    [SerializeField]
    private List<ShopItem> shopItemList = new List<ShopItem>();
    private void Awake() 
    {
        Instance = this;
    }

    public void Init()
    {
        isShop = true;
        ShopOpenPos = UIShop.position;
        ShopClosePos = new Vector3(UIShop.position.x, -UIShop.position.y, UIShop.position.z);

        RollShopItem();
    }
    public void ToggleShop() 
    {
        if (isShopScrolling) return;

        isShopScrolling = true;
        var targetPos = isShop ? ShopClosePos : ShopOpenPos;
        UIShop.DOMove(targetPos, 0.2f).OnComplete(() =>
        {
            isShop = !isShop;
            isShopScrolling = false;
        });
    }
    public void UpdateRollButton(float playerCurrentGold)
    {
        if (playerCurrentGold >= rollPrice)
        {
            buttonRoll.interactable = true;
        }
        else
        {
            buttonRoll.interactable = false;
        }
    }
    public void Roll()
    {
        RollShopItem();
        OnRollComplete?.Invoke(-rollPrice);
    }
    private void RollShopItem()
    {
        foreach (var item in shopItemList)
        {
            //Random a dice type in config -- TODO: Update mechanic roll
            var diceID = UnityEngine.Random.Range(0, DiceManager.Instance.diceConfig.Count);
            var diceTypeData = DiceManager.Instance.diceConfig[diceID];
            item.UpdateItem(diceID, diceTypeData.DiceName, diceTypeData.DiceDesc, diceTypeData.Price, diceTypeData.Icon);
        }
    }
}

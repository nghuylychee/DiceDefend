using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class King : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float currentHP, maxHP;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool isAlive;
    private void Start() 
    {
        Init();    
    }
    public void Init()
    {
        currentHP = maxHP;
        healthBar.UpdateHealthBar(currentHP / maxHP);
    }
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        healthBar.UpdateHealthBar(currentHP / maxHP);

        spriteRenderer.DOColor(Color.white, 0.05f).OnComplete(() => 
        {
            spriteRenderer.DOColor(Color.red, 0.05f).OnComplete(() => 
            {
                spriteRenderer.DOColor(Color.white, 0.05f);
            });
        });


        if (currentHP <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }
}

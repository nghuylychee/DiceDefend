using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private float maxHP, currentHP, moveSpeed, coinEarn;
    private Transform target;
    private bool isAlive = true;
    private Color initColor;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isAlive)
        {
            MoveToKing();
        }
    }
    public void Init()
    {
        target = GameObject.FindWithTag("King").transform;
        currentHP = maxHP;
        healthBar.value = 1;        
    }

    private void MoveToKing()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        healthBar.value = Mathf.Max(currentHP / maxHP, 0);

        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.DOColor(Color.white, 0.05f).OnComplete(() => 
        {
            rend.DOColor(Color.red, 0.05f).OnComplete(() => 
            {
                rend.DOColor(Color.white, 0.05f);
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
    
        //GameManager.Instance.AddMoney(rewardMoney);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.BulletDamage);
                Destroy(collision.gameObject); // Xóa đạn sau khi bắn trúng
            }
        }
    }
}

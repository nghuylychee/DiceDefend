using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float maxHP, currentHP, moveSpeed, coinEarn, damage, detectionRange, attackSpeed, attackRange;
    private Transform target, king;
    private bool isAlive, isAttacking;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (target == null)
            StopAttack();
        if (isAlive && !isAttacking)
        {
            FindTarget();
            MoveToTarget();
        }
    }
    public void Init()
    {
        isAlive = true; isAttacking = false;
        king = GameObject.FindWithTag("King").transform;
        currentHP = maxHP;
        healthBar.UpdateHealthBar(currentHP / maxHP);        
    }   

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        healthBar.UpdateHealthBar(currentHP / maxHP);

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

    private void FindTarget()
    {
        // Tìm Dice trong tầm phát hiện, giả định Dice có tag là "Dice"
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        Transform closestDice = null;
        float closestDistance = detectionRange;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Dice"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDice = hit.transform;
                }
            }
        }

        // Nếu có Dice trong tầm thì chọn Dice làm mục tiêu, ngược lại chọn King
        target = closestDice != null ? closestDice : king;
    }

    private void MoveToTarget()
    {
        if (target == null) return;
        
        // Di chuyển về phía mục tiêu
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // Kiểm tra nếu đã tới gần mục tiêu
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange)
        {
            StartCoroutine(AttackTarget());
        }
    }

    private IEnumerator AttackTarget()
    {
        isAttacking = true;

        // Tấn công mục tiêu
        while (isAttacking && target != null)
        {
            if (target.CompareTag("Dice"))
            {
                // Debug.Log("Damage " + target.name + " " + damage);
                target.GetComponent<Dice>().TakeDamage(damage);
            }
            else if (target.CompareTag("King"))
            {
                // Debug.Log("Damage " + target.name + " " + damage);
                king.GetComponent<King>().TakeDamage(damage);
            }
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    public void StopAttack()
    {
        isAttacking = false;
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

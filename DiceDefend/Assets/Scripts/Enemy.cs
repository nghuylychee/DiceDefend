using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float maxHP, currentHP, moveSpeed, coinEarn, damage, detectionRange, attackSpeed, attackRange, reward;
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
        EnemyManager.Instance.OnEnemyDie(reward, this.transform.position);
        Destroy(gameObject);
    }

    private void FindTarget()
    {
        Transform closestDice = null;
        float closestDistance = detectionRange;

        // Gửi Raycast từ enemy theo chiều ngang (giả sử tấn công hướng sang phải)
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.left, detectionRange);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Dice") || hit.collider.CompareTag("King"))
            {
                // Kiểm tra xem mục tiêu có trong cùng lane không (y gần bằng enemy y)
                if (Mathf.Abs(hit.collider.transform.position.y - transform.position.y) < 0.05f)
                {
                    float distance = Vector2.Distance(transform.position, hit.collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestDice = hit.collider.transform;
                    }
                }
            }
        }

        // Nếu tìm thấy Dice trong cùng lane thì target là Dice, nếu không thì target King
        target = closestDice != null ? closestDice : king;
    }


    private void MoveToTarget()
    {
        if (target == null) return;

        // Giữ nguyên vị trí y, chỉ di chuyển theo trục x về phía mục tiêu
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Kiểm tra nếu đã tới gần mục tiêu
        float distanceToTarget = Mathf.Abs(transform.position.x - target.position.x);
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

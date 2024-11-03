using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Dice : MonoBehaviour
{
    public Sprite[] diceSprites;
    [SerializeField]
    public Image cooldownImage;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private TextMeshProUGUI numberText;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float rollDuration, rollInterval, bulletSpeed, bulletDamage, currentHP, maxHP;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private bool isRolling, isDragging, isAlive;
    [SerializeField]
    private EnumConst.BulletDirection bulletDirection;
    void Start()
    {
        Init();
    }
    private void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isRolling = false;
        isDragging = false;
        isAlive = true;
        currentHP = maxHP;
        healthBar.UpdateHealthBar(currentHP / maxHP);
        StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        cooldownImage.fillAmount = 1;
        float elapsedTime = 0;
        while (elapsedTime <= rollInterval)
        {
            yield return new WaitForSeconds(0.05f);
            if (isDragging)
            {
                ResetDice();
                break;
            }
            cooldownImage.fillAmount -= 0.05f / rollInterval;
            elapsedTime += 0.05f;
        }
        RollDice();
    }

    public void ResetDice()
    {
        StopAllCoroutines();
        cooldownImage.fillAmount = 1;
        transform.eulerAngles = Vector3.zero;
        isRolling = false;
    }
    public void RollDice()
    {
        if (!isRolling && !isDragging)
        {
            StartCoroutine(RollDiceCoroutine());
        }
    }
    IEnumerator RollDiceCoroutine()
    {
        isRolling = true;
        float elapsedTime = 0f;

        while (elapsedTime < rollDuration)
        {
            if (isDragging)
            {
                break;
            }
            int spriteIndex = Random.Range(0, diceSprites.Length);
            spriteRenderer.sprite = diceSprites[spriteIndex];

            transform.DOShakeScale(0.1f, 0.1f, 10, 90, false, ShakeRandomnessMode.Harmonic).OnComplete(() =>
            {
                transform.DOScale(1.2f, 0.1f).OnComplete(() =>
                    transform.DOScale(1f, 0.1f));
            });

            transform.DOShakeRotation(.2f, new Vector3(0, 0, 5), 100, 90, false, ShakeRandomnessMode.Harmonic);
            yield return new WaitForSeconds(0.05f);
            elapsedTime += 0.05f;
        }
        int finalResult = Random.Range(0, diceSprites.Length);
        spriteRenderer.sprite = diceSprites[finalResult];
        transform.DORotate(Vector3.zero, 0.2f);

        // Hiển thị kết quả với hiệu ứng pop-up
        numberText.text = (finalResult + 1).ToString();
        numberText.gameObject.SetActive(true);
        numberText.transform.localScale = Vector3.zero;
        numberText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
            numberText.transform.DOScale(0f, 0.5f)
        );

        //Bắn theo số roll ra
        yield return StartCoroutine(Fire(finalResult + 1));

        //Đợi bắn xong mới reset và roll tiếp
        ResetDice();
        StartCoroutine(Roll());
    }
    IEnumerator Fire(int amount)
    {
        //Delay giữa các viên đạn nhìn cho dễ
        float bulletDelay = 0.5f;

        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(bulletDelay);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Fire(bulletDirection, bulletSpeed, bulletDamage);
        }
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
    private void OnMouseDrag()
    {
        isDragging = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;

        GridManager.Instance.TryPlaceDice(this, transform.position);
    }
    private void OnMouseUp()
    {
        isDragging = false;
        ResetDice();
        StartCoroutine(Roll());
        GridManager.Instance.PlaceDice(this);
    }
}

using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public Sprite[] diceSprites;
    public Image cooldownImage;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float rollDuration = 1.5f;
    [SerializeField]
    private float rollInterval = 2f; // Thời gian giữa các lần roll
    [SerializeField]
    private bool isRolling = false, isDragging = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        cooldownImage.fillAmount = 1;
        float elapsedTime = 0;
        while (elapsedTime < rollInterval)
        {
            if (isDragging)
            {
                ResetDice();
                break;
            }
            cooldownImage.fillAmount -= 0.05f / rollInterval;
            elapsedTime += 0.05f;
            yield return new WaitForSeconds(0.05f);
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
                transform.DOScale(1.1f, 0.1f).OnComplete(() =>
                    transform.DOScale(1f, 0.1f));
            });

            transform.DOShakeRotation(.2f, new Vector3(0, 0, 5), 100, 90, false, ShakeRandomnessMode.Harmonic);
            yield return new WaitForSeconds(0.05f);
            elapsedTime += 0.05f;
        }
        int finalResult = Random.Range(0, diceSprites.Length);
        spriteRenderer.sprite = diceSprites[finalResult];
        transform.DORotate(Vector3.zero, 0.2f);

        //Delay cho nhìn kết quả + attack
        yield return new WaitForSeconds(2f);
        ResetDice();
        StartCoroutine(Roll());
    }

    void OnMouseDrag()
    {
        isDragging = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;

        GridManager.Instance.TryPlaceDice(this, transform.position);
    }
    void OnMouseUp()
    {
        isDragging = false;
        ResetDice();
        StartCoroutine(Roll());
        GridManager.Instance.PlaceDice(this);
    }
}

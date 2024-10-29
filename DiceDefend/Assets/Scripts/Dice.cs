using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Dice : MonoBehaviour
{
    public Sprite[] diceSprites;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float rollDuration = 1.5f;
    [SerializeField]
    private bool isRolling = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void RollDice()
    {
        if (!isRolling)
        {
            StartCoroutine(RollDiceCoroutine());
        }
    }

    IEnumerator RollDiceCoroutine()
    {
        isRolling = true;
        float elapsedTime = 0f;
        int spriteIndex;

        while (elapsedTime < rollDuration)
        {
            spriteIndex = Random.Range(0, diceSprites.Length);
            spriteRenderer.sprite = diceSprites[spriteIndex];

            // Lắc lư qua lại và phóng to/thu nhỏ
            transform.DOShakeScale(0.1f, 0.1f, 10, 90, false, ShakeRandomnessMode.Harmonic).OnComplete(() =>
            {
                transform.DOScale(1.1f, 0.1f).OnComplete(() =>
                    transform.DOScale(1f, 0.1f));
            });

             // Lắc nghiêng cả trái và phải
            transform.DOShakeRotation(.2f, new Vector3(0, 0, 5), 100, 90, false, ShakeRandomnessMode.Harmonic); // Lắc nghiêng hai bên

            yield return new WaitForSeconds(0.05f);
            elapsedTime += 0.05f;
        }

        int finalResult = Random.Range(0, diceSprites.Length);
        spriteRenderer.sprite = diceSprites[finalResult];
        transform.DORotate(Vector3.zero, 0.2f);
        isRolling = false;
    }

    public void OnMouseDown()
    {
        Debug.Log("roll");
        RollDice();
    }
}

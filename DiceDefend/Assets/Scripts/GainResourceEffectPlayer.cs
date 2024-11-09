using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetKits.ParticleImage;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using DG.Tweening;

namespace Orc1M
{

    public class GainResourceEffectPlayer : MonoBehaviour
    {
        public ParticleImage gainResourceFX;

        [FoldoutGroup("Currency Sprites")][SerializeField] private Texture goldSprite;  
        [FoldoutGroup("Destination")][SerializeField] private Transform goldTargetPos;

        public bool isPlaying => gainResourceFX.isPlaying;

        public void PlayEffect(Vector3 startPos, float amount,
            EnumConst.CurrencyType currencyType = EnumConst.CurrencyType.Gold,
            System.Action OnParticleReachTarget = null)
        {
            gainResourceFX.transform.position = startPos;
            switch (currencyType)
            {
                case EnumConst.CurrencyType.Gold:
                    gainResourceFX.texture = goldSprite;
                    gainResourceFX.attractorTarget = goldTargetPos;
                    break;
                default:
                    break;
            }
            gainResourceFX.onFirstParticleFinish.RemoveAllListeners();
            gainResourceFX.onFirstParticleFinish.AddListener(() => OnParticleReachTarget?.Invoke());
            gainResourceFX.Play();

            //moneyText.transform.localScale = Vector2.zero;
            //moneyText.transform.localPosition = textStartPos;
            //moneyText.text = $"+{amount}";
            //moneyText.transform.DOScale(Vector2.one, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
            //moneyText.transform.DOLocalMoveY(textStartPos.y + 150, 1f).SetUpdate(true);
            //Sequence moneyTextAnim = DOTween.Sequence();
            //moneyTextAnim.AppendInterval(1.8f);
            //moneyTextAnim.Append(moneyText.transform.DOScale(Vector2.zero, 0.1f));
            //moneyTextAnim.SetUpdate(true);

            // Backup...
        //    DOVirtual.DelayedCall(2.5f, () =>
        //    {
        //        if (moneyText.transform.localScale.x > 0)
        //            moneyText.transform.DOScale(Vector2.zero, 0.1f);
        //    }, true);
        //}

    }
}
}

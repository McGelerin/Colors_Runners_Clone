using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;



namespace Controllers
{
    public class Text2xController: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI x2TMP;
        public void Show2XText()
        {
            x2TMP.GetComponent<TextMeshProUGUI>().DOFade(1f, 0.1f);

            x2TMP.DOScale(2.6f, 0.5f).SetEase(Ease.InOutBack);
            StartCoroutine(FadeOut());
            
        }

        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(0.5f);
            x2TMP.GetComponent<TextMeshProUGUI>().DOFade(0f, 1f);

        }
    }
}
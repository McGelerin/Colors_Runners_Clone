using System;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using CoreGameSignals = Signals.CoreGameSignals;

namespace Managers
{
    public class MiniGameManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject pointerImage;
        [SerializeField] private TextMeshProUGUI bonusScore;

        #endregion

        #region Private Variables

        private int _claimFactor;
        private int? _stackScore;

        #endregion

        #endregion

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            ScoreSignals.Instance.onGetScore += OnGetScore;
        }

        private void UnsubscribeEvents()
        {
            ScoreSignals.Instance.onGetScore -= OnGetScore;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        private void Update()
        {
            PlayPointerAnim();
        }

        private void PlayPointerAnim()
        {
            pointerImage.transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * 30, 30f)-15f);
            pointerImage.transform.localPosition = new Vector3(Mathf.PingPong(Time.time * 660, 660) - 330,
                Mathf.PingPong(Time.time * 74, 37) - 67f,0);
            SetClaimFactor();
            SetBonusScore();
        }

        private void SetClaimFactor()
        {
            var pointerEulerAnglesZ =  pointerImage.transform.eulerAngles.z;
            if (pointerEulerAnglesZ < 15)
            {
                if (pointerEulerAnglesZ > 10 )
                {
                    _claimFactor = 2;
                }else if (pointerEulerAnglesZ < 3)
                {
                    _claimFactor = 5;
                }
                else
                {
                    _claimFactor = 3;
                }
            }
            else
            {
                if (pointerEulerAnglesZ <350 )
                {
                    _claimFactor = 2;
                }else if (pointerEulerAnglesZ > 357)
                {
                    _claimFactor = 5;
                }
                else
                {
                    _claimFactor = 3;
                }
            }
        }

        private void SetBonusScore()
        {
            bonusScore.text = (_stackScore * _claimFactor).ToString();
        }

        private void OnGetScore(int score)
        {
            _stackScore = score;
        }
        
        
    }
}
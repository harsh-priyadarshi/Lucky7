using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Fordi.Core
{
    public enum Expression
    {
        NORMAL,
        QUESTION,
        SMILE,
        BLINK,
        CRY,
        SHOCK
    }

    public class HostView : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_questionExpression, m_smileExpression, m_shockExpression, m_blinkExpression, m_cryExpression;

        [SerializeField]
        private GameObject m_glass;

        [SerializeField]
        private Dice m_dice1, m_dice2;

        [SerializeField]
        private Transform m_dice1InitialAnchor, m_dice2InitialAnchor, m_dice1OnZoomAnchor, m_dice2OnZoomAnchor;

        private GameObject m_currentExpression;

        private void Awake()
        {
            m_currentExpression = null;
        }

        private IEnumerator m_coHostReset = null;

        private IEnumerator m_diceRollEnumerator = null;

        private IEnumerator m_diceCollectEnumerator = null;

        public void Express(Expression expression)
        {
            if (m_coHostReset != null)
                StopCoroutine(m_coHostReset);

            if (m_currentExpression != null)
                m_currentExpression.SetActive(false);

            switch (expression)
            {
                case Expression.NORMAL:
                    m_currentExpression = null;
                    break;
                case Expression.QUESTION:
                    m_currentExpression = m_questionExpression;
                    break;
                case Expression.SMILE:
                    m_currentExpression = m_smileExpression;
                    break;
                case Expression.BLINK:
                    m_currentExpression = m_blinkExpression;
                    break;
                case Expression.CRY:
                    m_currentExpression = m_cryExpression;
                    break;
                case Expression.SHOCK:
                    m_currentExpression = m_shockExpression;
                    break;
                default:
                    break;
            }

            if (m_currentExpression != null)
                m_currentExpression.SetActive(true);

            m_coHostReset = CoSetNormalExpression();
            StartCoroutine(m_coHostReset);
        }

        IEnumerator CoSetNormalExpression()
        {
            yield return new WaitForSeconds(Random.Range(2, 4));
            Express(Expression.NORMAL);
        }

        public void RollDice(UnityAction<int> done)
        {
            if (m_diceRollEnumerator != null)
                StopCoroutine(m_diceRollEnumerator);
            m_diceRollEnumerator = CoDiceRoll(done);
            m_glass.SetActive(true);
            StartCoroutine(m_diceRollEnumerator);
        }

        IEnumerator CoDiceRoll(UnityAction<int> done)
        {
            yield return new WaitForSeconds(3);
            m_glass.SetActive(false);
            int dice1Value = Random.Range(1, 7);
            int dice2Value = Random.Range(1, 7);

            m_dice1.SetValue(dice1Value);
            m_dice2.SetValue(dice2Value);
            m_dice1.gameObject.SetActive(true);
            m_dice2.gameObject.SetActive(true);

            m_dice1.transform.DOScale(m_dice1OnZoomAnchor.localScale, 2.0f);
            m_dice2.transform.DOScale(m_dice2OnZoomAnchor.localScale, 2.0f);
            m_dice1.transform.DOMove(m_dice1OnZoomAnchor.position, 2.0f);
            m_dice2.transform.DOMove(m_dice2OnZoomAnchor.position, 2.0f);

            yield return new WaitForSeconds(2.0f);

            done?.Invoke(dice1Value + dice2Value);
        }

        public void CollectDice(Action done)
        {
            if (m_diceCollectEnumerator != null)
                StopCoroutine(m_diceCollectEnumerator);
            m_diceCollectEnumerator = CoDiceCollect(done);
            StartCoroutine(m_diceCollectEnumerator);
        }

        IEnumerator CoDiceCollect(Action done)
        {
            yield return new WaitForSeconds(2);

            m_dice1.transform.DOScale(m_dice1InitialAnchor.localScale, 2.0f);
            m_dice2.transform.DOScale(m_dice2InitialAnchor.localScale, 2.0f);
            m_dice1.transform.DOMove(m_dice1InitialAnchor.position, 2.0f);
            m_dice2.transform.DOMove(m_dice2InitialAnchor.position, 2.0f);

            yield return new WaitForSeconds(2.0f);

            m_dice1.gameObject.SetActive(false);
            m_dice2.gameObject.SetActive(false);

            done?.Invoke();
        }
    }
}
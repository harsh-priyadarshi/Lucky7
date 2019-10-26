using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private GameObject m_currentExpression;

        private void Awake()
        {
            m_currentExpression = null;
        }

        public void Express(Expression expression)
        {
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
        }
    }
}
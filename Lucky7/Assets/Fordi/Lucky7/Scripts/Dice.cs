using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Core
{
    public class Dice : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] m_diceFaces;

        SpriteRenderer m_spriteRenderer;

        public void SetValue(int value)
        {
            if (m_spriteRenderer == null)
                m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_spriteRenderer.sprite = m_diceFaces[value - 1];
        }
    }
}
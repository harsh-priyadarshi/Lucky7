using Fordi.Common;
using Fordi.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Fordi.UI
{
    public class Timer : MonoBehaviour
    {
        private IEnumerator m_coTimer;

        [SerializeField]
        private TextMeshProUGUI m_valueText;
        [SerializeField]
        private Transform m_circle;

        private void Awake()
        {

        }

      //  private void Update()
      //  {
		    //m_circle.Rotate(15f*Vector3.forward*Time.deltaTime);
            
      //  }

        public void StartTimer(int time)
        {
            m_valueText.text = time.ToString();
            if (m_coTimer != null)
                StopCoroutine(m_coTimer);

            m_coTimer = CoRound(time);
            StartCoroutine(m_coTimer);
        }

        private IEnumerator CoRound(int time)
        {
            while (time > 0)
            {
                yield return new WaitForSeconds(1);
                m_circle.Rotate(-15f * Vector3.forward);
                time--;
                if (time == 0)
                    m_valueText.text = "";
                else
                    m_valueText.text = time.ToString();
            }
        }
    }
}
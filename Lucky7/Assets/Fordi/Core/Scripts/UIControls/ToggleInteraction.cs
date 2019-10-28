using Fordi.Common;
using Fordi.Core;
using Papae.UnitySDK.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fordi.UI
{
    [DisallowMultipleComponent]
    public class ToggleInteraction : Interaction
    {
        protected override void AwakeOverride()
        {
            base.AwakeOverride();
            ((Toggle)m_selectable).onValueChanged.AddListener((val) =>
            {
                var clip = AudioManager.Instance.GetClipFromPlaylist(ClickClip);
                AudioManager.Instance.PlayOneShot(clip, Vector3.zero, .2f);
            });
        }
    }
}
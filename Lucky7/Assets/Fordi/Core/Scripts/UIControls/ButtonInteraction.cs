﻿using Fordi.Common;
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
    public class ButtonInteraction : Interaction
    {
        protected override void AwakeOverride()
        {
            base.AwakeOverride();
            ((Button)m_selectable).onClick.AddListener(() =>
            {
                var clip = AudioManager.Instance.GetClipFromPlaylist(ClickClip);
                AudioManager.Instance.PlayOneShot(clip, Vector3.zero, .2f);
            });
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Core
{
    public interface IMenuSelection
    {
        Color Color { get; set; }
        string Location { get; set; }
        GameObject Mandala { get; set; }
        AudioClip Music { get; set; }
        AudioClip VoiceOver { get; set; }
        Playground  Playground { get; set; }
    }

    /// <summary>
    /// This is preserved between scenes.
    /// </summary>
    public class MenuSelection : IMenuSelection
    {
        public Color Color { get; set; }
        public string Location { get; set; }
        public GameObject Mandala { get; set; }
        public AudioClip Music { get; set; }
        public AudioClip VoiceOver { get; set; }
        public Playground Playground { get; set; } = Playground.HOME;
    }
}

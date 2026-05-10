using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SCP999XPlugin
{
    public class Config : IConfig
    {
        [Description("Plugin etkinleştirilsin mi?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug modunu aç")]
        public bool Debug { get; set; } = false;

        [Description("Görünmezlik süresi (saniye)")]
        public float InvisibilityDuration { get; set; } = 300f;

        [Description("Kamera offset (ayakların altında kaç birim)")]
        public float CameraOffsetY { get; set; } = -1.8f;

        [Description("Oyuncu ölümsüz mü olsun?")]
        public bool IsImmortal { get; set; } = true;

        [Description("Spawn odası (GR-18)")]
        public string SpawnRoom { get; set; } = "LczAirlock";

        public Exiled.API.Interfaces.ITranslation Translation { get; set; }
    }
}

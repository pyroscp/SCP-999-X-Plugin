using Exiled.API.Features;
using Exiled.API.Enums;
using CustomPlayerEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCP999XPlugin
{
    public class SCP999XPlugin : Plugin<Config>
    {
        public static SCP999XPlugin Instance { get; private set; }
        
        private Dictionary<Player, Vector3> cameraOffsets = new Dictionary<Player, Vector3>();

        public override void OnEnable()
        {
            base.OnEnable();
            Instance = this;
            
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Exiled.Events.Handlers.Player.Died += OnPlayerDied;
            
            Log.Info("SCP-999-X Plugin etkinleştirildi!");
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
            
            Log.Info("SCP-999-X Plugin devre dışı bırakıldı!");
        }

        public void TransformPlayerToSCP999X(Player player)
        {
            try
            {
                // D-Class rolüne dönüştür
                player.Role = RoleType.ClassD;
                
                // Görünmezlik ekle
                player.EnableEffect(EffectType.Invisibility, Config.InvisibilityDuration);
                
                // Kapı erişimi için gerekli keycard'ları ver
                GiveKeycardsForDoorAccess(player);
                
                // Kamera konumunu ayakların altına ayarla
                SetCameraPositionBelowFeet(player);
                
                // Ölümsüzlük kodu
                player.Health = 9999f;
                
                // Broadcast mesajı
                player.ShowBroadcast($"Sen SCP-999-X'e dönüştürüldün!", 10);
                
                // GR-18'de spawn olması için
                player.Position = RoomType.LczAirlock.GetRandomRoom().Position + Vector3.up * 1.5f;
                
                Log.Info($"{player.Nickname} SCP-999-X'e dönüştürüldü!");
            }
            catch (Exception e)
            {
                Log.Error($"Dönüştürme hatası: {e.Message}");
            }
        }

        private void SetCameraPositionBelowFeet(Player player)
        {
            // Kamera konumunu ayakların altına ayarla
            Vector3 offset = new Vector3(0, -1.8f, 0); // Ayakların 1.8 birim altı
            cameraOffsets[player] = offset;
        }

        private void GiveKeycardsForDoorAccess(Player player)
        {
            // Tüm kapılara erişim için keycard ver
            player.AddItem(ItemType.KeycardJanitor);
            player.AddItem(ItemType.KeycardScientist);
            player.AddItem(ItemType.KeycardSeniorGuard);
            player.AddItem(ItemType.KeycardContainmentEngineer);
        }

        private void OnRoundStarted()
        {
            cameraOffsets.Clear();
            Log.Info("Yeni round başladı, kamera offsetleri sıfırlandı.");
        }

        private void OnPlayerLeft(LeftEventArgs ev)
        {
            if (cameraOffsets.ContainsKey(ev.Player))
            {
                cameraOffsets.Remove(ev.Player);
            }
        }

        private void OnPlayerDied(DiedEventArgs ev)
        {
            if (cameraOffsets.ContainsKey(ev.Target))
            {
                cameraOffsets.Remove(ev.Target);
            }
        }

        public Vector3 GetCameraOffset(Player player)
        {
            return cameraOffsets.ContainsKey(player) ? cameraOffsets[player] : Vector3.zero;
        }
    }
}

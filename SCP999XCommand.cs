using CommandSystem;
using Exiled.API.Features;
using System;

namespace SCP999XPlugin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SCP999XCommand : ICommand
    {
        public string Command => "scp-999-x";
        public string[] Aliases => new string[] { "scp999x", "blob" };
        public string Description => "Bir oyuncuyu SCP-999-X karakterine dönüştürür";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // Admin kontrolü
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "Bu komut sadece oyuncu tarafından kullanılabilir!";
                return false;
            }

            Player admin = Player.Get(playerSender);
            if (admin == null || !admin.RemoteAdminAccess)
            {
                response = "Yetkiniz yok!";
                return false;
            }

            // Argüman kontrolü
            if (arguments.Count < 1)
            {
                response = "Kullanım: scp-999-x <oyuncu_adı>";
                return false;
            }

            // Oyuncu bul
            string playerName = arguments.At(0);
            Player target = Player.Get(playerName);

            if (target == null)
            {
                response = $"Oyuncu bulunamadı: {playerName}";
                return false;
            }

            // Dönüştür
            try
            {
                SCP999XPlugin.Instance.TransformPlayerToSCP999X(target);
                response = $"{target.Nickname} SCP-999-X'e dönüştürüldü!";
                return true;
            }
            catch (Exception ex)
            {
                response = $"Hata: {ex.Message}";
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace QuickTeleport
{
    class Program
    {
        private static Menu Menu;
        private static SpellDataInst Teleport;
        private static Obj_AI_Hero Player;

        static void Main(string[] args)
        {
            Player = ObjectManager.Player;
            Teleport = GetTeleport();
            if (Teleport == null)
                return;

            Menu = new Menu("QuickTeleport", "QuickTeleport", true);
            Menu.AddItem(new MenuItem("Hotkey", "Hotkey").SetValue(new KeyBind(16, KeyBindType.Press, false)));
            Menu.AddToMainMenu();
            Game.PrintChat("QuickTeleport by Trees loaded.");
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (!CanTeleport() || !Menu.Item("Hotkey").GetValue<bool>())
                return;
            
            Obj_AI_Base ClosestObject = Player;
            float d = 3000;

            foreach(Obj_AI_Base obj in ObjectManager.Get<Obj_AI_Base>().Where(obj => obj != null && obj.IsValid && obj.IsVisible && !obj.IsDead && obj.Team == Player.Team && Vector3.Distance(obj.ServerPosition, Game.CursorPos) < d)){
                ClosestObject = obj;
                d = Vector3.Distance(obj.ServerPosition, Game.CursorPos);
            }

            if(ClosestObject != Player && ClosestObject != null)
                CastTeleport(ClosestObject);
        }

        private static SpellDataInst GetTeleport()
        {
            var spells = Player.SummonerSpellbook.Spells;
            return spells.FirstOrDefault(spell => spell.Name == "SummonerTeleport");
        }

        private static bool CanTeleport()
        {
            if (Teleport != null && Teleport.Slot != SpellSlot.Unknown && Teleport.State == SpellState.Ready &&
               Player.CanCast)
                return true;
            return false;
        }

        private static void CastTeleport(Obj_AI_Base unit)
        {
            if (CanTeleport())
                Player.SummonerSpellbook.CastSpell(Teleport.Slot, unit);
        }

    }
}
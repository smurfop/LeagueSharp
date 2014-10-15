#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace SpellHumanizer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Game.PrintChat("SpellHumanizer Loaded!");
            Game.OnGameSendPacket += Game_OnGameSendPacket;
        }

        private static void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] != Packet.C2S.Cast.Header || IsSummonerSpell(args.PacketData[5]))
            {
                return;
            }
            
            //if(IsSummonerSpell(args.PacketData[5]) == true) {
            //    Game.PrintChat("Detected use of Summoner Spell!");
            //}

            var spellState = ObjectManager.Player.Spellbook.CanUseSpell((SpellSlot) args.PacketData[6]);

            if (ObjectManager.Player.IsDead || spellState == SpellState.Cooldown || spellState == SpellState.NoMana ||
                spellState == SpellState.NotLearned || spellState == SpellState.Surpressed ||
                spellState == SpellState.Unknown)
            {
                args.Process = false;
            }
        }
        
        private static bool IsSummonerSpell(byte spellByte)
        {
            if(spellByte == 0xE9) return true;
            if(spellByte == 0xEF) return true;
            if(spellByte == 0x8B) return true;
            if(spellByte == 0xED) return true;
            if(spellByte == 0x63) return true;
            return false;
        }
        
    }
}

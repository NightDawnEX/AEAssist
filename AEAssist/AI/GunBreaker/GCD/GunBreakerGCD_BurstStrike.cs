﻿using System.Threading.Tasks;
using AEAssist.Define;
using ff14bot;
using AEAssist.Helper;
using ff14bot.Managers;

namespace AEAssist.AI.GunBreaker.GCD
{
    public class GunBreakerGCD_BurstStrike : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Gunbreaker.Cartridge == 0)
                return -5;

            //子弹将要溢出
            if (ActionManager.LastSpell.Id == SpellsDefine.BrutalShell)
            {
                if (Core.Me.ClassLevel < 88 && ActionResourceManager.Gunbreaker.Cartridge == 2)
                    return 1;
                else if (ActionResourceManager.Gunbreaker.Cartridge ==3)
                    return 2;
            }

            //在无情中 泻子弹
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.NoMercy))
            {
                if (Core.Me.ClassLevel < 90)
                    return 5;

                //90级技能冷却时间<无情时间
                else if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.NoMercy, (int)SpellsDefine.DoubleDown.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds))
                    return -51;
                else return 6;
            }


            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.BurstStrike.GetSpellEntity();
            if (await spell.DoGCD())
                return spell;
            return null;
        }
    }
}
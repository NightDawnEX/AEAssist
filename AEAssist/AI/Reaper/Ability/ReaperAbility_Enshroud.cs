﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI.Reaper
{
    public class ReaperAbility_Enshroud : IAIHandler
    {
        public int Check(SpellData lastSpell)
        {
            if (!SpellsDefine.Enshroud.IsReady())
                return -1;
            return ReaperSpellHelper.ReadyToEnshroud();
        }

        public async Task<SpellData> Run()
        {
            if (await SpellHelper.CastAbility(SpellsDefine.Enshroud, Core.Me)) return SpellsDefine.Enshroud;

            return null;
        }
    }
}
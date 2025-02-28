﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;

namespace AEAssist.AI.Machinist.GCD
{
    public class MCHGCD_ChainSaw : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.ChainSaw.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await SpellsDefine.ChainSaw.DoGCD()) return SpellsDefine.ChainSaw.GetSpellEntity();

            return null;
        }
    }
}
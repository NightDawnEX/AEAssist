﻿using System.Threading.Tasks;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI
{
    public class BardAbility_UsePotion : IAIHandler
    {
        public int Check(SpellData lastSpell)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return -1;
            if (AIRoot.Instance.BurstOff)
                return -2;
            if (AIRoot.Instance.BattleData.maxAbilityTimes <
                SettingMgr.GetSetting<GeneralSettings>().MaxAbilityTimsInGCD)
                return -3;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -4;
            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<BardSettings>().UsePotionId))
                return -5;

            return 0;
        }

        public async Task<SpellData> Run()
        {
            LogHelper.Debug("===>Try using Potion   1111");
            var ret = await PotionHelper.UsePotion(SettingMgr.GetSetting<BardSettings>().UsePotionId);
            if (ret) AIRoot.Instance.MuteAbilityTime();

            await Task.CompletedTask;
            return null;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AEAssist;

namespace AEAssist.Helper
{
    public static class LanguageHelper
    {
        public static Dictionary<string, Language> AllLans = new Dictionary<string, Language>();

        public static List<LanguageOptionData> LanOptions { get; set; } = new List<LanguageOptionData>();

        public static void Init()
        {
            var currVersion = Language.Instance.LanVersion;
            var dir = $@"{Entry.Path}\Resources\";

            var path = $"{dir}Lan_{Language.Instance.LanType}.json";
            File.WriteAllText(path, MongoHelper.ToJson(Language.Instance));

            var files = Directory.GetFiles(dir, "Lan_.*json");

            // load from file
            files = Directory.GetFiles(dir, "Lan_*.json");
            AllLans.Clear();

            foreach (var v in files)
                try
                {
                    var lan = MongoHelper.FromJson<Language>(File.ReadAllText(v));
                    AllLans.Add(lan.LanType, lan);
                }
                catch (Exception e)
                {
                    LogHelper.Error("Loading LanguageConfig failed: \n" + e);
                }


            var currCulture = CultureInfo.CurrentCulture;
            if (AllLans.TryGetValue(currCulture.Name, out var targetLan)) SwitchLan(targetLan);
            else
            {
                SwitchLan(AllLans["en"]);
            }

            LanOptions.Clear();
            foreach (var v in AllLans)
            {
                LogHelper.Info("Find Language: " + v.Key);
                LanOptions.Add(new LanguageOptionData
                {
                    Key = v.Key,
                    Name = v.Key
                });
            }
        }

        public static void SwitchLan(string lanType)
        {
            //  LogHelper.Debug("Change lanType===>" + lanType);
            if (!AllLans.TryGetValue(lanType, out var language))
                return;
            SwitchLan(language);
        }

        public static void SwitchLan(Language target)
        {
            if (target.LanType == Language.Instance.LanType)
                return;
            var type = typeof(Language);
            var propertys = type.GetProperties();
            foreach (var v in propertys)
            {
                if (v.PropertyType != typeof(string))
                    continue;
                var va = v.GetValue(target).ToString();
                if (string.IsNullOrEmpty(va))
                    continue;
                v.SetValue(Language.Instance, va);
            }

            var fields = type.GetFields();
            foreach (var v in fields)
            {
                if (v.FieldType != typeof(string))
                    continue;
                var va = v.GetValue(target).ToString();
                if (string.IsNullOrEmpty(va))
                    continue;
                v.SetValue(Language.Instance, va);
            }
            
            SettingMgr.GetSetting<HotkeySetting>().ResetHotkeyName();
            LogHelper.Debug($"Change Language==>{target.LanType} finished");
        }
    }
}
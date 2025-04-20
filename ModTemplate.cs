using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SevenDayKillModDevelopmentTemplate
{
    public class ModTemplate : IModApi
    {
        //创建Mod文件夹路径
        public static DirectoryInfo Mod = new DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        public static string ConfigPath = $"{Mod.FullName}/config.json";
        public static string KeyPath = $"{Mod.FullName}/key.txt";
        public static SaveConfig saveConfig = null;
        private KeyCode k1;

        void IModApi.InitMod(Mod _modInstance)
        {

            ConfigPath.TryCreateAndWriteFile(Tool.ToJson(new SaveConfig()));
            KeyPath.TryCreateAndWriteFile(string.Join(Environment.NewLine, Enum.GetNames(typeof(KeyCode))
                            .Zip(Enum.GetValues(typeof(KeyCode)).Cast<int>(), (name, value) => $"{name}:{value}")));
            //读取保存的数据
            saveConfig = Tool.JsonToObject<SaveConfig>(File.ReadAllText($"{Mod.FullName}/config.json"));
            // 初始化Harmony实例
            var harmony = new Harmony(Path.GetRandomFileName());
            //应用
            //前置
            //harmony.Patch(typeof().GetMethod(""),
            //    prefix: new HarmonyMethod(typeof(PatchType), nameof(PatchType)));
            ////后置
            //harmony.Patch(typeof().GetMethod(""),
            //   postfix: new HarmonyMethod(typeof(PatchType), nameof(PatchType)));


            ModEvents.GameUpdate.RegisterHandler(new Action(this.OnGameUpdate));

            k1 = saveConfig.config5.Find(d => d.Key == $"{KeyCode.Mouse2}").Value;
        }

        private void OnGameUpdate()
        {
            if (Input.GetKeyDown(k1))
            {

            }
        }

    }
}

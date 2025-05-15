using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//提醒:选中变量或函数名或类名再按F2可以批量重命名整个工程涉及的地方.   给VS新手看的

//如果你发现VS提示你缺少引用,但是确实存在那个引用时,请删除引用,然后重新添加引用.
namespace SevenDayKillModDevelopmentTemplate
{
    /// <summary>
    /// 该类是Mod的主类,必须继承IModApi接口
    /// </summary>
    public class ModTemplate : IModApi
    {
        /// <summary>
        /// 该Mod所在的文件夹的路径
        /// </summary>
        public static DirectoryInfo Mod = new DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        /// <summary>
        /// 配置文件路径,用于存放Mod配置
        /// </summary>
        public static string ConfigPath = $"{Mod.FullName}/config.json";
        /// <summary>
        /// 键位参考文件,用于存放键位参考,给用户查看键位的.
        /// </summary>
        public static string KeyPath = $"{Mod.FullName}/key.txt";
        /// <summary>
        /// 保存配置的对象
        /// </summary>
        public static SaveConfig saveConfig = null;
        /// <summary>
        /// 临时按键
        /// </summary>
        private KeyCode k1;

        /// <summary>
        /// Mod初始化函数,在游戏加载时调用,该函数只会被调用一次,必须要有该函数,且函数名以及函数签名不能更改
        /// </summary>
        /// <param name="_modInstance"></param>
        void IModApi.InitMod(Mod _modInstance)
        {
            //尝试创建配置文件
            ConfigPath.TryCreateAndWriteFile(Tool.ToJson(new SaveConfig()));
            //尝试创建键位参考文件
            KeyPath.TryCreateAndWriteFile(string.Join(Environment.NewLine, Enum.GetNames(typeof(KeyCode))
                            .Zip(Enum.GetValues(typeof(KeyCode)).Cast<int>(), (name, value) => $"{name}:{value}")));
            //读取保存的数据
            saveConfig = Tool.JsonToObject<SaveConfig>(File.ReadAllText($"{Mod.FullName}/config.json"));

            // 初始化Harmony实例 这个对象的功能是把你编写的函数粘到目标函数上,当然覆盖目标函数也可以
            //参数是Harmony的编号,可以随便写,但是不能重复,如果你有多个补丁,可以把它们放在同一个Harmony实例上
            var harmony = new Harmony(Path.GetRandomFileName());

            //应用补丁
            //前置    原函数执行前就执行,如果你的函数返回了false,则不执行原函数,也就是覆盖
            //harmony.Patch(typeof(目标函数所在的类).GetMethod("目标函数"),
            //    prefix: new HarmonyMethod(typeof(PatchType), nameof(PatchType)));
            //说明例子
            //harmony.Patch(typeof(目标函数所在的类).GetMethod("目标函数"),
            //    prefix: new HarmonyMethod(typeof(你的函数所在的类), nameof(你的函数的函数名)));

            ////后置  原函数执行后才执行
            //harmony.Patch(typeof(目标函数所在的类).GetMethod("目标函数"),
            //   postfix: new HarmonyMethod(typeof(PatchType), nameof(PatchType)));
            //说明例子,同上

            //游戏内的UI窗口
            //GameManager.Instance.m_GUIConsole.windowManager.playerUI.xui

            //把你编写的函数注册到游戏更新事件上,每次游戏轮询都会调用这个函数
            ModEvents.GameUpdate.RegisterHandler(叫什么名称都可以);

            //从配置文件中读取目标按键
            k1 = saveConfig.config5.Find(d => d.Key == $"{KeyCode.Mouse2}").Value;
        }

        /// <summary>
        /// 这个函数会一直执行,所以如果有些功能只需要偶尔执行需要添加执行条件.
        /// 不要随便往这里面加东西,非常容易让游戏卡顿.
        /// </summary>
        private void 叫什么名称都可以()
        {
            //判断是否按下该按键,按下时会一直执行
            if (Input.GetKeyDown(k1))
            {
                //在控制台输出文本  这个只是例子,可以删除
                "Hello World 111".Log();
                //逻辑
            }

            //按下时执行,只执行1次
            if (Input.GetKey(k1))
            {
                //在控制台输出文本  这个只是例子,可以删除
                "Hello World 222".Log();
                //逻辑
            }

            //逻辑...
        }

    }
}

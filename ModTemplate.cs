using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static ModEvents;

//编写好代码之后点击生成->重新生成解决方案,然后再打开项目文件所在的文件夹,

//通常在 SevenDayKillModDevelopmentTemplate\bin\Debug 文件夹下可以找到 SevenDayKillModDevelopmentTemplate.dll 把文件复制到一个新建文件夹里,

//然后..去看Github里面这个项目的说明吧,跟着说明来操作就行了.   给VS新手看的

//项目链接:https://github.com/qq7995516/SevenDayKillModDevelopmentTemplate

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
        /// 临时按键
        /// </summary>
        private KeyCode F3;

        /// <summary>
        /// 用于存放自定义窗口的对象,如果不需要自定义窗口可以删除这个变量和相关代码
        /// </summary>
        private Tool.MyCustomWindow myPopupWindow;
        /// <summary>
        /// 显示自定义窗口的按键,如果不需要自定义窗口可以删除这个变量和相关代码
        /// </summary>
        public KeyCode toggleWindowKey = default; // 定义切换窗口的按键，可以按需修改
        /// <summary>
        /// 窗口标题
        /// </summary>
        public string title = "自定义窗口";

        /// <summary>
        /// Mod初始化函数,在游戏加载时调用,该函数只会被调用一次,必须要有该函数,且函数名以及函数签名不能更改
        /// </summary>
        /// <param name="_modInstance"></param>
        void IModApi.InitMod(Mod _modInstance)
        {
            //尝试创建配置文件
            ConfigPath.TryCreateAndWriteFile(Tool.ToJson(new SaveConfig()));
            //尝试创建键位参考文件
            KeyPath.TryCreateAndWriteFile(
                string.Join(
                    Environment.NewLine,
                        Enum.GetNames(typeof(KeyCode)).Zip(
                                Enum.GetValues(typeof(KeyCode)).Cast<int>(),
                            (name, value) => $"{name}:{value}"
                        )
                    )
                );

            //读取保存的数据
            saveConfig = Tool.JsonToObject<SaveConfig>(File.ReadAllText($"{Mod.FullName}/config.json"));
            //尝试添加按键
            saveConfig.config5.TryAddItem(new MyKV<string, KeyCode>($"{KeyCode.Mouse2}", KeyCode.Mouse2));
            //尝试添加按键
            saveConfig.config5.TryAddItem(new MyKV<string, KeyCode>($"{KeyCode.F3}", KeyCode.F3));
            //尝试添加按键
            saveConfig.config5.TryAddItem(new MyKV<string, KeyCode>($"窗口按键", KeyCode.F2));
            //保存配置文件
            File.WriteAllText(ConfigPath, saveConfig.ToJson());

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

            //从配置文件中读取目标按键
            k1 = saveConfig.config5.Find(d => d.Key == $"{KeyCode.Mouse2}").Value;
            F3 = saveConfig.config5.Find(d => d.Key == $"{KeyCode.F3}").Value;
            toggleWindowKey = saveConfig.config5.Find(d => d.Key == $"窗口按键").Value;
            //注册游戏更新事件处理器
            SetupGameUpdateHandler();
            "mod加载完成".Log();
        }


        /// <summary>
        /// 这是我们统一的游戏更新处理逻辑。
        /// 这个函数会一直执行,所以如果有些功能只需要偶尔执行需要添加执行条件.
        /// 不要随意往这个函数加东西,非常容易让游戏卡顿.
        /// </summary>
        private void MyUniversalGameUpdateHandler()
        {
            // 在这里编写你的游戏更新逻辑
            //是否按下了切换窗口的按键,如果不需要自定义窗口可以删除这个判断和相关代码
            if (Input.GetKey(toggleWindowKey))
            {
                //如果窗口不存在,则创建一个新的窗口
                if (myPopupWindow == null)
                {
                    myPopupWindow = new Tool.MyCustomWindow(title: title);
                    myPopupWindow.IsVisible = true;
                }
                else
                    //显示或隐藏窗口
                    myPopupWindow.IsVisible = !myPopupWindow.IsVisible;
            }

            //判断是否按下该按键,按下时会一直执行
            if (Input.GetKeyDown(F3))
            {
                //在控制台输出文本  这个只是例子,可以删除
                "Hello World 111".Log();
                //其他逻辑
            }

            //按下时执行,只执行1次
            if (Input.GetKey(k1))
            {
                //在控制台输出文本  这个只是例子,可以删除
                "Hello World 222".Log();
                //其他逻辑
            }
        }


        /// <summary>
        /// 使用反射注册游戏更新事件处理器,为了兼容不同版本的API,我使用了反射来注册这个处理器.
        /// 这个函数的原理比较复杂,如果你不理解它的原理,请不要修改!
        /// 如果你不是资深CSharp开发者,请不要随意修改这个函数!
        /// </summary>
        private void SetupGameUpdateHandler()
        {
            // 1.获取 ModEvents 类的 Type 对象
            var modEventsType = typeof(ModEvents);
            // 2.获取名为 "GameUpdate" 的字段
            var gameUpdateField = modEventsType.GetField("GameUpdate", BindingFlags.Static | BindingFlags.Public);
            // 3.获取该静态字段的值（即 ModEvent<SGameUpdateData> 的实例）
            var gameUpdateInstance = gameUpdateField.GetValue(null);
            // 4.从实例中获取其 Type
            var gameUpdateInstanceType = gameUpdateInstance.GetType();
            // 5. 从该类型中获取名为 "RegisterHandler" 的方法
            var registerHandlerMethod = gameUpdateInstanceType.GetMethod("RegisterHandler");

            // 尝试获取 v2.0+ 的 SGameUpdateData 类型
            var sGameUpdateDataType = modEventsType.GetNestedType("SGameUpdateData");

            Delegate handlerDelegate;

            if (sGameUpdateDataType != null)
            {
                // --- v2.0 或更高版本 ---
                // 我们检测到了 SGameUpdateData 类型的存在。
                // 目标委托签名: void(ref SGameUpdateData)
                "游戏版本为2.0以上".Log();

                // 获取我们自己的、统一的处理方法
                var myHandlerMethodInfo = typeof(ModTemplate).GetMethod(nameof(MyUniversalGameUpdateHandler), BindingFlags.Instance | BindingFlags.NonPublic);

                // 使用表达式树在运行时动态创建一个适配器委托
                // 表达式树将生成一个类似这样的lambda: (ref SGameUpdateData data) => this.MyUniversalGameUpdateHandler()

                // 1. 定义委托的参数 (类型为 ref SGameUpdateData)
                var delegateParam = Expression.Parameter(sGameUpdateDataType.MakeByRefType(), "data");

                // 2. 创建对 this.MyUniversalGameUpdateHandler() 的调用
                var methodCall = Expression.Call(Expression.Constant(this), myHandlerMethodInfo);

                // 3. 获取委托的类型，例如 ModEventHandlerDelegate<SGameUpdateData>
                var delegateType = registerHandlerMethod.GetParameters()[0].ParameterType;

                // 4. 创建并编译Lambda表达式
                var lambda = Expression.Lambda(delegateType, methodCall, delegateParam);
                // 5. 编译并获取委托实例
                handlerDelegate = lambda.Compile();
            }
            else
            {
                // --- v1.4 或更早版本 ---
                // 未找到 SGameUpdateData 类型。
                // 目标委托签名: void() (即 System.Action)
                "游戏版本为2.0以下".Log();
                handlerDelegate = (Action)MyUniversalGameUpdateHandler;
            }
            // 6. 调用 RegisterHandler 方法，传入我们创建的委托
            registerHandlerMethod.Invoke(gameUpdateInstance, new object[] { handlerDelegate });
        }

    }
}

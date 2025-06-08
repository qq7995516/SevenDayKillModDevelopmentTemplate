using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AIDirectorPlayerInventory;

namespace SevenDayKillModDevelopmentTemplate
{
    /// <summary>
    /// 工具类,把一些常用的函数放在这里,方便调用
    /// </summary>
    public static class Tool
    {
        #region 窗口
        /// <summary>
        /// 自定义窗口
        /// </summary>
        public class MyCustomWindow : GUIWindow
        {
            /// <summary>
            /// 字符串ID,由GUIWindow使用,唯一标识窗口,可以是任意字符串,但建议使用唯一的字符串
            /// </summary>
            public static string WindowID = "窗口字符串ID";
            /// <summary>
            /// 数字ID,由GuiLayout使用,唯一标识窗口,可以是任意数字,但建议使用唯一的数字
            /// </summary>
            public static int _guiLayoutWindowID = 0;
            /// <summary>
            /// 窗口的坐标与大小
            /// </summary>
            public Rect windowRect = new Rect(100, 100, 300, 200);
            /// <summary>
            /// 窗口是否可见
            /// </summary>
            public bool IsVisible = false;
            /// <summary>
            /// 窗口内容
            /// </summary>
            public string Content = "标题";
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="windowID">
            /// 窗口ID,用于唯一标识窗口,可以是任意字符串,但建议使用唯一的字符串
            /// </param>
            /// <param name="x">
            /// 窗口左上角X坐标,默认为100
            /// </param>
            /// <param name="y">
            /// 窗口左上角Y坐标,默认为100
            /// </param>
            /// <param name="width">
            /// 窗口宽度,默认为300
            /// </param>
            /// <param name="height">
            /// 窗口高度,默认为200
            /// </param>
            /// <param name="title">
            /// 窗口内容,默认为"窗口标题",可以是任意字符串
            /// </param>
            public MyCustomWindow(int x = 100, int y = 100, int width = 300, int height = 200, string windowID = null, string title = "窗口标题") : base(_id: WindowID)
            {
                //这里使用了随机ID,不要随意改动,除非你知道自己在做什么
                _guiLayoutWindowID = new System.Random().Next(1000, 9999);
                WindowID = windowID;
                windowRect = new Rect(x, y, width, height);
                Content = title;
            }

            /// <summary>
            /// 绘制函数,在这里绘制窗口内容
            /// </summary>
            /// <param name="_inputActive"></param>
            public override void OnGUI(bool _inputActive)
            {
                // 如果窗口不可见，则不执行任何绘制操作
                if (!IsVisible)
                    return;
                base.OnGUI(_inputActive);
                //定义整体颜色,风格
                GUI.DrawTexture(windowRect, CreateBackgroundTexture(new Color(0f, 0f, 0f, 0.7f)));
                //绘制
                GUILayout.Window(_guiLayoutWindowID, windowRect, MyWindow, Content);
            }

            /// <summary>
            /// 窗口回调函数,在这里添加控件
            /// </summary>
            /// <param name="id"></param>
            private void MyWindow(int id)
            {
                // 使窗口可以通过拖动其顶部区域来移动 (假设标题栏高度为20像素)
                GUI.DragWindow(new Rect(0, 0, windowRect.width, 20f));

                // 处理关闭事件: 如果按下了Esc键,则隐藏此窗口
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
                {
                    IsVisible = false; // 隐藏当前窗口
                    Event.current.Use();    // 消耗此事件，防止其他UI元素也处理它
                }
                AddControl();

                // 可以在这里添加一个显式的关闭按钮
                //if (GUILayout.Button("关闭"))
                //    IsVisible = false;
            }

            /// <summary>
            /// 在这里面添加控件,比如按钮,文本框等,这个函数每一帧都会被调用
            /// </summary>
            public void AddControl()
            {
                //水平布局,可以添加多个控件,每个控件占一行
                Add_Horizontal(options =>
                {
                    // 添加一个标签
                    "这是一个自定义窗口".AddLabel();
                    // 添加一个按钮,点击后隐藏窗口
                    "关闭窗口".AddButton(() =>
                    {
                        //点击按钮后执行的逻辑
                        IsVisible = false;

                        //返回空是为了在自定义函数兼容输入框,不要随便修改返回类型
                        return null;
                    });
                    // 添加一个文本框,可以输入内容
                    // 要用一个全局变量来存储输入的内容,否则每次都会创建一个新的文本框
                    // 这里只做例子,实际使用时请使用全局变量存储输入内容
                    var text = "输入内容:".AddTextField();
                    return null;
                });
                //垂直布局
                Add_Vertical(options =>
                {
                    // 添加一个标签
                    "这是一个垂直布局的标签".AddLabel();
                    // 添加一个按钮,点击后隐藏窗口
                    "关闭窗口".AddButton(() =>
                    {
                        IsVisible = false;
                        return null;
                    });
                    // 添加一个文本框,可以输入内容
                    "输入内容:".AddTextField();
                    return null;
                });

            }
            /// <summary>
            /// 创建背景纹理的方法
            /// </summary>
            /// <param name="color"></param>
            /// <returns></returns>
            private static Texture2D CreateBackgroundTexture(Color color)
            {
                // 创建新的 Texture2D 对象
                Texture2D texture2D = new Texture2D(1, 1);
                // 设置像素颜色
                texture2D.SetPixel(0, 0, color);
                // 应用纹理
                texture2D.Apply();
                // 返回纹理
                return texture2D;
            }
        }
        #region 窗口控件操作

        /// <summary>
        /// 不会Linq的不要随便使用这个函数,比较复杂,添加多个按钮,按每行固定数量分组显示按钮
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="collection">要显示的集合</param>
        /// <param name="itemsPerRow">每行显示的元素数量</param>
        /// <param name="getButtonText">生成按钮文本的函数</param>
        /// <param name="onButtonClick">按钮点击事件处理函数</param>
        public static void AddButtonsByRow<T>(
            this IEnumerable<T> collection,
            int itemsPerRow,
            Func<T, string> getButtonText,
            Func<T, object> onButtonClick)
        {
            var list = collection.ToList();

            // 按照指定的每行数量分组处理
            for (int groupIndex = 0; groupIndex < list.Count; groupIndex += itemsPerRow)
            {
                // 获取当前分组的元素（或剩余元素）
                var currentGroup = list.Skip(groupIndex).Take(itemsPerRow).ToList();

                Tool.Add_Horizontal(options =>
                {
                    foreach (var item in currentGroup)
                    {
                        // 使用局部变量解决闭包问题
                        var currentItem = item;
                        string buttonText = getButtonText(currentItem);

                        buttonText.AddButton(() =>
                        {
                            onButtonClick(currentItem);
                            return null;
                        });
                    }
                    return null;
                });
            }
        }

        /// <summary>
        /// 添加一个标签
        /// </summary>
        /// <param name="str"></param>
        public static void AddLabel(this string str, int fontSize = 17) => GUILayout.Label(str, new GUIStyle(GUI.skin.label) { fontSize = fontSize }, new GUILayoutOption[0]);

        /// <summary>
        /// 添加一个button
        /// </summary>
        /// <param name="str">按钮文本</param>
        /// <param name="action">点击事件</param>
        public static void AddButton(this string str, Func<object> action, int fontSize = 17)
        {
            if (GUILayout.Button(str, new GUIStyle(GUI.skin.button) { fontSize = fontSize }, new GUILayoutOption[0]))
                action();
        }

        /// <summary>
        /// 添加一个文本框
        /// </summary>
        /// <param name="str">文本</param>
        /// <returns></returns>
        public static string AddTextField(this string str, int fontSize = 17) => GUILayout.TextField(str, new GUIStyle(GUI.skin.textField) { fontSize = fontSize }, new GUILayoutOption[0]);

        /// <summary>
        /// 添加垂直布局的控件,垂直布局表示独占一行,可以添加多个控件
        /// </summary>
        /// <param name="str">要显示的内容</param>
        /// <param name="function">控件,如果是添加button,button的返回即为点击事件</param>
        public static object Add_Vertical(Func<GUILayoutOption[], object> function)
        {
            GUILayout.BeginVertical(); // 开始垂直布局
            var obj = function(new GUILayoutOption[0]); // 添加一个标签
            GUILayout.EndVertical(); // 结束垂直布局
            if (obj != null)
                return obj;
            return null;
        }

        /// <summary>
        ///添加水平布局的控件,表示与其他控件水平排列,可以添加多个控件
        /// </summary>
        /// <param name="str">要显示的内容</param>
        /// <param name="action">控件,如果是添加button,button的返回即为点击事件</param>
        /// <returns></returns>
        public static object Add_Horizontal(Func<GUILayoutOption[], object> function)
        {
            GUILayout.BeginHorizontal();
            var obj = function(new GUILayoutOption[0]); // 添加一个标签
            GUILayout.EndHorizontal();
            if (obj != null)
                return obj;
            return null;
        }

        #endregion


        #endregion

        #region 时间

        /// <summary>
        /// 获取世界时间戳,返回值为毫秒(我也不确定,一般是返回毫秒)
        /// </summary>
        /// <returns></returns>
        public static ulong GetWorldTime() => GameManager.Instance.World.worldTime;

        /// <summary>
        /// 获取世界天数
        /// </summary>
        /// <returns></returns>
        public static int GetWorldDay() => GameManager.Instance.World.WorldDay;

        /// <summary>
        /// 获取当前时针位置,返回值为0-23之间的整数
        /// </summary>
        /// <returns></returns>
        public static int GetWorldHour() => GameManager.Instance.World.WorldHour;

        /// <summary>
        /// 检查当前是否处于血月状态
        /// </summary>
        /// <returns></returns>
        public static bool IsBloodMoon() => GameManager.Instance.World.isEventBloodMoon;

        /// <summary>
        /// 检查当前是否白天
        /// </summary>
        /// <returns></returns>
        public static bool IsDayTime() => GameManager.Instance.World.IsDaytime();
        #endregion

        /// <summary>
        /// 尝试添加一个键值对到列表中
        /// </summary>
        /// <param name="myKeyValues"></param>
        /// <param name="myKeyValue"></param>
        /// <returns>添加成功会返回true</returns>
        public static bool TryAddItem<T>(this List<MyKV<string, T>> myKeyValues, MyKV<string, T> myKeyValue)
        {
            if (myKeyValues == null || myKeyValue == null)
                return false;
            if (myKeyValues.Any(d => d.Key == myKeyValue.Key))
                return false;
            myKeyValues.Add(new MyKV<string, T>(myKeyValue.Key, myKeyValue.Value));
            return true;
        }

        /// <summary>
        /// 把对象转为Json字符串
        /// </summary>
        /// <typeparam name="T"> 类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj) => JsonConvert.SerializeObject(obj); //转为字节数组

        /// <summary>
        /// Json字符串转对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static T JsonToObject<T>(this string JsonStr) => JsonConvert.DeserializeObject<T>(JsonStr);


        /// <summary>
        /// 尝试创建一个文件并写入文本,文件存在时则不执行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileInfo TryCreateAndWriteFile(this string path, string str)
        {
            if (!File.Exists(path))
                File.WriteAllText(path, str);
            return new FileInfo(path);
        }
        /// <summary>
        /// 尝试读取文件,如果文件不存在则返回空字符串
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string TryReadFile(this string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    return File.ReadAllText(path);
                }
                catch (Exception e)
                {
                    Debug.LogError($"读取文件失败: {e.Message}");
                    return string.Empty;
                }
            }
            else
            {
                Debug.LogWarning($"文件不存在: {path}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 尝试写入文件,写入成功会返回true,失败会返回false
        /// </summary>
        /// <param name="path"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TryWriteFile(this string path, string str)
        {
            try
            {
                File.WriteAllText(path, str);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"写入文件失败: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 尝试追加写入文件,追加成功会返回true,失败会返回false
        /// </summary>
        /// <param name="path"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TryAppendFile(this string path, string str)
        {
            try
            {
                File.AppendAllText(path, str);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"追加写入文件失败: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 在控制台打印日志
        /// </summary>
        /// <typeparam name="T">对象,一般为string,如果不是string则会调用对象的ToString()</typeparam>
        /// <param name="t"></param>
        public static void Log<T>(this T t) => Debug.Log(t);

        /// <summary>
        /// 根据id查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity GetEntity(this int id) =>
             GameManager.Instance.World.GetEntity(id);

        /// <summary>
        /// 根据名称查找实体
        /// 会返回所有包含该名称的实体,如果有多个实体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<Entity> FindEntity(this string name) =>
            GameManager.Instance.World.Entities.list.Where(d => d.name.Contains(name)).ToList();

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<Entity> GetEntitieALL() =>
            GameManager.Instance.World.Entities.list;

        public enum Items
        {
            /// <summary>
            /// 背包
            /// </summary>
            Bag,
            /// <summary>
            /// 快捷栏
            /// </summary>
            QuickBar
        }

        /// <summary>
        /// 获取玩家背包或快捷栏所有物品
        /// </summary>
        /// <param name="items">获取类型,默认获取背包物品</param>
        /// <returns></returns>
        public static List<ItemStack> GetItemStacks(Items items = Items.Bag)
        {
            var localPlayer = GetlocalPlayer();
            switch (items)
            {
                case Items.Bag:
                    return localPlayer.bag.items.ToList();
                case Items.QuickBar:
                    return localPlayer.inventory.GetSlots().ToList();
                default:
                    return null;
            }
        }


        /// <summary>
        /// 获取本地玩家
        /// </summary>
        /// <returns></returns>
        public static EntityPlayerLocal GetlocalPlayer()
        {
            return GameManager.Instance?.World?.GetPrimaryPlayer();
        }

    }
}

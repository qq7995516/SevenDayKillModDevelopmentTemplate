using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static SevenDayKillModDevelopmentTemplate.UIHelpers;

namespace SevenDayKillModDevelopmentTemplate
{
    /// <summary>
    /// 自定义UI窗口，继承自GUIWindow以与游戏UI系统集成。
    /// </summary>
    public class MyCustomWindow : GUIWindow
    {
        // 窗口的数字ID，由GUILayout使用，必须唯一。
        private readonly int _guiLayoutWindowID;
        // 窗口在屏幕上的位置和大小。
        private Rect _windowRect = new Rect(100, 100, 400, 500);
        // 窗口标题。
        private readonly string _title;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="windowId">窗口的唯一字符串ID，由WindowManager使用。</param>
        /// <param name="title">窗口标题。</param>
        public MyCustomWindow(string windowId, string title = "自定义窗口") : base(_id: windowId)
        {
            // 使用随机数确保GUILayout的ID唯一，避免与其他Mod冲突。
            _guiLayoutWindowID = new System.Random().Next(10000, 99999);
            _title = title;
        }

        /// <summary>
        /// 绘制窗口的核心方法，由WindowManager在窗口打开时每帧调用。
        /// </summary>
        public override void OnGUI(bool _inputActive)
        {
            base.OnGUI(_inputActive);

            // 绘制半透明背景
            GUI.DrawTexture(_windowRect, CreateBackgroundTexture(new Color(0f, 0f, 0f, 0.8f)));

            // 使用GUILayout.Window来创建和绘制窗口。
            // _windowRect会通过引用传递，因此拖动窗口时会自动更新其位置。
            _windowRect = GUILayout.Window(_guiLayoutWindowID, _windowRect, MyWindow, _title);
        }

        /// <summary>
        /// 定义窗口内部的所有UI控件。
        /// </summary>
        /// <param name="id">窗口的数字ID。</param>
        private void MyWindow(int id)
        {
            // 允许通过拖动窗口顶部25像素的区域来移动窗口。
            GUI.DragWindow(new Rect(0, 0, _windowRect.width, 25f));

            // 按下Escape键时关闭窗口。
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                // 通过WindowManager来关闭自己，这是标准做法。
                GameManager.Instance.m_GUIConsole.windowManager.Close(base.Id);
                Event.current.Use(); // 消耗事件，防止其他UI响应。
            }

            // --- 在这里使用已有的辅助方法添加UI控件 ---

            // 示例：使用 Add_Vertical 和 AddLabel
            Add_Vertical(options =>
            {
                "示例窗口".AddLabel(20);
                return null; // 遵循 Func<GUILayoutOption[], object> 签名
            });

            // 示例：使用 Add_Horizontal 和 AddButton
            Add_Horizontal(options =>
            {
                "关闭窗口".AddButton(() =>
                {
                    // 点击按钮后执行的逻辑
                    GameManager.Instance.m_GUIConsole.windowManager.Close(base.Id);

                    // 遵循 Func<object> 签名，返回 null
                    return null;
                });

                "打印日志".AddButton(() =>
                {
                    "按钮被点击了!".Log();
                    return null;
                });

                return null; // 遵循 Func<GUILayoutOption[], object> 签名
            });

            // ---------------------------------------------
        }

        /// <summary>
        /// 创建一个纯色纹理作为窗口背景。
        /// </summary>
        private static Texture2D CreateBackgroundTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SevenDayKillModDevelopmentTemplate
{
    public static class UIHelpers
    {

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

                Add_Horizontal(options =>
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

    }
}

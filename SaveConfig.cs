using System.Collections.Generic;
using UnityEngine;

namespace SevenDayKillModDevelopmentTemplate
{
    /// <summary>
    /// 配置类,用于存放Mod的配置,该类必须是public的,且必须有一个无参构造函数
    /// </summary>
    public class SaveConfig
    {
        /// <summary>
        /// 这句话是给玩家看的.
        /// </summary>
        public string brief_introduction = "如果要修改按键,请参考同文件夹下的key.txt文件";
        /// <summary>
        /// 用于存放Bool类型
        /// </summary>
        public List<MyKV<string, bool>> config1 = new List<MyKV<string, bool>>() { };
        /// <summary>
        /// 用于存放int类型
        /// </summary>
        public List<MyKV<string, int>> config2 = new List<MyKV<string, int>>() { };
        /// <summary>
        /// 用于存放string类型
        /// </summary>
        public List<MyKV<string, string>> config3 = new List<MyKV<string, string>>() { };
        /// <summary>
        /// 用于存放float类型
        /// </summary>
        public List<MyKV<string, double>> config4 = new List<MyKV<string, double>>() { };
        /// <summary>
        /// 用于存放KeyCode类型   也就是按键,当然可以直接存放int类型,但是这样不方便查看,何必节省那几个字节折磨自己呢.
        /// </summary>
        public List<MyKV<string, KeyCode>> config5 = new List<MyKV<string, KeyCode>>() { };
    }

    /// <summary>
    /// 键值对类,因为原版的字典不支持修改,所以自己写了一个
    /// </summary>
    /// <typeparam name="K">键类型</typeparam>
    /// <typeparam name="V">值联系</typeparam>
    public class MyKV<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public MyKV(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}
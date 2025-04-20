using System.Collections.Generic;
using UnityEngine;

namespace SevenDayKillModDevelopmentTemplate
{
    public class SaveConfig
    {
        public string brief_introduction = "如果要修改按键,请参考同文件夹下的key.txt文件";
        public List<MyKV<string, bool>> config1 = new List<MyKV<string, bool>>() { };
        public List<MyKV<string, int>> config2 = new List<MyKV<string, int>>() { };
        public List<MyKV<string, string>> config3 = new List<MyKV<string, string>>() { };
        public List<MyKV<string, double>> config4 = new List<MyKV<string, double>>() { };
        public List<MyKV<string, KeyCode>> config5 = new List<MyKV<string, KeyCode>>() { };
    }

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
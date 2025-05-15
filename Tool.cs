using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SevenDayKillModDevelopmentTemplate
{
    /// <summary>
    /// 工具类,把一些常用的函数放在这里,方便调用
    /// </summary>
    public static class Tool
    {

        /// <summary>
        /// 创建一个文件并写入文本,文件存在时则不执行
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

        public static void Log<T>(this T t) => Debug.Log(t);
    }
}

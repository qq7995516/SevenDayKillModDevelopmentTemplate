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
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity FindEntity(this string name) =>
            GameManager.Instance.World.Entities.list.Find(d => d.name == name);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<Entity> GetEntitieALL() =>
            GameManager.Instance.World.Entities.list;

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

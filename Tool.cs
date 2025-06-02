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

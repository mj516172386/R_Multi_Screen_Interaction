using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;  
using Newtonsoft.Json; 
using UnityEngine;
using UnityEngine.Events;

public class Utils
{ 
     
    /// <summary>
    /// Retrun Child Transform
    /// </summary>
    /// <param name="parent"></param> Parent Transform
    /// <param name="childName"></param> Get ChildName
    /// <returns></returns>
    public static Transform GetChildTransform(Transform parent,string childName)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name== childName)
            {
                return parent.GetChild(i);
            }
        }
        return null;
    }
    /// <summary>
    /// 返回图片纹理
    /// </summary>
    /// <param name="_strImagePath"></param>
    /// <returns></returns>
    public static Sprite GetTexture2d(string _strImagePath)
    {
        Texture2D tx = new Texture2D(100, 100);
        tx.LoadImage(GetImageByte(_strImagePath));
        Sprite sp = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);
        return sp;
    }

    /// <summary>  
    /// 根据图片路径返回图片的字节流byte[]  
    /// </summary>  
    /// <param name="imagePath">图片路径</param>  
    /// <returns>返回的字节流</returns>  
    public static byte[] GetImageByte(string imagePath)
    {
        FileStream files = new FileStream(imagePath, FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();
        return imgByte;
    }
}
public class UtilsJson<T> 
{
    //Json数据写入
    public static void WriteJsonConfigFile(string path, string fileName, T configInfo)
    {

        string file = System.IO.Path.Combine(path, fileName);
        if (!File.Exists(file))
        {
            File.CreateText(file).Dispose();
        }

        string data = JsonUtility.ToJson(configInfo, true);
        Debug.Log("数据写入 data=  "+data);
        File.WriteAllText(file, data);

    }
    //读取JSON内容
    public static T ReadJsonConfigFile(string path, string fileName)
    {
        string file = System.IO.Path.Combine(path, fileName);
        T ConfigInfo = default(T);
        if (File.Exists(file))
        {
            string data = File.ReadAllText(file);
            ConfigInfo = JsonUtility.FromJson<T>(data);
        }
        return ConfigInfo;
    }
    //使用.NET框架中的某Dll文件写入JSON单条数据
    public static void UseNetFrameWriteJsonConfigFile(T course, string path, string fileName)
    {
        string file = System.IO.Path.Combine(path, fileName);
        if (!File.Exists(file))
        {
            File.CreateText(file).Dispose();
        }
        string jsondata = JsonConvert.SerializeObject(course);
        File.WriteAllText(file, jsondata);
    }
    //使用.NET框架中的某Dll文件写入List多条数据到Json文件
    public static void UseNetFrameWriteJsonConfigFiles(List<T> course, string path, string fileName)
    {
        string file = System.IO.Path.Combine(path, fileName);
        if (!File.Exists(file))
        {
            File.CreateText(file).Dispose();
        }
        string jsondata = JsonConvert.SerializeObject(course);
        File.WriteAllText(file, jsondata);
    }
    //使用.NET框架中的某Dll文件读取JSON
    public static T UseNetFrameReadJsonConfigFile(string path, string fileName)
    {
        string file = System.IO.Path.Combine(path, fileName);
        T ConfigInfo = default(T);
        if (File.Exists(file))
        {
            string data = File.ReadAllText(file);
            ConfigInfo = JsonConvert.DeserializeObject<T>(data);
        }
        return ConfigInfo;
    }
    //使用.NET框架中的某Dll文件读取JSON
    public static List<T> UseNetFrameReadJsonConfigFiles(string path, string fileName)
    {
        string file = System.IO.Path.Combine(path, fileName);
        List<T> ConfigInfo = new List<T>();
        if (File.Exists(file))
        {
            string data = File.ReadAllText(file);
            ConfigInfo = JsonConvert.DeserializeObject<List<T>>(data);
        }
        return ConfigInfo;
    }
} 
 
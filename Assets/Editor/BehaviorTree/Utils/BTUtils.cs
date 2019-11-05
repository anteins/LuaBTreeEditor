using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class BTUtils
{
    public static string jsonFile = "json.json";

    public static void DrawBezier(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent)
    {
        Handles.DrawBezier(
            startPosition,
            endPosition,
            startTangent,
            endTangent,
            Color.green,
            null,
            3f
        );
    }

    public static void DumpTree(BaseNode node, Action<BaseNode> action = null)
    {
        if (node == null)
            return;

        if(action != null)
            action(node);

        if (node.childs.Count > 0)
        {
            for (int i = 0; i < node.childs.Count; i++)
            {
                DumpTree(node.childs[i], action);
            }
        }
    }

    public static void RemoveTree(BaseNode node)
    {
        NodeDataManager.Remove(node);
        if (node.childs.Count > 0)
        {
            for (int i = 0; i < node.childs.Count; i++)
            {
                DumpTree(node.childs[i]);
            }
        }
    }

    public static string GetGenPath()
    {
        return "Assets/Editor/BehaviorTree/Gen/";
    }

    /// <summary>
    /// 获取文件二进制数据 
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public static byte[] GetFileBytes(string path)
    {
        FileStream file;
        try
        {
            if (File.Exists(path))
            {
                file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                return null;
            }
        }
        catch (FileLoadException ex)
        {
            Debug.Log("#Error : Open file which path of : " + path + " : " + ex.Message);
            return null;
        }

        BinaryReader binaryReader = new BinaryReader(file);
        byte[] buff = binaryReader.ReadBytes((int)file.Length);

        binaryReader.Close();
        file.Close();

        return buff;
    }

    /// <summary>
    /// 解析json文件成为具体类对象 
    /// </summary>
    /// <typeparam name="T">泛类类型</typeparam>
    /// <param name="fileName">读取文件目录</param>
    /// <param name="ext">文件扩展名</param>
    /// <returns></returns>
    public static T GetJsonFromFile<T>(string fileName, string ext)
    {
        return GetJsonFromFile<T>(fileName + ext);
    }
    public static T GetJsonFromFile<T>(string fileName)
    {
        byte[] buff = GetFileBytes(fileName);

        if (buff != null)
        {
            string json_str = System.Text.UTF8Encoding.UTF8.GetString(buff);
            return LitJson.JsonMapper.ToObject<T>(json_str);
        }
        return default(T);
    }

    /// <summary>
    /// 保存就json数据到文件
    /// </summary>
    /// <typeparam name="T">泛类类型</typeparam>
    /// <param name="obj">具体数据对象</param>
    /// <param name="savePath">保存路径</param>
    /// <param name="isCompress">是否需压缩</param>
    public static void SaveJsonToFile<T>(T obj, string savePath)
    {
        byte[] buff = TransformObjToJsonByte(obj);
        if (buff.Length > 0)
        {
            SaveFileByte(buff, savePath);
        }
        Debug.Log("保存到 " + savePath);
    }

    /// <summary>
    /// 保存二进制数据到文件 
    /// </summary>
    /// <param name="buff">二进制数据</param>
    /// <param name="savePath">保存路径</param>
    /// <param name="isCompress ">是否需要压缩</param>
    public static void SaveFileByte(byte[] buff, string savePath)
    {
        string dir = savePath.Substring(0, savePath.LastIndexOf("/"));
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        try
        {
            FileStream fs = new FileStream(savePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(buff);
            writer.Close();
            fs.Close();
        }
        catch (IOException exception)
        {
            Debug.Log(exception);
        }
    }

    /// <summary>
    /// 转换对象成为json二进制数据
    /// </summary>
    /// <typeparam name="T">泛类类型</typeparam>
    /// <param name="obj">泛类类型对象</param>
    /// <returns></returns>
    public static byte[] TransformObjToJsonByte<T>(T obj)
    {
        string jsonData = LitJson.JsonMapper.ToJson(obj);
        byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes(jsonData);
        return buff;
    }
}
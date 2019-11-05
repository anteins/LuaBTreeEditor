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
    /// ��ȡ�ļ����������� 
    /// </summary>
    /// <param name="path">�ļ�·��</param>
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
    /// ����json�ļ���Ϊ��������� 
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="fileName">��ȡ�ļ�Ŀ¼</param>
    /// <param name="ext">�ļ���չ��</param>
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
    /// �����json���ݵ��ļ�
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="obj">�������ݶ���</param>
    /// <param name="savePath">����·��</param>
    /// <param name="isCompress">�Ƿ���ѹ��</param>
    public static void SaveJsonToFile<T>(T obj, string savePath)
    {
        byte[] buff = TransformObjToJsonByte(obj);
        if (buff.Length > 0)
        {
            SaveFileByte(buff, savePath);
        }
        Debug.Log("���浽 " + savePath);
    }

    /// <summary>
    /// ������������ݵ��ļ� 
    /// </summary>
    /// <param name="buff">����������</param>
    /// <param name="savePath">����·��</param>
    /// <param name="isCompress ">�Ƿ���Ҫѹ��</param>
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
    /// ת�������Ϊjson����������
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="obj">�������Ͷ���</param>
    /// <returns></returns>
    public static byte[] TransformObjToJsonByte<T>(T obj)
    {
        string jsonData = LitJson.JsonMapper.ToJson(obj);
        byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes(jsonData);
        return buff;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using EvolvingCode.MergingBoard;

public static class FileHandler
{
    public static void SaveToJSON<T>(List<T> to_Save, string filename)
    {
        Debug.Log(GetPath(filename));
        WriteFile(GetPath(filename), JsonHelper.ToJson<T>(to_Save.ToArray()));
    }
    public static void SaveToJSON<T>(T to_Save, string filename)
    {
        Debug.Log(GetPath(filename));
        WriteFile(GetPath(filename), JsonUtility.ToJson(to_Save));
    }

    public static List<T> ReadListFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));
        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }

        List<T> results = JsonHelper.FromJson<T>(content).ToList();

        return results;
    }

    public static T ReadFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));
        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return default(T);
        }

        T results = JsonUtility.FromJson<T>(content);

        return results;
    }

    public static string GetPath(string filename)
    {
        return Application.persistentDataPath + "/" + filename;
    }

    public static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    public static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
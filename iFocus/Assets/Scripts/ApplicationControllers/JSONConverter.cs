using System;
using System.IO;
using UnityEngine;

public class JSONConverter
{
    private string _fileFormat = ".json";
    private string _fileJson;
    private bool _isReady;
    private bool _isSuccess;

    public T[] GetData<T>(string fileName, Action success, Action fail)
    {
        string filePath;

#if UNITY_EDITOR
        filePath = Path.Combine(Application.streamingAssetsPath, fileName + _fileFormat);
        LoadData(filePath);

#elif UNITY_IOS
        filePath = Path.Combine(Application.dataPath + "/Raw", fileName + _fileFormat);
        LoadData(filePath);

#elif UNITY_ANDROID
        filePath = Path.Combine(Application.streamingAssetsPath + "/", fileName + _fileFormat);
        LoadDataAndroid(filePath);

#endif

        if (!_isSuccess)
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> JSON File don't founded. File Path: {filePath}");
            fail.Invoke();
            return null;
        }
        success.Invoke();
        return Converter<T>();
    }

    private void LoadData(string filePath)
    {
        if (File.Exists(filePath))
        {
            _fileJson = File.ReadAllText(filePath);
            _isSuccess = true;
        }
        else
        {
            _isSuccess = false;
        }
    }

    private void LoadDataAndroid(string filePath)
    {
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW reader = new WWW(filePath); // DON'T CHANGE!

            while (!reader.isDone) { }

            _fileJson = reader.text;
        }
        else
        {
            _fileJson = File.ReadAllText(filePath);
        }

        _isSuccess = true;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array = null;
    }

    private T[] Converter<T>()
    {
        string newJson = "{ \"array\": " + _fileJson + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }
}
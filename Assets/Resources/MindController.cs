using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MindController : MonoBehaviour
{
    [System.Serializable]
    public class PositionsWrapper
    {
        public float[] position;

        public PositionsWrapper(float[] array)
        {
            position = array;
        }
    }

    [System.Serializable]
    public class RecordsWrapper
    {
        public List<PositionsWrapper> records;

        public RecordsWrapper(List<PositionsWrapper> arrays)
        {
            records = arrays;
        }
    }

    public List<float[]> Mind;

    void SaveToJson(List<float[]> floatList, string fileName)
    {
        List<PositionsWrapper> wrappedArrays = new List<PositionsWrapper>();
        foreach (var array in floatList)
        {
            wrappedArrays.Add(new PositionsWrapper(array));
        }

        RecordsWrapper wrapper = new RecordsWrapper(wrappedArrays);
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName), json);
        Debug.Log("File path: " + Application.persistentDataPath + "\nSaved to JSON: " + json);
    }

    List<float[]> LoadFromJson(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            RecordsWrapper wrapper = JsonUtility.FromJson<RecordsWrapper>(json);
            List<float[]> floatArrays = new List<float[]>();
            foreach (var wrappedArray in wrapper.records)
            {
                floatArrays.Add(wrappedArray.position);
            }
            return floatArrays;
        }
        else
        {
            Debug.LogError("File does not exist: " + path);
            return new List<float[]>();
        }
    }

    [Button]
    public void SaveTest()
    {
        List<float[]> floatList = new List<float[]>
        {
            new float[] { 1.0f, 2.0f, 3.0f },
            new float[] { 4.0f, 5.0f, 6.0f }
        };

        // Save to JSON file
        SaveToJson(floatList, "data.json");
    }

    [Button]
    public void LoadTest()
    {
        List<float[]> loadedList = LoadFromJson("data.json");

        // Printing loaded data
        foreach (var array in loadedList)
        {
            Debug.Log("Array: " + string.Join(", ", array));
        }
    }
}
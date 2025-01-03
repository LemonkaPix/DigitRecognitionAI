using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MindController : MonoBehaviour
{
    public static List<float[]> Mind = new List<float[]>();

    #region Saving to file
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


    static void SaveToJson(List<float[]> floatList, string fileName)
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

    static List<float[]> LoadFromJson(string fileName)
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
    #endregion

    [Button]
    public static void SaveMind()
    {
        SaveToJson(Mind, "mind.json");
    }

    [Button]
    public static void LoadMind()
    {
        Mind = LoadFromJson("mind.json");
    }

    #region TESTS
        [Button]
        public void SaveTest()
        {
            List<float[]> floatList = new List<float[]>
            {
                new float[] { 1.0f, 2.0f, 3.0f },
                new float[] { 4.0f, 5.0f, 6.0f }
            };

            // Save to JSON file
            SaveToJson(floatList, "test.json");
        }

        [Button]
        public void LoadTest()
        {
            List<float[]> loadedList = LoadFromJson("test.json");

            // Printing loaded data
            foreach (var array in loadedList)
            {
                Debug.Log("Array: " + string.Join(", ", array));
            }
        }

    #endregion

}
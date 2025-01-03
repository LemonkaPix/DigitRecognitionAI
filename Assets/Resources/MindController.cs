using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MindController : MonoBehaviour
{
    public static List<(int, float[])> Mind = new List<(int, float[])>();

    #region Saving to file
    [System.Serializable]
    public class TupleWrapper
    {
        public int number;
        public float[] position;

        public TupleWrapper(int id, float[] array)
        {
            this.number = id;
            this.position = array;
        }
    }

    [System.Serializable]
    public class RecordsWrapper
    {
        public List<TupleWrapper> records;

        public RecordsWrapper(List<TupleWrapper> tuples)
        {
            this.records = tuples;
        }
    }

    static void SaveToJson(List<(int, float[])> tupleList, string fileName)
    {
        List<TupleWrapper> wrappedTuples = new List<TupleWrapper>();
        foreach (var tuple in tupleList)
        {
            wrappedTuples.Add(new TupleWrapper(tuple.Item1, tuple.Item2));
        }

        RecordsWrapper wrapper = new RecordsWrapper(wrappedTuples);
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName), json);
        Debug.Log("File path: " + Application.persistentDataPath + "\nSaved to JSON: " + json);
    }

    static List<(int, float[])> LoadFromJson(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            RecordsWrapper wrapper = JsonUtility.FromJson<RecordsWrapper>(json);
            List<(int, float[])> tuples = new List<(int, float[])>();
            foreach (var wrappedTuple in wrapper.records)
            {
                tuples.Add((wrappedTuple.number, wrappedTuple.position));
            }
            return tuples;
        }
        else
        {
            Debug.LogError("File does not exist: " + path);
            return new List<(int, float[])>();
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
            List<(int, float[])> originalList = new List<(int, float[])>
            {
                (1, new float[] { 1.1f, 2.2f, 3.3f }),
                (2, new float[] { 4.4f, 5.5f, 6.6f }),
                (3, new float[] { 7.7f, 8.8f, 9.9f })
            };

        // Save to JSON file
        SaveToJson(originalList, "test.json");
        }

        [Button]
        public void LoadTest()
        {
            List<(int, float[])> loadedList = LoadFromJson("test.json");

            // Printing loaded data
            foreach (var array in loadedList)
            {
                Debug.Log($"Number: {array.Item1} Array: {string.Join(", ", array.Item2)}");
            }
        }

    #endregion

}
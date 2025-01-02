using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MNISTLoader : MonoBehaviour
{
    [SerializeField] bool Debugging = false;

    public TextAsset trainingDataFile;
    public TextAsset testDataFile;

    public List<float[]> trainingData;
    public List<float[]> testData;

    void Start()
    {
        trainingData = LoadData(trainingDataFile);
        testData = LoadData(testDataFile);

        Debug.Log("Train data loaded: " + trainingData.Count);
        Debug.Log("Test data loaded: " + testData.Count);
    }

    List<float[]> LoadData(TextAsset dataFile)
    {
        List<float[]> data = new List<float[]>();
        string[] lines = dataFile.text.Split('\n');

        if (Debugging) 
        {
            string line = lines[0];
            string[] values = line.Split(',');
            if (values.Length > 1)
            {
                float[] pixels = new float[values.Length - 1];
                for (int i = 1; i < values.Length; i++)
                {
                    pixels[i - 1] = float.Parse(values[i]) / 255.0f;
                }
                data.Add(pixels);
            }
        }
        else
        {
            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                if (values.Length > 1)
                {
                    float[] pixels = new float[values.Length - 1];
                    for (int i = 1; i < values.Length; i++)
                    {
                        pixels[i - 1] = float.Parse(values[i]) / 255.0f;
                    }
                    data.Add(pixels);
                }
            }
        }


        return data;
    }
}
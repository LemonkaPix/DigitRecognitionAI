using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using JetBrains.Annotations;
using NaughtyAttributes;

public class MNISTLoader : MonoBehaviour
{
    public static MNISTLoader instance;
    [SerializeField] bool Debugging = false;
    [SerializeField] int debugExamples = 1;

    public TextAsset trainingDataFile;
    public TextAsset testDataFile;

    public List<(int, float[])> trainingData;
    public List<(int, float[])> testData;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    [Button]
    void LoadData()
    {
        trainingData = LoadData(trainingDataFile);
        testData = LoadData(testDataFile);

        print("Train data loaded: " + trainingData.Count);
        print("Test data loaded: " + testData.Count);
    }

    List<(int, float[])> LoadData(TextAsset dataFile)
    {
        List<(int, float[])> data = new List<(int, float[])>();
        string[] lines = dataFile.text.Split('\n');

        if (Debugging) 
        {
            for (int j = 0; j < debugExamples; j++)
            {
                string line = lines[j];
                string[] values = line.Split(',');


                if (values.Length > 1)
                {
                    float[] pixels = new float[values.Length - 1];
                    for (int i = 1; i < values.Length; i++)
                    {
                        pixels[i - 1] = float.Parse(values[i]) / 255.0f;
                    }
                    data.Add((Convert.ToInt32(values[0]), pixels));
                }

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
                    data.Add((Convert.ToInt32(values[0]), pixels));
                }
            }
        }


        return data;
    }
}
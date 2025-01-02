using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningManager : MonoBehaviour
{
    [Button]
    public void StartLearning()
    {
        foreach (float[] image in MNISTLoader.instance.trainingData)
        {
            bool[,] map = FloatArrayToBoolArray(image);
        }

    }

    private static bool[,] FloatArrayToBoolArray(float[] image)
    {
        bool[,] map = new bool[28, 28];

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                if (image[y * 28 + x] < .5f)
                {
                    map[x, y] = false;
                }
                else
                {
                    map[x, y] = true;
                }
            }
        }

        return map;
    }

    [Button]
    public void test()
    {
        var map = FloatArrayToBoolArray(MNISTLoader.instance.trainingData[0]);
        MNISTVisualizer.Instance.VisualizeImage(map);
    }
}

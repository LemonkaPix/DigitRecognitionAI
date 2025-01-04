using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Usage : MonoBehaviour
{
    [SerializeField] TMP_Text outputText;

    public static float DataVectorDistance(float[] v1, float[] v2)
    {
        if (v1.Length != 9 || v2.Length != 9)
        {
            throw new ArgumentException("Obie tablice musz¹ mieæ d³ugoœæ 9.");
        }

        float sumaKwadratow = 0.0f;
        for (int i = 0; i < 9; i++)
        {
            sumaKwadratow += (float)Math.Pow(v1[i] - v2[i], 2);
        }

        return (float)Math.Sqrt(sumaKwadratow);
    }

    public static int RecognizeByClosestNeighbour(float[] image)
    {
        MapState[,] map = ImageOperations.FloatArrayToMapState(image);

        float[] vector = new float[9];

        int whiteCount = ImageOperations.CountState(map, MapState.normal);

        List<List<(int, int)>> directions = new List<List<(int, int)>> {
                new List<(int, int)> { (1, 0), (-1, 0), (0, 1), }, //TOP
                new List<(int, int)> { (1, 1), (-1, 0), (0, 1), (-1, -1), }, //TOP LEFT
                new List<(int, int)> { (1, 0), (-1, 1), (0, 1), (1, -1), }, //TOP RIGHT
                new List<(int, int)> { (-1, 0), (0, 1), (0, -1), }, //LEFT
                new List<(int, int)> { (1, 0), (0, 1), (0, -1), }, //RIGHT
                new List<(int, int)> { (1, -1), (-1, 0), (-1, 1), (0, -1), }, //BOTTOM LEFT
                new List<(int, int)> { (1, 0), (-1, -1), (1, 1), (0, -1), }, //BOTTOM RIGHT
                new List<(int, int)> { (1, 0), (-1, 0), (0, -1), }, //BOTTOM
                new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1), }, //ALL
            };

        for (int i = 0; i < directions.Count; i++)
        {
            int count = ImageOperations.BFS(map, directions[i]);
            count = (28 * 28) - count - whiteCount;
            vector[i] = (float)count / ((28 * 28) - whiteCount);
        }

        (int, float) closestNeighbour = (-1,1f);   //digit,distance

        foreach ((int, float[]) item in MindController.Mind)
        {
            float distance = DataVectorDistance(vector, item.Item2);
            if(distance < closestNeighbour.Item2)
            {
                closestNeighbour.Item1 = item.Item1;
                closestNeighbour.Item2 = distance;
            }
        }

        return closestNeighbour.Item1;
    }

    public static int RecognizeByClosestDataCloud(float[] image)
    {
        MapState[,] map = ImageOperations.FloatArrayToMapState(image);

        float[] vector = new float[9];

        int whiteCount = ImageOperations.CountState(map, MapState.normal);

        List<List<(int, int)>> directions = new List<List<(int, int)>> {
                new List<(int, int)> { (1, 0), (-1, 0), (0, 1), }, //TOP
                new List<(int, int)> { (1, 1), (-1, 0), (0, 1), (-1, -1), }, //TOP LEFT
                new List<(int, int)> { (1, 0), (-1, 1), (0, 1), (1, -1), }, //TOP RIGHT
                new List<(int, int)> { (-1, 0), (0, 1), (0, -1), }, //LEFT
                new List<(int, int)> { (1, 0), (0, 1), (0, -1), }, //RIGHT
                new List<(int, int)> { (1, -1), (-1, 0), (-1, 1), (0, -1), }, //BOTTOM LEFT
                new List<(int, int)> { (1, 0), (-1, -1), (1, 1), (0, -1), }, //BOTTOM RIGHT
                new List<(int, int)> { (1, 0), (-1, 0), (0, -1), }, //BOTTOM
                new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1), }, //ALL
            };

        for (int i = 0; i < directions.Count; i++)
        {
            int count = ImageOperations.BFS(map, directions[i]);
            count = (28 * 28) - count - whiteCount;
            vector[i] = (float)count / ((28 * 28) - whiteCount);
        }

        //Dictionary<int, List<float>> dataClouds = new Dictionary<int, List<float>>();

        List<float>[] dataClouds = new List<float>[10];
        float[] avgDistanceFromCLoud = new float[10];

        for (int i = 0; i < 10; i++)
        {
            dataClouds[i] = new List<float>();
        }

        foreach ((int, float[]) item in MindController.Mind)
        {
            float distance = DataVectorDistance(vector, item.Item2);

            dataClouds[item.Item1].Add(distance);
        }

        for (int i = 0; i < dataClouds.Length; i++)
        {
            List<float> distances = dataClouds[i];

            float sum = 0;

            foreach (float distance in distances)
            {
                sum += distance;
            }

            avgDistanceFromCLoud[i] = sum/distances.Count;
        }

        int closestDigit = -1;
        int closestDistance = 2;

        for (int i = 0; i < avgDistanceFromCLoud.Length; i++)
        {
            var avgDist = avgDistanceFromCLoud[i];
            if(avgDist < closestDistance)
            {
                closestDigit = i;
            }
        }

        return closestDigit;
    }

    public float[] TextureToMap(Texture2D texture)
    {
        float[] map = new float[28*28];

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color color = texture.GetPixel(x,y);

                if (color == Color.black)
                    map[(27 - y) * 28 + x] = 0;
                else
                if (color == Color.white)
                    map[(27 - y) * 28 + x] = 1;
            }
        }

        return map;
    }

    [Button]
    public void UseModel()
    {
        float[] map = TextureToMap(MNISTVisualizer.Instance.texture);
        int digit = RecognizeByClosestNeighbour(map);
        print(digit);
        outputText.text = $"You drew {digit}!";
    }

    [Button]
    public void test()
    {
        print("AI response: " + RecognizeByClosestNeighbour(MNISTLoader.instance.trainingData[0].Item2));
        print("Correct response: " + MNISTLoader.instance.trainingData[0].Item1);
    }
}

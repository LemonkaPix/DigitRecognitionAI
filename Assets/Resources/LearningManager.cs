using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LearningManager : MonoBehaviour
{

    [Button]
    public void StartLearning()
    {
        MindController.ClearMind();

        foreach ((int,float[]) image in MNISTLoader.instance.trainingData)
        {
            MapState[,] map = ImageOperations.FloatArrayToMapState(image.Item2);
            //MNISTVisualizer.Instance.VisualizeImage(map);

            (int, float[]) mindRecord;
            mindRecord.Item1 = image.Item1;
            mindRecord.Item2 = new float[9];

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
                mindRecord.Item2[i] = (float)count / ((28 * 28) - whiteCount);
            }


            MindController.Mind.Add((mindRecord.Item1, mindRecord.Item2));
        }

        MindController.SaveMind();
    }



    [Button]
    public void test()
    {
        var map = ImageOperations.FloatArrayToMapState(MNISTLoader.instance.trainingData[0].Item2);
        MNISTVisualizer.Instance.VisualizeImage(map);
        var count = ImageOperations.BFS(map, new List<(int, int)> { (1, 0), (0, 1), (0, -1) });
        print((28*28) - count - ImageOperations.CountState(map, MapState.normal));
    }
}

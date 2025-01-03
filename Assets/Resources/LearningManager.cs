using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningManager : MonoBehaviour
{

    [Button]
    public void StartLearning()
    {
        foreach ((int,float[]) image in MNISTLoader.instance.trainingData)
        {
            MapState[,] map = ImageOperations.FloatArrayToMapState(image.Item2);

            
        }
    }



    [Button]
    public void test()
    {
        var map = ImageOperations.FloatArrayToMapState(MNISTLoader.instance.trainingData[0].Item2);
        MNISTVisualizer.Instance.VisualizeImage(map);
        StartCoroutine(ImageOperations.BFS(map, 0, 0, new List<(int, int)> { (1, 0), (0, -1), (-1, 0) }));

    }
}

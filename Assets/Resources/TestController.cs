using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    int i = 0;
    bool isTesting = false;


    [Button]
    public void StartTestingRecognizeByClosestNeighbour()
    {
        int correctResponses = 0;
        foreach ((int, float[]) item in MNISTLoader.instance.testData)
        {
            int modelResponse = Usage.RecognizeByClosestNeighbour(item.Item2);
            int correctResponse = item.Item1;

            if(modelResponse == correctResponse) correctResponses++;

        }

        print($"Model precision: {(float)correctResponses / (float)MNISTLoader.instance.testData.Count * 100f}%");
    }

    [Button]
    public void StartTestingRecognizeByClosestDataCloud()
    {
        int correctResponses = 0;
        foreach ((int, float[]) item in MNISTLoader.instance.testData)
        {
            int modelResponse = Usage.RecognizeByClosestDataCloud(item.Item2);
            int correctResponse = item.Item1;

            if(modelResponse == correctResponse) correctResponses++;

        }

        print($"Model precision: {(float)correctResponses / (float)MNISTLoader.instance.testData.Count * 100f}%");
    }
}

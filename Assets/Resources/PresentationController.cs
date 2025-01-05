using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresentationController : MonoBehaviour
{
    public TMP_Text outputText;
    public TMP_Text vectorText;
    public TextAsset trainingDataFile;
    public RawImage rawImage;
    public Texture2D texture;

    int digit = -1;
    int count = -1;
    int closesNeighbourIndex = -1;

    private void Start()
    {
        texture = new Texture2D(28, 28);

        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                texture.SetPixel(x, y, Color.black);
            }
        }

        texture.Apply();
        rawImage.texture = texture;
    }

    public void UseModel()
    {
        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        float[] map = Usage.TextureToMap(MNISTVisualizer.Instance.texture);
        yield return StartCoroutine(RecognizeByClosestNeighbour(map));
        print(digit);
        outputText.text = $"You drew {digit}!";
    }

    public IEnumerator RecognizeByClosestNeighbour(float[] image)
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
            yield return StartCoroutine(BFS(map, directions[i]));
            count = (28 * 28) - count - whiteCount;
            vector[i] = (float)count / ((28 * 28) - whiteCount);
            string VTText = "[";
            for (int j = 0; j < vector.Length; j++)
            {
                VTText += Math.Round(vector[j], 2).ToString() + ", "; 
            }
            VTText += "]";
            vectorText.text = VTText;
        }

        MNISTVisualizer.Instance.VisualizeImage(map);

        (int, float) closestNeighbour = (-1, 1f);   //digit,distance

        for(int i = 0; i < MindController.Mind.Count; i++)
        {
            (int, float[]) item = MindController.Mind[i];
            float distance = DataVectorDistance(vector, item.Item2);
            if (distance < closestNeighbour.Item2)
            {
                closestNeighbour.Item1 = item.Item1;
                closestNeighbour.Item2 = distance;
                closesNeighbourIndex = i;
            }
        }

        var knnImage = MNISTLoader.LoadOneData(trainingDataFile, closesNeighbourIndex);
        VisualizeImage(knnImage);

        digit = closestNeighbour.Item1;
    }

    public IEnumerator BFS(MapState[,] gridInput, List<(int, int)> directions)
    {
        MapState[,] grid = (MapState[,])gridInput.Clone();

        int rows = 28;
        int cols = 28;

        List<(int, int)> startPoints = new List<(int, int)>
        {
            (0,0),
            (27,0),
            (27,27),
            (0,27),
        };

        Queue<(int, int)> queue = new Queue<(int, int)>();
        foreach (var (startX, startY) in startPoints)
        {
            if (startX >= 0 && startX < rows && startY >= 0 && startY < cols && grid[startX, startY] == MapState.blank)
            {
                queue.Enqueue((startX, startY));
                grid[startX, startY] = MapState.marker;
            }
        }

        int count2 = 0;
        int counter = 0;
        while (queue.Count > 0)
        {
            counter++;
            var (x, y) = queue.Dequeue();
            count2++;

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y - dy;

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && grid[nx, ny] == MapState.blank)
                {
                    queue.Enqueue((nx, ny));
                    grid[nx, ny] = MapState.marker;
                }
            }

            if(counter%3 == 0)
            {
                yield return null;
                MNISTVisualizer.Instance.VisualizeImage(grid);
            }
        }

        count = count2;
    }

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

    public void VisualizeImage(float[] image)
    {
        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                float pixelValue = image[y * 28 + x];

                texture.SetPixel(x, 27 - y, Color.HSVToRGB(0, 0, pixelValue));
            }
        }

        texture.Apply();
    }

}

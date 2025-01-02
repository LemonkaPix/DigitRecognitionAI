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

    public static IEnumerator BFS(bool[,] grid, int startX, int startY, List<(int, int)> directions)
    {
        int rows = 28;
        int cols = 28;

        if (startX < 0 || startX >= rows || startY < 0 || startY >= cols || grid[startX, startY])
        {
            yield break;
        }

        int count = 0;
        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((startX, startY));
        grid[startX, startY] = true; // Mark as visited

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            count++;

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && !grid[nx, ny])
                {
                    queue.Enqueue((nx, ny));
                    grid[nx, ny] = true; // Mark as visited
                }
            }
            yield return new WaitForSeconds(.03f);
            //print("TICK");
            MNISTVisualizer.Instance.VisualizeImage(grid);
        }

        print(count);
    }

    [Button]
    public void test()
    {
        var map = FloatArrayToBoolArray(MNISTLoader.instance.trainingData[0]);
        MNISTVisualizer.Instance.VisualizeImage(map);
        StartCoroutine(BFS(map, 0, 27, new List<(int, int)> { (1, 0), (0, -1), (0, 1) }));

    }
}

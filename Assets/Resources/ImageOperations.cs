using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MapState
{
    blank = 0,
    normal = 1,
    marker = 2
}


public class ImageOperations : MonoBehaviour
{
    public static MapState[,] FloatArrayToMapState(float[] image)
    {
        MapState[,] map = new MapState[28, 28];

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                if (image[y * 28 + x] < .5f)
                {
                    map[x, y] = MapState.blank;
                }
                else
                {
                    map[x, y] = MapState.normal;
                }
                //if (image[y * 28 + x] > 0)
                //{
                //    map[x, y] = MapState.normal;
                //}

            }
        }

        return map;
    }

    public static int BFS(MapState[,] gridInput, List<(int, int)> directions)
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

        int count = 0;

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            count++;

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
            //MNISTVisualizer.Instance.VisualizeImage(grid);
        }

        return count;
    }

    public static int CountState(MapState[,] array, MapState state)
    {
        int count = 0;

        for (int i = 0; i < array.GetLength(0); i++) 
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j] == state)
                {
                    count++;
                }
            }
        }

        return count;
    }


}

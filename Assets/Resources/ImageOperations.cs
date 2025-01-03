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
            }
        }

        return map;
    }

    public static IEnumerator BFS(MapState[,] grid, int startX, int startY, List<(int, int)> directions)
    {
        int rows = 28;
        int cols = 28;

        if (startX < 0 || startX >= rows || startY < 0 || startY >= cols || grid[startX, startY] != MapState.blank)
        {
            yield break;
        }

        int count = 0;
        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((startX, startY));
        grid[startX, startY] = MapState.marker; // Mark as visited

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            count++;

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && grid[nx, ny] == MapState.blank)
                {
                    queue.Enqueue((nx, ny));
                    grid[nx, ny] = MapState.marker; // Mark as visited
                }
            }
            yield return new WaitForSeconds(.01f);
            //print("TICK");
            MNISTVisualizer.Instance.VisualizeImage(grid);
        }

        print(count);
    }

}

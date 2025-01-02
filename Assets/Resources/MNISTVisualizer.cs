using UnityEngine;

public class MNISTVisualizer : MonoBehaviour
{
    public MNISTLoader mnistLoader;
    public GameObject pixelPrefab;
    public float spacing = 1f;

    void Start()
    {
        VisualizeImage(mnistLoader.trainingData[0]);
    }

    void VisualizeImage(float[] image)
    {
        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                float pixelValue = image[y * 28 + x];
                GameObject pixel = Instantiate(pixelPrefab, new Vector3(x * spacing, y * spacing, 0), Quaternion.identity);
                pixel.GetComponent<SpriteRenderer>().color = new Color(pixelValue, pixelValue, pixelValue);
            }
        }
    }
}
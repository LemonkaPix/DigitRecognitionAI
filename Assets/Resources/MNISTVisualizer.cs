using UnityEngine;
using UnityEngine.UI;

public class MNISTVisualizer : MonoBehaviour
{
    public static MNISTVisualizer Instance;

    public MNISTLoader mnistLoader;

    public RawImage rawImage;

    private Texture2D texture;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        VisualizeImage(mnistLoader.trainingData[0]);
    }

    public void VisualizeImage(float[] image)
    {
        texture = new Texture2D(28, 28);

        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                float pixelValue = image[y * 28 + x];

                texture.SetPixel(x, y, Color.HSVToRGB(0,0,pixelValue));

                //GameObject pixel = Instantiate(pixelPrefab, new Vector3(x * spacing, y * spacing, 0), Quaternion.identity);
                //pixel.GetComponent<SpriteRenderer>().color = new Color(pixelValue, pixelValue, pixelValue);
            }
        }

        texture.Apply();
        rawImage.texture = texture;
    }
    public void VisualizeImage(bool[,] image)
    {
        texture = new Texture2D(28, 28);

        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                float pixelValue = 0;

                if (image[x,y]) pixelValue = 1f;

                texture.SetPixel(x, y, Color.HSVToRGB(0,0,pixelValue));

                //GameObject pixel = Instantiate(pixelPrefab, new Vector3(x * spacing, y * spacing, 0), Quaternion.identity);
                //pixel.GetComponent<SpriteRenderer>().color = new Color(pixelValue, pixelValue, pixelValue);
            }
        }

        texture.Apply();
        rawImage.texture = texture;
    }
}
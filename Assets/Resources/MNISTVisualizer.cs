using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class MNISTVisualizer : MonoBehaviour
{
    public static MNISTVisualizer Instance;

    public MNISTLoader mnistLoader;

    public RawImage rawImage;

    public Texture2D texture;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        //VisualizeImage(mnistLoader.trainingData[4].Item2);
        texture = new Texture2D(28, 28);

        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                texture.SetPixel(i, j, Color.black);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();
        rawImage.texture = texture;

    }

    [Button]
    public void ClearTexture()
    {
        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                texture.SetPixel(i, j, Color.black);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

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

                texture.SetPixel(x, 27-y, Color.HSVToRGB(0,0,pixelValue));

                //GameObject pixel = Instantiate(pixelPrefab, new Vector3(x * spacing, y * spacing, 0), Quaternion.identity);
                //pixel.GetComponent<SpriteRenderer>().color = new Color(pixelValue, pixelValue, pixelValue);
            }
        }

        texture.Apply();
        rawImage.texture = texture;
    }
    public void VisualizeImage(MapState[,] image)
    {
        texture = new Texture2D(28, 28);

        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                switch (image[x, y])
                {
                    case MapState.blank:
                        texture.SetPixel(x, 27-y, Color.black);
                        break;
                    case MapState.normal:
                        texture.SetPixel(x, 27-y, Color.white);
                        break;
                    case MapState.marker:
                        texture.SetPixel(x, 27-y, Color.red);
                        break;
                    default:
                        texture.SetPixel(x, 27-y, Color.black);
                        break;
                }


                //GameObject pixel = Instantiate(pixelPrefab, new Vector3(x * spacing, y * spacing, 0), Quaternion.identity);
                //pixel.GetComponent<SpriteRenderer>().color = new Color(pixelValue, pixelValue, pixelValue);
            }
        }

        texture.Apply();
        rawImage.texture = texture;
    }

    void Update()
    {
        // Przyk³ad zmiany koloru piksela po klikniêciu mysz¹
        if (Input.GetMouseButton(0))
        {

            // Pobranie pozycji kursora myszy
            Vector2 mousePosition = Input.mousePosition;

            // Przeliczenie pozycji kursora na wspó³rzêdne tekstury
            Vector2 localCursor;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, mousePosition, null, out localCursor);

            //print(mousePosition);
            //print(localCursor);

            int x = (int)((localCursor.x + rawImage.rectTransform.rect.width / 2) * (texture.width / rawImage.rectTransform.rect.width));
            int y = (int)((localCursor.y + rawImage.rectTransform.rect.height / 2) * (texture.height / rawImage.rectTransform.rect.height));

            //print($"{x} {y}");


            // Zmiana koloru piksela
            if(x > 0 && y > 0 && x < 26 && y < 26)
            {
                texture.SetPixel(x, y, Color.white);
                texture.SetPixel(x+1, y, Color.white);
                texture.SetPixel(x, y+1, Color.white);
                texture.SetPixel(x+1, y+1, Color.white);
            }


            // Zastosowanie zmian w teksturze
            texture.Apply();

        }
    }

    [Button]
    public void test()
    {
        texture = new Texture2D(28, 28);

        texture.filterMode = FilterMode.Point;

        texture.SetPixel(0, 0, Color.green);


        texture.Apply();
        rawImage.texture = texture;

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Chalet : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private List<Vector2Int> exceptIndex = new List<Vector2Int>();

    [SerializeField]
    private Image targetImage;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    private Color[] color;

    [SerializeField]
    private ColorPicker colorPicker;

    private void Awake()
    {
        color = new Color[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (exceptIndex.Contains(new Vector2Int(i, j)))
                {
                    color[i + j * width] = new Color(0, 0, 0, 0);
                    continue;
                }

                color[i + j * width] = Color.white;
            }
        }

        SetPixel(color);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 leftdown = (Vector2)targetImage.rectTransform.position + targetImage.rectTransform.rect.min * transform.root.localScale;
        Vector2 rightUp = (Vector2)targetImage.rectTransform.position + targetImage.rectTransform.rect.max * transform.root.localScale;
        Vector2 leftUp = new Vector2(leftdown.x, rightUp.y);
        Vector2 rightDown = new Vector2(rightUp.x, leftdown.y);

        float heightSpacing = (leftUp.y - leftdown.y) / height;// * transform.root.localScale.y;
        float widthSpacing = (rightUp.x - leftUp.x) / width;// * transform.root.localScale.y;

        Vector2 mousePosition = (Vector2)Input.mousePosition;


        Vector2 size = new Vector2(targetImage.rectTransform.rect.width, targetImage.rectTransform.rect.height) * transform.root.localScale;
        Rect rect = new Rect((Vector2)targetImage.rectTransform.position - size * 0.5f, size);
        if (rect.Contains(mousePosition))
        {
            mousePosition = mousePosition - ((Vector2)targetImage.rectTransform.position - size * 0.5f);
            int x = (int)(mousePosition.x / widthSpacing);
            int y = (int)(mousePosition.y / heightSpacing);

            if(exceptIndex.Contains(new Vector2Int(x, y)))
            {
                return;
            }

            if (targetImage.sprite != null)
            {
                color = targetImage.sprite.texture.GetPixels();
                int index = x + y * width;
                color[index] = colorPicker.SelectedColor;
            }
            else
            {
                color = new Color[width * height];

                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        if (exceptIndex.Contains(new Vector2Int(i, j)))
                        {
                            color[i + j * width] = new Color(0, 0, 0, 0);
                            continue;
                        }

                        color[i + j * width] = Color.white;
                    }
                }

                int index = x + y * width;
                color[index] = colorPicker.SelectedColor;
            }


            SetPixel(color);
        }
    }

    public void SetPixel(Color[] colors)
    {
        Texture2D newTexture = new Texture2D(width, height);
        newTexture.filterMode = FilterMode.Point;
        newTexture.SetPixels(colors);
        newTexture.Apply();
        targetImage.sprite = Sprite.Create(newTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 leftdown = (Vector2)targetImage.rectTransform.position + targetImage.rectTransform.rect.min * transform.root.localScale;
        Vector2 rightUp = (Vector2)targetImage.rectTransform.position + targetImage.rectTransform.rect.max * transform.root.localScale;
        Vector2 leftUp = new Vector2(leftdown.x, rightUp.y);
        Vector2 rightDown = new Vector2(rightUp.x, leftdown.y);
        //Gizmos.DrawLine(leftdown,  leftUp);
        //Gizmos.DrawLine(leftUp, rightUp);
        //Gizmos.DrawLine(rightUp, rightDown);
        //Gizmos.DrawLine(leftdown, rightDown);

        float heightSpacing = (leftUp.y - leftdown.y) / height;// * transform.root.localScale.y;
        float widthSpacing = (rightUp.x - leftUp.x) / width;// * transform.root.localScale.y;
        for (int x = 0; x <= width; x++)
        {
            for(int y = 0; y <= height; y++)
            {
                Gizmos.DrawLine(leftdown + Vector2.up * (y * heightSpacing), rightDown + Vector2.up * (y * heightSpacing));

            }

            Gizmos.DrawLine(leftdown + Vector2.right * (x * widthSpacing), leftUp + Vector2.right * (x * widthSpacing));
        }

        Vector2 mousePosition = (Vector2)Input.mousePosition;


        Vector2 size = new Vector2(targetImage.rectTransform.rect.width, targetImage.rectTransform.rect.height) * transform.root.localScale;
        Rect rect = new Rect((Vector2)targetImage.rectTransform.position - size * 0.5f,  size);
        if(rect.Contains(mousePosition))
        {
            mousePosition = mousePosition - ((Vector2)targetImage.rectTransform.position - size * 0.5f);
            int x = (int)(mousePosition.x / widthSpacing);
            int y = (int)(mousePosition.y / heightSpacing);
            Debug.Log(x + " | " + y);
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnPointerDown(eventData);
    }
}

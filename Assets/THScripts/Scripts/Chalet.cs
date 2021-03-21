using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Chalet : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private int spreadCount = 4;
    [SerializeField]
    private float spreadTime = 1f;
    [SerializeField]
    private List<Vector2Int> exceptIndex = new List<Vector2Int>();
    [SerializeField]
    private Image targetImage;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    [SerializeField]
    private ColorPicker colorPicker;

    [SerializeField]
    private GameObject applyButton;

    private int selectedColorCount;
    public int SelectedColorCount
    {
        get { return selectedColorCount; }
    }

    private int cost;
    public int Cost
    {
        get { return cost; }
    }
    private int pixelCount;
    public int PixelCount
    {
        get { return pixelCount; }
    }

    private Color[,] colors;
    private Color lastColor;

    [SerializeField]
    private Cylinder cylinder;

    

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        Color[] defaultColors = new Color[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (exceptIndex.Contains(new Vector2Int(i, j)))
                {
                    defaultColors[i + j * width] = new Color(0, 0, 0, 0);
                    continue;
                }

                defaultColors[i + j * width] = Color.clear;
            }
        }

        SetPixel(defaultColors);

        colors = new Color[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                colors[x, y] = Color.clear;
            }
        }
        selectedColorCount = 1;
        cost = 0;
        pixelCount = 0;
        lastColor = Color.clear;
        isPressed = false;
    }

    private void OnDisable()
    {
        
    }

    public void Apply()
    {
        if (UserData.Instance == null)
            return;

        if(UserData.Instance.GetUserData.Cost >= cost)
            UserData.Instance.GetUserData.Cost -= cost;


        cylinder.Reset();
        for (int i = 0; i < width; i++)
        {
            for(int j =0; j< height; j++)
            {
                if (colors[i, j] == Color.clear)
                    continue;

                Point pos;
                pos.x = i;
                pos.y = j;
                cylinder.Creat_Col(pos, colors[i, j]);
            }
        }

        Init();
    }

    private void Update()
    {
        if (UserData.Instance == null)
            return;

        if (UserData.Instance.GetUserData.Cost >= cost)
        {
            applyButton.SetActive(true);
        }
        else
        {
            applyButton.SetActive(false);
        }

        if(pixelCount == 0 && selectedColorCount == 0)
        {
            applyButton.SetActive(false);
        }
    }



    private List<Vector2Int> pressedIndexes;
    private bool isPressed = false;

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
        Color[] newColors;

        if (rect.Contains(mousePosition))
        {
            mousePosition = mousePosition - ((Vector2)targetImage.rectTransform.position - size * 0.5f);
            int x = (int)(mousePosition.x / widthSpacing);
            int y = (int)(mousePosition.y / heightSpacing);

            if(!isPressed)
            {
                isPressed = true;
                if(pressedIndexes == null)
                {
                    pressedIndexes = new List<Vector2Int>();
                }
            }

            if (pressedIndexes.Contains(new Vector2Int(x, y)))
            {
                return;
            }

            if (exceptIndex.Contains(new Vector2Int(x, y)))
            {
                return;
            }

            
            if (targetImage.sprite != null)
            {
                newColors = targetImage.sprite.texture.GetPixels();
            }
            else
            {
                newColors = new Color[width * height];

                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        if (exceptIndex.Contains(new Vector2Int(i, j)))
                        {
                            newColors[i + j * width] = new Color(0, 0, 0, 0);
                            continue;
                        }

                        newColors[i + j * width] = Color.white;
                    }
                }

            }

            int index = x + y * width;
            pressedIndexes.Add(new Vector2Int(x, y));

            if(newColors[index] == Color.clear)
            {
                newColors[index] = Color.white;
            }

            Color newColor = (newColors[index] * colorPicker.SelectedColor) / 1f;
            

            newColor = new Color(Mathf.Max(0.3f, newColor.r), Mathf.Max(0.3f, newColor.g), Mathf.Max(0.3f, newColor.b), 1);
            newColors[index] = newColor;
            colors[x, y] = newColor;

            if (lastColor == Color.clear)
            {
                pixelCount = 0;
            }
            else if(lastColor != colorPicker.SelectedColor)
            {
                selectedColorCount++;
                pixelCount = 0;
            }

            SetPixel(newColors);
            pixelCount++;
            int newCost = (int)(Mathf.Pow(1 + selectedColorCount, 1.08f) * (5 * Mathf.Pow(1.02f, pixelCount)));
            Debug.Log(selectedColorCount + " | " + "1.08f" + " | " + "5" + " | " + "1.02f" + " | " + pixelCount + " | " + newCost);
            cost += newCost;

            if (lastColor != colorPicker.SelectedColor)
            {
                lastColor = colorPicker.SelectedColor;
            }

            StartCoroutine(SpreadColor(new Vector2Int(x, y)));
        }
    }

    public Vector2Int[] GetRandomIndex(Vector2Int index)
    {
        Vector2Int[] result = new Vector2Int[] {index + new Vector2Int(1,0), index + new Vector2Int(-1, 0), index + new Vector2Int(0, 1), index + new Vector2Int(0, -1) };
        ShuffleArray<Vector2Int>(result);
        return result;
    }


    IEnumerator SpreadColor(Vector2Int index) 
    {
        Color[] newColors;

        int currentSpreadCount = spreadCount;
        Queue<Vector2Int> addedIndex = new Queue<Vector2Int>();
        Vector2Int[] nextIndex = GetRandomIndex(index);
        foreach(Vector2Int nexti in nextIndex)
        {
            addedIndex.Enqueue(nexti);
        }

        List<Vector2Int> visitedIndex = new List<Vector2Int>();
        visitedIndex.Add(index);
        while(addedIndex.Count > 0 && currentSpreadCount > 0)
        {
            Vector2Int current = addedIndex.Dequeue();

            Vector2Int[] nextnextIndex = GetRandomIndex(current);
            foreach (Vector2Int nexti in nextnextIndex)
            {
                addedIndex.Enqueue(nexti);
            }

            if (visitedIndex.Contains(current))
            {
                continue;
            }
            
            visitedIndex.Add(current);

            if(current.x >= width || current.x < 0)
            {
                continue;
            }

            if (current.y >= height || current.y < 0)
            {
                continue;
            }

            if(exceptIndex.Contains(current))
            {
                continue;
            }

            


            if (targetImage.sprite != null)
            {
                newColors = targetImage.sprite.texture.GetPixels();
            }
            else
            {
                newColors = new Color[width * height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (exceptIndex.Contains(new Vector2Int(i, j)))
                        {
                            newColors[i + j * width] = new Color(0, 0, 0, 0);
                            continue;
                        }

                        newColors[i + j * width] = Color.white;
                    }
                }

            }

            if (newColors[current.x + current.y * width] == Color.clear)
            {
                newColors[current.x + current.y * width] = Color.white;
            }

            Color newColor = (newColors[current.x + current.y * width] * colorPicker.SelectedColor) / 1f;
            Color prevColor = new Color(newColors[current.x + current.y * width].r / colorPicker.SelectedColor.r, newColors[current.x + current.y * width].g / colorPicker.SelectedColor.g, newColors[current.x + current.y * width].b / colorPicker.SelectedColor.b, 1);
            if (prevColor == colorPicker.SelectedColor)
            {
                continue;
            }
            
            newColor = new Color(Mathf.Max(0.3f, newColor.r), Mathf.Max(0.3f, newColor.g), Mathf.Max(0.3f, newColor.b), 1);
            newColors[current.x + current.y * width] = newColor;
            colors[current.x, current.y] = newColor;

            SetPixel(newColors);
            currentSpreadCount--;

            yield return new WaitForSeconds(spreadTime);
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

    public static void ShuffleArray<T>(T[] array)
    {
        int random1;
        int random2;

        T tmp;

        for (int index = 0; index < array.Length; ++index)
        {
            random1 = UnityEngine.Random.Range(0, array.Length);
            random2 = UnityEngine.Random.Range(0, array.Length);

            tmp = array[random1];
            array[random1] = array[random2];
            array[random2] = tmp;
        }
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

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        pressedIndexes.Clear();
    }
}

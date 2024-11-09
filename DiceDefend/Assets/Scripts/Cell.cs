using Unity.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isOccupied;
    public int GridX {get {return gridX;}}
    public int GridY {get {return gridY;}}

    [SerializeField]
    private Color selectedColor, normalColor;
    [SerializeField]
    private int gridX, gridY;
    public void Init(int x, int y)
    {
        gridX = x; gridY = y;
        GetComponent<SpriteRenderer>().color = normalColor;
    }

    public void OnSelected()
    {
        GetComponent<SpriteRenderer>().color = selectedColor;
    }
    public void OnDeSelected()
    {
        GetComponent<SpriteRenderer>().color = normalColor;
    }

    public void OnRelease()
    {
        isOccupied = false;
        GetComponent<SpriteRenderer>().color = normalColor;
    }
    public void OnOccupy()
    {
        isOccupied = true;
        GetComponent<SpriteRenderer>().color = normalColor;
    }
}

using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField]
    private GameObject cellPrefab; 
    [SerializeField]
    private int gridWidth, gridHeight;
    [SerializeField]
    private float cellSize = 0.5f;
    private Cell targetCell = null;

    void Awake() 
    {
        Instance = this;
    }

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int x = -gridWidth / 2; x < gridWidth / 2; x++)
        {
            for (int y = -gridHeight / 2; y < gridHeight / 2; y++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize, y * cellSize, 0);
                var cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                cell.GetComponent<Cell>().Init(x, y);

                cell.transform.SetParent(this.transform);
            }
        }
    }
    public void TryPlaceDice(Dice dice, Vector3 position)
    {
        targetCell?.OnRelease();
        Collider2D[] hits = Physics2D.OverlapPointAll(position);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Cell"))
            {
                Cell cell = hit.GetComponent<Cell>();
                if (!cell.isOccupied)
                {
                    targetCell = cell;
                    targetCell.OnSelected();
                    return;
                }
            }
        }
    }
    public void PlaceDice(Dice dice)
    {
        if (targetCell)
        {
            Debug.Log(targetCell.GridX + "-" + targetCell.GridY);
            targetCell.OnOccupy();
            dice.transform.position = targetCell.transform.position;
        }
        else
        {
            Debug.Log("No cell");
        }
    }
    public bool IsSameCell(Cell cellA, Cell cellB)
    {
        return (cellA.GridX == cellB.GridX) && (cellA.GridY == cellB.GridY);
    }
}

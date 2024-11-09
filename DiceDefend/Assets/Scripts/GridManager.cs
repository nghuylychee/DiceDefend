using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public Cell[,] CellGrid;
    [SerializeField]
    private Transform cellSpawnPoint; 

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
    public void Init() 
    {
        CreateGrid();
    }
    public void CreateGrid()
    {
        CellGrid = new Cell[gridWidth, gridHeight];
        Vector3 startPosition = cellSpawnPoint.position; // Lấy vị trí cụ thể từ scene

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector3 cellPosition = new Vector3(startPosition.x + x * cellSize, (gridHeight - y) * cellSize - startPosition.y, 0);
                var cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                cell.GetComponent<Cell>().Init(x, y);
                cell.transform.SetParent(this.transform);
                CellGrid[x, y] = cell.GetComponent<Cell>();
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
            targetCell.OnOccupy();
            dice.transform.position = targetCell.transform.position;
        }
        else
        {
            Debug.Log("No cell");
        }
    }
    public void PlaceDice(Dice dice, int gridX, int gridY)
    {
        dice.transform.position = CellGrid[gridX, gridY].transform.position;
    }
    public bool IsSameCell(Cell cellA, Cell cellB)
    {
        return (cellA.GridX == cellB.GridX) && (cellA.GridY == cellB.GridY);
    }
}

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
    private float cellSize;
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

    public void CheckTargetGrid(Dice dice, Vector3 position)
    {
        targetCell?.OnDeSelected();
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
    public void PlaceDiceByTarget(Dice dice)
    {
        var currentGridX = dice.GridX;
        var currentGridY = dice.GridY;
        
        if (targetCell)
        {
            CellGrid[currentGridX, currentGridY].OnRelease();
            PlaceDice(dice, targetCell.GridX, targetCell.GridY);
        }
        else
        {
            Debug.Log("No cell");
        }
    }
    public void PlaceDice(Dice dice, int gridX, int gridY)
    {
        if (!CellGrid[gridX, gridY].isOccupied)
        {
            CellGrid[gridX, gridY].OnOccupy();
            dice.GridX = gridX;
            dice.GridY = gridY;
            dice.transform.position = CellGrid[gridX, gridY].transform.position;
        }
        else
        {
            Debug.Log("Not available, trying to place at the nearest cell!");
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (!CellGrid[x, y].isOccupied)
                    {
                        Debug.Log(x + "-" + y);
                        CellGrid[x, y].OnOccupy();
                        dice.GridX = x;
                        dice.GridY = y;
                        dice.transform.position = CellGrid[x, y].transform.position;
                        return;
                    }
                }
            }
        }
    }
    public void ReleaseGrid(int gridX, int gridY)
    {
        CellGrid[gridX, gridY].OnRelease();
    }
}

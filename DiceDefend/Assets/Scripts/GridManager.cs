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
        
        // Kiểm tra số lượng child có đủ không
        int expectedChildCount = gridWidth * gridHeight;
        if (this.transform.childCount < expectedChildCount)
        {
            Debug.LogError($"Not enough child cells! Expected: {expectedChildCount}, Found: {this.transform.childCount}");
            return;
        }

        // Gán các cell từ child objects vào grid
        for (int i = 0; i < this.transform.childCount && i < expectedChildCount; ++i)
        {
            int x = i % gridWidth;
            int y = i / gridWidth;
            
            // Kiểm tra bounds trước khi gán
            if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
            {
                CellGrid[x, y] = this.transform.GetChild(i).GetComponent<Cell>();
                this.transform.GetChild(i).GetComponent<Cell>().Init(x, y);
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
            // Kiểm tra bounds trước khi truy cập CellGrid
            if (IsValidGridPosition(currentGridX, currentGridY))
            {
                CellGrid[currentGridX, currentGridY].OnRelease();
            }
            PlaceDice(dice, targetCell.GridX, targetCell.GridY);
        }
        else
        {
            Debug.Log("No cell");
        }
    }
    public void PlaceDice(Dice dice, int gridX, int gridY)
    {
        // Kiểm tra bounds trước khi truy cập CellGrid
        if (!IsValidGridPosition(gridX, gridY))
        {
            Debug.LogError($"Invalid grid position: ({gridX}, {gridY})");
            return;
        }

        if (!CellGrid[gridX, gridY].isOccupied)
        {
            CellGrid[gridX, gridY].OnOccupy();
            dice.GridX = gridX;
            dice.GridY = gridY;

            var pos = CellGrid[gridX, gridY].transform.position;
            dice.transform.position = new Vector3(pos.x, pos.y, 0);
        }
        else
        {
            if (dice.GridX != -1 && dice.GridY != -1)
            {
                Debug.Log("Back to old grid");
                dice.transform.position = CellGrid[dice.GridX, dice.GridY].transform.position;
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
    }
    public void ReleaseGrid(int gridX, int gridY)
    {
        if (IsValidGridPosition(gridX, gridY))
        {
            CellGrid[gridX, gridY].OnRelease();
        }
    }

    private bool IsValidGridPosition(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }
}

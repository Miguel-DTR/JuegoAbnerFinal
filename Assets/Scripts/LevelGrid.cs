using System; //** nuevo **//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
	private GridSystem<GridObject> gridSystem;
	
	[SerializeField] private Transform gridDebugObjectPrefab;
	[SerializeField] private int width;
	[SerializeField] private int height;
	[SerializeField] private float cellSize;

	// eventos
	public event EventHandler OnAnyUnitMovedGridPosition;
	
	// singleton
    public static LevelGrid Instance {get; private set;}

    void Awake()
    {
        if(Instance != null)
        {
            // Hay más de un level grid, destruimos este
            Debug.LogError("Hay más de un LevelGrid en la escena");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Creamos nuestra matriz de nivel
        gridSystem = new GridSystem<GridObject>(
		width,
		height,
		cellSize,
		(GridSystem<GridObject> g, GridPosition gridPosition)
		=>
		new GridObject(g, gridPosition));

        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    
    public void AddUnitAtGridPosition(GridPosition pos, Unit unidad)
    {
        // Obtengo el cuadrito que está en la posición
        GridObject casilla = gridSystem.GetGridObject(pos);
        // En esa casilla agrego a la unidad
        casilla.AddUnit(unidad);
    }

    
    public void RemoveUnitAtGridPosition(GridPosition pos, Unit unidad)
    {
        GridObject casilla = gridSystem.GetGridObject(pos);
        casilla.RemoveUnit(unidad);
    }

    
    public List<Unit> GetUnitListAtGridPosition(GridPosition pos)
    {
        GridObject casilla = gridSystem.GetGridObject(pos);
        return casilla.GetUnitList();
    }

    
    public void UnitMovedGridPosition(
        Unit unidad, GridPosition origen, GridPosition destino)
    {
        RemoveUnitAtGridPosition(origen, unidad);
        AddUnitAtGridPosition(destino, unidad);
        
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => 
		gridSystem.GetGridPosition(worldPosition);
    
    public Vector3 GetWorldPosition(GridPosition posGrid) =>
        gridSystem.GetWorldPosition(posGrid);

    public bool IsValidGridPosition(GridPosition pos) =>
        gridSystem.IsValidGridPosition(pos);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();
    
	
	public bool HasAnyUnitOnGridPosition(GridPosition posicion)
	{
		GridObject objeto = gridSystem.GetGridObject(posicion);
		return objeto.HasAnyUnit();
	}
	
	public Unit GetUnitAtGridPosition(GridPosition pos)
	{
		GridObject objeto = gridSystem.GetGridObject(pos);
		return objeto.GetUnit();
	}

}

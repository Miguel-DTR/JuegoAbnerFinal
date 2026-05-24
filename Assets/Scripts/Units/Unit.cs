using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
	// constantes
	private const int ACTION_POINTS_MAX = 2;
	
	// eventos
	public static event EventHandler OnAnyActionPointsChanged;
	public static event EventHandler OnAnyUnitSpawned;
	public static event EventHandler OnAnyUnitDead;

	// variables privadas
    [SerializeField] private bool isEnemy;
	
	private GridPosition gridPosition;
	private int actionPoints = ACTION_POINTS_MAX;
	private BaseAction[] baseActionArray;
	private HealthSystem healthSystem;
	
	private void Awake()
	{
		healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>(); 
        
	}
	
	private void Start()
	{
		// leemos en donde está la unidad
		gridPosition = 
			LevelGrid.Instance.GetGridPosition(transform.position);
		
		//Le avisamos al grid del nivel que tiene una unidad en esa posición
		LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        // Evento de cuando se muera la unidad
		healthSystem.OnDead += HealthSystem_OnDead;
		
        // Avisamos que una unidad nueva fue creada
		OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
	}
	
	void Update()
	{
		// vemos donde está ahorita la unidad
		GridPosition pos = 
			LevelGrid.Instance.GetGridPosition(
				transform.position);
			
		// preguntamos si esta posición es diferente que la guardada
		if(pos != gridPosition)
		{
			// La unidad cambió de casilla
			GridPosition oldPos = gridPosition;
			gridPosition = pos;
			
			LevelGrid.Instance.UnitMovedGridPosition(
				this, oldPos, pos);
		}
	}
	
    
    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }
    

	public GridPosition GetGridPosition()
	{
		return gridPosition;
	}
	
	public BaseAction[] GetBaseActionArray()
	{
		return baseActionArray;
	}

	public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    
	public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        } else
        {
            return false;
        }
    }

    
	public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        } else
        {
            return false;
        }
    }

    
    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
    
    
    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

}

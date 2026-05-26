using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAction : BaseAction
{

    public static event EventHandler OnAnyHealthAction;

    public event EventHandler OnHealthActionStarted;
    public event EventHandler OnHealthActionCompleted;


    private enum State
    {
        SwingingHealthBeforeHit,
        SwingingHealthAfterHit,
    }

	[SerializeField]private int healthCure = 50;
	[SerializeField]private int maxHealthDistance = 1;
	
    private State state;
    private float stateTimer;
    private Unit targetUnit;


    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingHealthBeforeHit:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingHealthAfterHit:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingHealthBeforeHit:
                state = State.SwingingHealthAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
				
                targetUnit.Cure(healthCure);
                Debug.Log("activamos la cura");
				
                OnAnyHealthAction?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingHealthAfterHit:
                OnHealthActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Health";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200 - (targetUnit.GetHealth() * 2),
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxHealthDistance; x <= maxHealthDistance; x++)
        {
            for (int z = -maxHealthDistance; z <= maxHealthDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() != unit.IsEnemy())
                {
                    //ambos son del mismo equipo si se pueden atacar
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingHealthBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnHealthActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public int GetMaxHealthDistance()
    {
        return maxHealthDistance;
    }

}

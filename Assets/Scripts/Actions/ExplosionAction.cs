using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAction : BaseAction
{

    public static event EventHandler OnAnyExplosionHit;

    public event EventHandler OnExplosionActionStarted;
    public event EventHandler OnExplosionActionCompleted;

    //referencias
    HealthSystem healthSystem;


    private enum State
    {
        BeforeExplosion,
        AfterExplosion,
    }

	[SerializeField]private int explosionDamage = 50;
	[SerializeField]private int maxExplosionDistance = 1;
	
    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private int explosionDamageToSelf = 100;


    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.BeforeExplosion:

                healthSystem = GetComponent<HealthSystem>();
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.AfterExplosion:
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
            case State.BeforeExplosion:
                state = State.AfterExplosion;
                float afterExplosionStateTime = 0.5f;
                stateTimer = afterExplosionStateTime;

                GridPosition targetGridPosition = targetUnit.GetGridPosition();

                for (int x = -1; x <= 1; x++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        GridPosition offsetPosition = new GridPosition(x, z);
                        GridPosition testGridPosition = targetGridPosition + offsetPosition;

                        if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                        {
                            continue;
                        }

                        if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                        {
                            continue;
                        }

                        Unit unitToDamage = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                        if (unitToDamage.IsEnemy() == unit.IsEnemy())
                        {
                            continue;
                        }

                        unitToDamage.Damage(explosionDamage);
                    }
                }

                OnAnyExplosionHit?.Invoke(this, EventArgs.Empty);
                healthSystem.Damage(explosionDamageToSelf);
                break;

            case State.AfterExplosion:
                OnExplosionActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Explosion";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 180,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxExplosionDistance; x <= maxExplosionDistance; x++)
        {
            for (int z = -maxExplosionDistance; z <= maxExplosionDistance; z++)
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

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on same 'team'
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

        state = State.BeforeExplosion;
        float beforeExplosionStateTime = 0.7f;
        stateTimer = beforeExplosionStateTime;

        OnExplosionActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public int GetMaxExplosionDistance()
    {
        return maxExplosionDistance;
    }

}

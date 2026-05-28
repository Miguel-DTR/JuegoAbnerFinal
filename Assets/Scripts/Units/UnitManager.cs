using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitManager : MonoBehaviour
{

    public static UnitManager Instance { get; private set; }

    [SerializeField] EnemyAI enemyAi;
    [SerializeField] GameObject gameOverPanel;
    //[SerializeField] GameObject winPanel;
    //[SerializeField] GameObject canvaInterfaz;    
    [SerializeField] private string perderEscenaName;
    [SerializeField] private string ganarEscenaName;

    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        } else
        {
            friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
            if(IsWin())
            {
                SceneManager.LoadScene(ganarEscenaName);
            }
        }
        else
        {
            friendlyUnitList.Remove(unit);
            if(IsGameOver())
            {
                SceneManager.LoadScene(perderEscenaName);
            }
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

    public Unit GetFirstFriendlyUnit()
    {
        if (friendlyUnitList.Count > 0)
        {
            return friendlyUnitList[0];
        }
        else
        {
            return null;
        }
    }

    public void GetFirstEnemyUnit()
    {
        if (enemyUnitList.Count > 0)
        {
            enemyAi.BuscarUnEnemyNuevo();
        }
    }

    private bool IsGameOver()
    {
        if(friendlyUnitList.Count <= 0)
        {
            return true;
        }
        else return false;
    }

    private bool IsWin()
    {
        if(enemyUnitList.Count <= 0)
        {
            return true;
        }
        else return false;
    }


}

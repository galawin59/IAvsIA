using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    static GridManager instance = null;
    public static GridManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        rootTree = new GridData(Application.streamingAssetsPath + "/wall.txt");
       
    }
    public int NbLines = 0;
    public int NbColumns = 0;
    GridData rootTree;


    public void SetIaRef(List<Ia> ias)
    {
        rootTree.SetIaRef(ias);
    }

    public void SetIaRef(Ia ia, int column, int line)
    {
        rootTree.SetIaRef(ia, column, line);
    }

    public bool IsValidPos(int column, int line)
    {
        return column >= 0 && line >= 0 && column < NbColumns && line < NbLines;
    }

    public GridData.Case GetCase(int column, int line)
    {
        if (IsValidPos(column, line))
        {
            return rootTree.Grid[column, line];
        }
        return null;
    }
    public GridData.STATE GetCaseState(int column, int line)
    {
        if (IsValidPos(column, line))
        {
            return rootTree.Grid[column, line].state;
        }
        return GridData.STATE.BUSY;
    }
    public Ia GetCaseIa(int column, int line)
    {
        if (IsValidPos(column, line))
        {
            return rootTree.Grid[column, line].ia;
        }
        return null;
    }

}

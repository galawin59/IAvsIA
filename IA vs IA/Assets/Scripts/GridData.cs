using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GridData
{
    public int NbLine = 0;
    public int NbColumn = 0;
    public Case[,] Grid;

    public enum STATE { EMPTY, BUSY, IA };



    public class Case
    {
        public STATE state;
        public PosGrid PosGrid;
        public Ia ia;
    };

    [System.Serializable]
    public struct PosGrid
    {
        public int column;
        public int line;
    };

    public GridData(int nbLine, int nbColumn)
    {
        NbColumn = nbColumn;
        NbLine = nbLine;
        Grid = new Case[nbColumn, nbLine];

        for (int i = 0; i < nbColumn; i++)
        {
            for (int j = 0; j < nbLine; j++)
            {
                Grid[i, j] = new Case();
                Grid[i, j].PosGrid.line = j;
                Grid[i, j].PosGrid.column = i;
                Grid[i, j].state = STATE.EMPTY;
                Grid[i, j].ia = null;
            }
        }
    }


    public GridData(string path)
    {
        LoadFromFile(path);
    }
    public void SetIaRef(List<Ia> ias)
    {
        foreach (Ia ia in ias)
        {
            Grid[ia.Pos.column, ia.Pos.line].ia = ia;
            Grid[ia.Pos.column, ia.Pos.line].state = STATE.IA;
        }
    }

    public void SetIaRef(Ia ia, int column, int line)
    {
        Grid[column, line].ia = ia;
        if (ia == null)
        {
            Grid[column, line].state = STATE.EMPTY;
        }
        else
        {
            Grid[column, line].state = STATE.IA;
        }
    }

    static public int Distance(PosGrid _posA, PosGrid _posB)
    {
        return Mathf.Abs(_posA.column - _posB.column) + Mathf.Abs(_posA.line - _posB.line);
    }

     public void LoadFromFile(string path)
    {
       
        List<STATE[]> states = new List<STATE[]>();
        StreamReader read = new StreamReader(path);
        int i = 0;
        while (!read.EndOfStream)
        {


            string[] currentLine = read.ReadLine().Split(',');
            states.Add(new STATE[currentLine.Length]) ;
            for (int j = 0; j < currentLine.Length; j++)
            {
                states[i][j] = (STATE)int.Parse(currentLine[j]);
            }
            i++;
        }


        NbColumn = states[0].Length ;
        NbLine = i;
        Grid = new Case[NbColumn, NbLine];

        for (int j = 0; j < NbColumn; j++)
        {
            for (int k = 0; k < NbLine; k++)
            {
                Grid[j, k] = new Case();
                Grid[j, k].PosGrid.line = k;
                Grid[j, k].PosGrid.column = j;
                
                Grid[j, k].state = states[k][j];
                Grid[j, k].ia = null;
            }
        }

    }
}

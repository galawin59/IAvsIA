using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ia
{
    public enum BEHAVIOR
    {
        MELEE,
        DISTANCE,
        SUMMONER,
        TRIKKY,
        HEALER
    }

    [System.Serializable]
    public class IaStats
    {
        public GridData.PosGrid Pos;
        public int MovePoints;
        public int ActionPoints;
        public int Hp;
        public int Damage;
        public BEHAVIOR Behavior;
    }

    public GridData.PosGrid Pos;
    public int MovePoints;
    public int ActionPoints;
    public int Hp;
    public int Damage;
    public int Id;
    public int IdTeam;
    public bool IsAlive;
    public BEHAVIOR Behavior;
    public abstract void Play();
    protected abstract void Think();
    protected abstract void Move();
    protected abstract void Attack();

    public virtual void LoseHp(int damage,Ia target)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            IsAlive = false;
            GridManager.Instance.SetIaRef(null, Pos.column, Pos.line);
         
        }
    }


    protected virtual GridData.PosGrid Dijkstra(GridData.PosGrid _startingPos, GridData.PosGrid _destination, int _movePoints)
    {
       
        GridManager gm = GridManager.Instance;
        // nombre de pas a faire pour atteindre les differentes cases du plateau
        int[,] distances = new int[gm.NbColumns, gm.NbLines];
        for (int i = 0; i < gm.NbColumns; i++)
        {
            for (int j = 0; j < gm.NbLines; j++)
            {
                distances[i, j] = -1;
            }
        }

        distances[_startingPos.column, _startingPos.line] = 0;
        int index = 0;
        //parcours tout les cases pas a pas
        while (distances[_destination.column, _destination.line] == -1)
        {
            List<GridData.PosGrid> currentPos = new List<GridData.PosGrid>();
            // tout les positions du pas actuel
            for (int i = 0; i < gm.NbColumns; i++)
            {
                for (int j = 0; j < gm.NbLines; j++)
                {

                    if (distances[i, j] == index)
                    {
                        GridData.PosGrid posGrid;
                        posGrid.column = i;
                        posGrid.line = j;
                        currentPos.Add(posGrid);

                    }
                }
            }
            // regarde toute les cases a coté des cases du pas actuel quon as calculer avant et si libre on les enregistre avec un pas de valeur + 1
            foreach (var pos in currentPos)
            {
                if (gm.IsValidPos(pos.column + 1, pos.line) && (gm.GetCaseState(pos.column + 1, pos.line) == GridData.STATE.EMPTY && distances[pos.column + 1, pos.line] == -1) ||
                    (pos.column + 1 == _destination.column && pos.line == _destination.line))
                {
                    distances[pos.column + 1, pos.line] = index + 1;
                }
                if (gm.IsValidPos(pos.column - 1, pos.line) && (gm.GetCaseState(pos.column - 1, pos.line) == GridData.STATE.EMPTY && distances[pos.column - 1, pos.line] == -1) ||
                    (pos.column - 1 == _destination.column && pos.line == _destination.line))
                {
                    distances[pos.column - 1, pos.line] = index + 1;
                }
                if (gm.IsValidPos(pos.column, pos.line + 1) && (gm.GetCaseState(pos.column, pos.line + 1) == GridData.STATE.EMPTY && distances[pos.column, pos.line + 1] == -1) ||
                    (pos.column == _destination.column && pos.line + 1 == _destination.line))
                {
                    distances[pos.column, pos.line + 1] = index + 1;
                }
                if (gm.IsValidPos(pos.column, pos.line - 1) && (gm.GetCaseState(pos.column, pos.line - 1) == GridData.STATE.EMPTY && distances[pos.column, pos.line - 1] == -1) ||
                    (pos.column == _destination.column && pos.line - 1 == _destination.line))
                {
                    distances[pos.column, pos.line - 1] = index + 1;
                }
            }
            index++;

        }
        GridData.PosGrid posT = _destination;
        //parcours le chemin de l'arrivé vers le depart et on s'arrete  a la distance que peux parcourir l'ia 
        for (int i = distances[_destination.column, _destination.line]; i > _movePoints; i--)
        {
            if (gm.IsValidPos(posT.column + 1, posT.line) && distances[posT.column + 1, posT.line] == i - 1)
            {
                posT.column += 1;
            }
            else if (gm.IsValidPos(posT.column - 1, posT.line) && distances[posT.column - 1, posT.line] == i - 1)
            {
                posT.column -= 1;
            }
            else if (gm.IsValidPos(posT.column, posT.line + 1) && distances[posT.column, posT.line + 1] == i - 1)
            {
                posT.line += 1;
            }
            else if (gm.IsValidPos(posT.column, posT.line - 1) && distances[posT.column, posT.line - 1] == i - 1)
            {
                posT.line -= 1;
            }
        }
        //pour ne pas avoir les ia qui se chevauche
        if (distances[_destination.column, _destination.line] <= _movePoints)
        {
            if (gm.IsValidPos(posT.column + 1, posT.line) && distances[posT.column + 1, posT.line] == distances[_destination.column, _destination.line] - 1)
            {
                posT.column += 1;
                return posT;
            }
            if (gm.IsValidPos(posT.column - 1, posT.line) && distances[posT.column - 1, posT.line] == distances[_destination.column, _destination.line] - 1)
            {
                posT.column -= 1;
                return posT;
            }
            if (gm.IsValidPos(posT.column, posT.line + 1) && distances[posT.column, posT.line + 1] == distances[_destination.column, _destination.line] - 1)
            {
                posT.line += 1;
                return posT;
            }
            if (gm.IsValidPos(posT.column, posT.line - 1) && distances[posT.column, posT.line - 1] == distances[_destination.column, _destination.line] - 1)
            {
                posT.line -= 1;
                return posT;
            }
        }
        return posT;
    }

}

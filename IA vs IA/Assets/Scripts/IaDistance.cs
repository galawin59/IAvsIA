using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaDistance : Ia
{
    GridData.PosGrid destination;
    Ia target;
    public override void Play()
    {
        if (!IsAlive)
        {
            return;
        }
        Think();
        Move();
        Attack();
    }

    protected override void Attack()
    {
        if (GridData.Distance(Pos, target.Pos) <= 5)
        {
            target.LoseHp(Damage,target);
        }
    }

    protected override void Move()
    {
        GridManager gm = GridManager.Instance;
        gm.SetIaRef(null, Pos.column, Pos.line);
        gm.SetIaRef(this, destination.column, destination.line);
        Pos = destination;
    }

    protected override void Think()
    {
        IaManager im = IaManager.Instance;

        int idEnemiesTeam = (IdTeam + 1) % 2;
        int shortestDistanceToTarget = 1000000;

        destination = im.IaTeam(idEnemiesTeam)[0].Pos;
        target = im.IaTeam(idEnemiesTeam)[0];
        shortestDistanceToTarget = GridData.Distance(Pos, destination);

        for (int i = 1; i < im.IaTeam(idEnemiesTeam).Count; i++)
        {
            if (im.IaTeam(idEnemiesTeam)[i].IsAlive)
            {
                int distanceTemp = GridData.Distance(Pos, im.IaTeam(idEnemiesTeam)[i].Pos);

                if (distanceTemp <= shortestDistanceToTarget)
                {
                    shortestDistanceToTarget = distanceTemp;
                    destination = im.IaTeam(idEnemiesTeam)[i].Pos;
                    target = im.IaTeam(idEnemiesTeam)[i];
                }

            }
        }
        // pour rester a distance de la target
        if (GridData.Distance(Pos, destination) <= 4)
        {
            List<GridData.PosGrid> pos = new List<GridData.PosGrid>();
            for (int i = Pos.column - 3; i < Pos.column + 3; i++)
            {
                for (int j = Pos.line - 3; j < Pos.line + 3; j++)
                {
                    GridData.PosGrid posGrid;
                    posGrid.column = i;
                    posGrid.line = j;
                    if (GridData.Distance(destination, posGrid) == 3 && GridManager.Instance.IsValidPos(posGrid.column, posGrid.line))
                    {
                        pos.Add(posGrid);
                    }
                }
            }
            destination = pos[0];
            int distance = GridData.Distance(Pos, destination);
            for (int i = 1; i < pos.Count; i++)
            {
                if (GridData.Distance(Pos, pos[i]) < distance)
                {
                    destination = pos[i];
                    distance = GridData.Distance(Pos, pos[i]);
                }
            }
        }
        else
            destination = Dijkstra(Pos, destination, MovePoints);
    }
}

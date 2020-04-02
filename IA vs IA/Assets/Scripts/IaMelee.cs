using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IaMelee : Ia
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
        if (GridData.Distance(Pos, target.Pos) <= 1)
        {
            target.LoseHp(Damage, target);
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
               
               if(distanceTemp <= shortestDistanceToTarget)
                {
                    shortestDistanceToTarget = distanceTemp;
                    destination = im.IaTeam(idEnemiesTeam)[i].Pos;
                    target = im.IaTeam(idEnemiesTeam)[i];
                }

            }
        }
        destination = Dijkstra(Pos, destination, MovePoints);
        
    }
}

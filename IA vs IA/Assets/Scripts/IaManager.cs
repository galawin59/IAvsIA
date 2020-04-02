using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaManager : MonoBehaviour
{
    static IaManager instance;
    public static IaManager Instance
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
    }

    [SerializeField] List<Ia.IaStats> statsIaTeamOne = null;
    [SerializeField] List<Ia.IaStats> statsIaTeamTwo = null;

    List<Ia> iaTeamOne;
    List<Ia> iaTeamTwo;

    public List<Ia> IaTeamOne
    {
        get
        {
            return iaTeamOne;
        }
    }
    public List<Ia> IaTeamTwo
    {
        get
        {
            return iaTeamTwo;
        }
    }

    public List<Ia> IaTeam(int idTeam)
    {
        if (idTeam == 0)
        {
            return iaTeamOne;
        }
        else if (idTeam == 1)
        {
            return iaTeamTwo;
        }
        return null;
    }

    [SerializeField] bool isTurnIaOne = true;
    void PlayTeam(List<Ia> team, List<Ia> enemiesTeams)
    {
        StartCoroutine(IPlayTeam(team, enemiesTeams));
    }

    IEnumerator IPlayTeam(List<Ia> team, List<Ia> enemiesTeams)
    {
        //Debug.Log("One = " + iaTeamOne.Count);
        //Debug.Log("two = " + iaTeamTwo.Count);
        //Debug.Log("team = " + team.Count);
        for (int i = 0; i < team.Count; i++)
        {
            yield return new WaitForSeconds(1f);
            team[i].Play();
        }
        isTurnIaOne = !isTurnIaOne;
        PlayTeam(enemiesTeams, team);
    }

    private void Start()
    {
       
        InitTeam(ref iaTeamOne, statsIaTeamOne, 0);
        InitTeam(ref iaTeamTwo, statsIaTeamTwo, 1);
        if (isTurnIaOne)
        {
           
            PlayTeam(iaTeamOne, iaTeamTwo);
        }
        else
        {
            PlayTeam(iaTeamTwo, iaTeamOne);
        }
    }

    void InitTeam(ref List<Ia> team, List<Ia.IaStats> teamStats, int idTeam)
    {
        team = new List<Ia>();
        int id = 0;
        foreach (Ia.IaStats stat in teamStats)
        {
            Ia newIa;
            if (stat.Behavior == Ia.BEHAVIOR.MELEE)
            {
                newIa = new IaMelee();
            }
            else if(stat.Behavior == Ia.BEHAVIOR.DISTANCE)
            {
                newIa = new IaDistance();
            }
            else
            {
                newIa = new IaMelee();
            }
            newIa.ActionPoints = stat.ActionPoints;
            newIa.Behavior = stat.Behavior;
            newIa.Damage = stat.Damage;
            newIa.Hp = stat.Hp;
            newIa.Pos = stat.Pos;
            newIa.MovePoints = stat.MovePoints;
            newIa.Id = id++;
            newIa.IdTeam = idTeam;
            newIa.IsAlive = true;
          
            team.Add(newIa);
        }
      
        GridManager.Instance.SetIaRef(team);
    }

    public bool TeamOneIsAlive()
    {
        foreach (Ia ia in iaTeamOne)
        {
            if (ia.IsAlive)
            {
                return true;
            }
        }
        return false;
    }
    public bool TeamTwoIsAlive()
    {
        foreach (Ia ia in iaTeamTwo)
        {
            if (ia.IsAlive)
            {
                return true;
            }
        }
        return false;
    }
    public bool TeamIsAlive(int idTeam)
    {
        if (idTeam == 0)
        {
            return TeamOneIsAlive();
        }
        else if (idTeam == 1)
        {
            return TeamTwoIsAlive();
        }
        return false;
    }
    public int GetNbIaAliveTeamOne()
    {
        int count = 0;
        foreach (Ia ia in iaTeamOne)
        {
            if (ia.IsAlive)
            {
                count++;
            }
        }
        return count;
    }
    public int GetNbIaAliveTeamTwo()
    {
        int count = 0;
        foreach (Ia ia in iaTeamTwo)
        {
            if (ia.IsAlive)
            {
                count++;
            }
        }
        return count;
    }
    public int GetNbIaAliveTeam(int idTeam)
    {
        if (idTeam == 0)
        {
            return GetNbIaAliveTeamOne();
        }
        else if (idTeam == 1)
        {
            return GetNbIaAliveTeamTwo();
        }
        return 0;
    }
  
}
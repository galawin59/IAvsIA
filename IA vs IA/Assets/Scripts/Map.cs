using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour
{
    [SerializeField] GameObject prefabCase = null;
    [SerializeField] GameObject prefabMeleeTeamOne = null;
    [SerializeField] GameObject prefabMeleeTeamTwo = null;
    [SerializeField] GameObject prefabDistanceTeamOne = null;
    [SerializeField] GameObject prefabDistanceTeamTwo = null;
    [SerializeField] GameObject prefabIaOne = null;
    [SerializeField] GameObject prefabIaTwo = null;
    [SerializeField] GameObject prefabWall = null;
    [SerializeField] float sizeCase = 1f;

   public List<GameObject> iasOne;
    public List<GameObject> iasTwo;
    IaManager im;
    GridManager gm;


    IEnumerator Start()
    {
        yield return null;
        im = IaManager.Instance;
        gm = GridManager.Instance;
        //map visuel
        for (int i = 0; i < gm.NbColumns; i++)
        {
            for (int j = 0; j < gm.NbLines; j++)
            {
                if (gm.GetCaseState(i, j) == GridData.STATE.EMPTY)
                    Instantiate(prefabCase, new Vector3(i * sizeCase, j * sizeCase), Quaternion.identity);
                else if (gm.GetCaseState(i, j) == GridData.STATE.BUSY)
                    Instantiate(prefabWall, new Vector3(i * sizeCase, j * sizeCase), Quaternion.identity);
                else
                    Instantiate(prefabCase, new Vector3(i * sizeCase, j * sizeCase), Quaternion.identity);
            }
        }
        iasOne = new List<GameObject>();
        iasTwo = new List<GameObject>();
      
        // ia equipe 1 
        foreach (Ia ia in im.IaTeamOne)
        {
            if (ia.Behavior == Ia.BEHAVIOR.MELEE)
            {
                iasOne.Add(Instantiate(prefabMeleeTeamOne, new Vector2(ia.Pos.column, ia.Pos.line) * sizeCase, Quaternion.identity));
            }
            else if (ia.Behavior == Ia.BEHAVIOR.DISTANCE)
            {
                iasOne.Add(Instantiate(prefabDistanceTeamOne, new Vector2(ia.Pos.column, ia.Pos.line) * sizeCase, Quaternion.identity));
            }
            else
            {
                iasOne.Add(Instantiate(prefabIaOne, new Vector2(ia.Pos.column, ia.Pos.line) * sizeCase, Quaternion.identity));
            }
        }
        //ia equipe 2
        foreach (Ia ia in im.IaTeamTwo)
        {
            if (ia.Behavior == Ia.BEHAVIOR.MELEE)
            {
                iasTwo.Add(Instantiate(prefabMeleeTeamTwo, new Vector2(ia.Pos.column, ia.Pos.line) * sizeCase, Quaternion.identity));
            }
            else if (ia.Behavior == Ia.BEHAVIOR.DISTANCE)
            {
                iasTwo.Add(Instantiate(prefabDistanceTeamTwo, new Vector2(ia.Pos.column, ia.Pos.line) * sizeCase, Quaternion.identity));
            }
            else
            {
                iasTwo.Add(Instantiate(prefabIaTwo, new Vector2(ia.Pos.column, ia.Pos.line) * sizeCase, Quaternion.identity));
            }
        }
    }

    private void Update()
    {
        if (iasOne == null || iasTwo == null)
        {
            return;
        }
        // mise a jour des ia 
        for (int i = 0; i < iasOne.Count; i++)
        {
            if (im.IaTeamOne[i].IsAlive)
            {
                iasOne[i].SetActive(true);
                iasOne[i].transform.position = new Vector2(im.IaTeamOne[i].Pos.column, im.IaTeamOne[i].Pos.line) * sizeCase;
            }
            else
            {
                iasOne[i].SetActive(false);
                
             
            }
        }
        for (int i = 0; i < iasTwo.Count; i++)
        {
            if (im.IaTeamTwo[i].IsAlive)
            {
                iasTwo[i].SetActive(true);
                iasTwo[i].transform.position = new Vector2(im.IaTeamTwo[i].Pos.column, im.IaTeamTwo[i].Pos.line) * sizeCase;
            }
            else
            {
                iasTwo[i].SetActive(false);
           
            }
        }
    }
}


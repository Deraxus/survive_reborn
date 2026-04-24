using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockUtils
{
    public static List<Vector2> GetCellsAround4(GameObject obj)
    {
        List<Vector2> returnedVectors = new List<Vector2>();
        Vector2 vectorTop = obj.transform.position;
        vectorTop.y = Mathf.Round(vectorTop.y) + 1;
        vectorTop.x = Mathf.Round(vectorTop.x);
        Vector2 vectorBot = obj.transform.position;
        vectorBot.y = Mathf.Round(vectorBot.y) - 1;
        vectorBot.x = Mathf.Round(vectorBot.x);
        Vector2 vectorLeft = obj.transform.position;
        vectorLeft.x = Mathf.Round(vectorLeft.x) - 1;
        vectorLeft.y = Mathf.Round(vectorLeft.y);
        Vector2 vectorRight = obj.transform.position;
        vectorRight.x = Mathf.Round(vectorRight.x) + 1;
        vectorRight.y = Mathf.Round(vectorRight.y);
        returnedVectors.Add(vectorTop);
        returnedVectors.Add(vectorRight);
        returnedVectors.Add(vectorBot);
        returnedVectors.Add(vectorLeft);
        foreach (Vector2 v in returnedVectors)
        {
            GameObject.Instantiate(GameObject.Find("slime"), v, new Quaternion(0, 0, 0, 0));
        }
        return returnedVectors;
    }


}

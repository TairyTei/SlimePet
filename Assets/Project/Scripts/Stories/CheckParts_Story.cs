using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Story", menuName = "My/Stroies/CheckParts_Story", order = 2)]
public class CheckParts_Story : Story {

    public List<int> requireHistoryID = new List<int>();
    public override bool Condition()
    {
        foreach (int id in requireHistoryID)
        {
            if (PlayerPrefs.GetInt("Story_" + id) == 0)
            {
                return false;
            }
        }
            return true;
        
    }
}

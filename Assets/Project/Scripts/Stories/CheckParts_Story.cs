using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Story", menuName = "My/Stroies/CheckParts_Story", order = 2)]
public class CheckParts_Story : Story {

    public int requireHistoryID;
    public override bool Condition()
    {
        if (PlayerPrefs.GetInt("Story_" + requireHistoryID) != 0)
        {
            return true;
        }
        return false;
    }
}

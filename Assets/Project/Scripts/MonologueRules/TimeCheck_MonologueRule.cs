using UnityEngine;

[CreateAssetMenu(fileName = "Monologue", menuName = "My/MonologueRules/TimeCheck")]
public class TimeCheck_MonologueRule : MonologueRule {

    public int minTime, maxTime;
    public override bool Condition()
    {
        return System.DateTime.Now.Hour > minTime && System.DateTime.Now.Hour < minTime ? true : false;
    }
}
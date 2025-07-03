using UnityEngine;

[CreateAssetMenu(fileName = "Monologue", menuName = "My/MonologueRules/HatCheck")]
public class HatCheck_MonologueRule : MonologueRule {

    public override bool Condition()
    {
        int activeHatID = SkinManger.instance.ActiveHatID;

        return activeHatID != 0 ? true : false;
    }
}
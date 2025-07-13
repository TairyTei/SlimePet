using UnityEngine;

[CreateAssetMenu(fileName = "Monologue", menuName = "My/MonologueRules/TagCheck")]
public class AreThereObjectsWithTag_MonologueRule : MonologueRule {
    public string tag;
    public override bool Condition()
    {
        return FurnitureBox.instance.GetObjectsWithTag(tag).Length > 0 ? true : false;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "Monologue", menuName = "My/MonologueRules/NameCheck", order = 0)]
public class NameCheck_MonologueRule : MonologueRule
{
    public string requiredName;
    public override bool Condition()
    {
        string current = Pet.Instance.petName.Replace(" ", "").ToLower();
        string requiredString = requiredName.Replace(" ", "").ToLower();
        if (current == requiredString) return true;
        return false;
    }
}
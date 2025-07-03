using UnityEngine;

[CreateAssetMenu(fileName = "Monologue", menuName = "My/MonologueRules/HappinessCheck")]
public class HappinessCheck_MonologueRule : MonologueRule
{
    public float minValue, maxValue;
    public override bool Condition()
    {
        return Pet.Instance.Stats.Happiness >= minValue && Pet.Instance.Stats.Happiness <= maxValue;
    }
}

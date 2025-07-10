using UnityEngine;

[System.Serializable]
public class PetStats
{
    [SerializeField] private float satiety = 10;
    public float Satiety { get { return satiety; } set { satiety = Mathf.Clamp(value, 0, 10); } }
    [SerializeField] private float cleanliness = 10;
    public float Cleanliness { get { return cleanliness; } set { cleanliness = Mathf.Clamp(value, 0, 10); } }
    [SerializeField] private float love = 5;
    public float Love { get { return love; } set { love = Mathf.Clamp(value, 0, 10); } }
    [SerializeField] private float boredom = 5;
    public float Boredom { get { return boredom; } set { boredom = Mathf.Clamp(value, 0, 10); } }
    [SerializeField] private float cheerfulness = 10;
    public float Cheerfulness { get { return cheerfulness; } set { cheerfulness = Mathf.Clamp(value, 0, 10); } }
    [SerializeField] private float happiness = 10;
    public float Happiness { get { return happiness; } set { happiness = Mathf.Clamp(value, 0, 10); } }
    [SerializeField] private int level = 0;
    public int Level { get { return level; } }
    private const float lvlMultiplier = 0.2f;
    [SerializeField] private int expirience = 0;
    public int Expirience
    {
        get { return expirience; }
        set
        {
            SetExpirience(value);
        }
    }
    [SerializeField] private int coins = 0;
    public int Coins { get { return coins; } set { coins = value; } }
    private int storiesQueue = 0;
    public int StoriesQueue { get { return storiesQueue; } set { storiesQueue = value; } }
    [SerializeField] private float poopBar = 0;
    public float PoopBar { get { return poopBar; } set { poopBar = value; } }
    private void SetExpirience(int newValue)
    {
        expirience = newValue;

        int newLevel = CalculateLevelFromExp(newValue);
        if (newLevel != level)
        {
            SetLevel(newLevel);
        }
    }
    private void SetLevel(int newLevel) {
        int difference = newLevel - level;
        level = newLevel;

        if (difference > 0)
        {
            StoriesQueue += difference;
            PlayerPrefs.SetInt("StoriesInQueue", storiesQueue);
        }
        Debug.Log("Уровень повышен до " + level);
    }
    public static int CalculateLevelFromExp(int exp) {
        return Mathf.FloorToInt(lvlMultiplier * Mathf.Sqrt(exp));
    }
    public static int CalculateExpirienceFromLvl(int lvl) {
        return (int)Mathf.Pow(lvl / lvlMultiplier, 2);
    }
}
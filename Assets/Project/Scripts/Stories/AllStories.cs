using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllStories", menuName = "My/Stroies/AllStories", order = 0)]
public class AllStories : ScriptableObject {
    public List<Story> stories = new List<Story>();
}
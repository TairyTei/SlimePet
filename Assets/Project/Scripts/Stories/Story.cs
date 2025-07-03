using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Story", menuName = "My/Stroies/Story", order = 1)]
public class Story : LocalizableSO
{
    [Header("Text Files")]
    public string StoryTitle { get; private set; }
    [SerializeField] private string _storyTextPath;
    public string StoryText { get; private set; }

    [Header("Editor Preview")]
    [TextArea(10, 30)][SerializeField] private string _editorPreview;


    public Hat hatGift;
    public virtual bool Condition()
    {
        return true;
    }
    public override void OnLanguageChanged()
    {
        StoryText = ExternalTextLoader.LoadText(_storyTextPath);
        StoryTitle = ExternalTextLoader.LoadText(_storyTextPath + "_title");
    }
    
}

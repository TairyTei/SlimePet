using TMPro;
using UnityEngine;
public class StorySelectButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    private StoriesWindow storiesWindow;
    private Story story;

    public void Set(StoriesWindow storiesWindow, Story story)
    {
        this.storiesWindow = storiesWindow;
        this.story = story;
        text.text = story.StoryTitle;
    }
    public void Press()
    {
        storiesWindow.LoadStory(story);
    }
}
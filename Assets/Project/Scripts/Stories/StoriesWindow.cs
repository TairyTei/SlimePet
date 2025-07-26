using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoriesWindow : MonoBehaviour, IInitializable
{
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI storyTitle;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private GameObject getGiftButton;
    [SerializeField] private Transform listParent;

    [SerializeField] private GameObject selectButtonPrefab;
    [SerializeField] private HatSelectionMenu hatSelectionMenu;
    [SerializeField] private AllStories allStories;
    private Story openedStory;
    private static StoriesWindow instance;
    public static StoriesWindow Instance {get { return instance; }}

    public void Initialize()
    {
        instance = this;
    }
    public void LoadStory(Story story)
    {
        openedStory = story;
        storyTitle.text = story.StoryTitle;
        storyText.text = VariableReplacer.Replace(story.StoryText);
        if (story.hatGift != null
        && PlayerPrefs.GetInt("IsHat" + SkinManger.instance.allHats.hats.IndexOf(story.hatGift) + "Bought") != 1)
        {
            getGiftButton.SetActive(true);
        }
        else
        {
            getGiftButton.SetActive(false);
        }
    }
    public void Open()
    {
        UpdateList();
        menu.SetActive(true);
    }
    public void Close()
    {
        menu.SetActive(false);
    }
    public void UpdateList()
    {
        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < allStories.stories.Count; i++)
        {
            if (PlayerPrefs.GetInt("Story_" + i) == 0) continue;

            Story story = allStories.stories[i];

            GameObject button = Instantiate(selectButtonPrefab, listParent);
            button.GetComponent<StorySelectButton>().Set(this, story);
        }
    }
    public void GetGift()
    {
        if (openedStory.hatGift != null)
        {
            Hat hatGift = openedStory.hatGift;
            int hatGiftIndex = SkinManger.instance.allHats.hats.IndexOf(hatGift);
            PlayerPrefs.SetInt("IsHat" + hatGiftIndex + "Bought", 1);
            getGiftButton.SetActive(false);
            hatSelectionMenu.UpdateHatList();
        }
    }
    public Story[] GetArrayOfAvailableStories()
    {
        List<Story> availableStories = new List<Story>();
        for (int i = 0; i < allStories.stories.Count; i++)
        {
            if (allStories.stories[i].Condition() && PlayerPrefs.GetInt("Story_" + i) == 0)
            {
                availableStories.Add(allStories.stories[i]);
            }
        }
        return availableStories.ToArray();
    }
    public void UnlockRandomStory()
    {
        Story[] stories = GetArrayOfAvailableStories();
        if (stories.Length == 0)
        {
            Debug.Log("Историй нет");
            return;
        }
        Story story = stories[Random.Range(0, stories.Length)];
        int indexOfStory = allStories.stories.IndexOf(story);
        PlayerPrefs.SetInt("Story_" + indexOfStory, 1);
    }
}

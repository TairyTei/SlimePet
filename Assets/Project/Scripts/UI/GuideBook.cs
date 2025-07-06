using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBook : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject[] pages;
    private int currentPage = 0;
    public void Open()
    {
        OpenPage(0);
        menu.SetActive(true);
    }
    public void Close()
    {
        menu.SetActive(false);
    }
    private void OpenPage(int index)
    {
        if (index > pages.Length) return;

        currentPage = index;
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }

        bool previousButtonActive = currentPage != 0;
        previousButton.SetActive(previousButtonActive);

        bool nextButtoActive = currentPage < pages.Length -1;
        nextButton.SetActive(nextButtoActive);
    }
    public void NextPage()
    {
        if (currentPage < pages.Length -1)
            OpenPage(currentPage + 1);
    }
    public void PreviousPage()
    {
        if (currentPage > 0)
            OpenPage(currentPage - 1);
    }
}

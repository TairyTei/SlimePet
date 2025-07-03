using TMPro;
using UnityEngine;

public class TMProLocalization : LocalizableMB
{
    [SerializeField] private PathLink[] pathLinks;

    [System.Serializable]
    private struct PathLink
    {
        public TextMeshProUGUI TMProComponent;
        public string TextPath;
    }
    public override void OnLanguageChanged()
    {
        foreach (PathLink pathLink in pathLinks)
        {
            pathLink.TMProComponent.text = ExternalTextLoader.LoadText(pathLink.TextPath);
        }
    }
}
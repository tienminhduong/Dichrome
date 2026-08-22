using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialLine> tutorialLines = new();
    [SerializeField] private TutorialCanvas tutorialCanvas;
    private readonly Dictionary<int, List<string>> tutorialLinesDictionary = new();
    private readonly HashSet<int> levelsShown = new();

    void Awake()
    {
        LoadListToDictionary();
    }

    void OnEnable()
    {
        PublicEvents.OnLevelLoaded += HandleLevelLoaded;
    }

    void OnDisable()
    {
        PublicEvents.OnLevelLoaded -= HandleLevelLoaded;
    }

    private void LoadListToDictionary()
    {
        tutorialLinesDictionary.Clear();
        foreach (var tutorialLine in tutorialLines)
        {
            if (!tutorialLinesDictionary.ContainsKey(tutorialLine.levelIndex))
            {
                tutorialLinesDictionary[tutorialLine.levelIndex] = new List<string>();
            }
            tutorialLinesDictionary[tutorialLine.levelIndex].AddRange(tutorialLine.lines);
        }
    }

    private void HandleLevelLoaded(int levelIndex)
    {
        if (tutorialLinesDictionary.TryGetValue(levelIndex, out var lines) && !levelsShown.Contains(levelIndex))
        {
            tutorialCanvas.LoadAndShowDialogue(lines);
            levelsShown.Add(levelIndex);
        }
    }
}

[Serializable]
public struct TutorialLine
{
    public int levelIndex;
    public List<string> lines;
}

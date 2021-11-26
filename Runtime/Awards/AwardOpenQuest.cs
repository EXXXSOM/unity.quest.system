using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpenQuest", menuName = "StorySystem/Awards/OpenQuest", order = 1)]
public class AwardOpenQuest : QuestAward
{
    [SerializeField] private List<QuestDescription> _storyPartsForOpen;

    public override void Give()
    {
        for (int i = 0; i < _storyPartsForOpen.Count; i++)
        {
            QuestSystem.Instance.UnlockQuest(_storyPartsForOpen[i]);
        }
    }
}

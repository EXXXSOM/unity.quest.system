using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDescription", menuName = "StorySystem/Description/QuestDescription", order = 1)]
public class QuestDescription : ScriptableObject
{
    [Header("Main info:")]
    [SerializeField] private QuestStatus _status = QuestStatus.AwaitingActivation;
    [SerializeField] private QuestLocationType _locationType = QuestLocationType.None;
    [SerializeField] private string _nameStoryPart;
    [SerializeField] private Sprite _icon;
    [SerializeField, TextArea(5, 10)] private string _description;

    [Header("Award:")]
    [SerializeField] private List<QuestAward> _storyPartAwards;

    public QuestStatus StoryPartStatus => _status;
    public QuestLocationType StoryLocationType => _locationType;
    public string QuestName => _nameStoryPart;
    public Sprite Icon => _icon;
    public string Description => _description;
    public List<QuestAward> StoryPartAward => new List<QuestAward>(_storyPartAwards);
}

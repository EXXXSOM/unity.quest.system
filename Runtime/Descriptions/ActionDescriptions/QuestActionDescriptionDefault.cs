using UnityEngine;

[CreateAssetMenu(fileName = "QuestActionDescriptionDefault", menuName = "StorySystem/Description/QuestActionDescription/Default", order = 0)]
public class QuestActionDescriptionDefault : QuestActionDescriptionBase
{
    public string GetDescription => _description;
}

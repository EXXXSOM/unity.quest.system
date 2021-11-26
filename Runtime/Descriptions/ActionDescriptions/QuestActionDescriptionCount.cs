using UnityEngine;

[CreateAssetMenu(fileName = "QuestActionDescriptionCount", menuName = "StorySystem/Description/QuestActionDescription/Count", order = 1)]
public class QuestActionDescriptionCount : QuestActionDescriptionBase
{
    [SerializeField, Tooltip("Use {1}/{0} in the description to insert a number.")] private int _needCountCallAction;
    public string Description(int count) => string.Format(_description, _needCountCallAction, count);
}

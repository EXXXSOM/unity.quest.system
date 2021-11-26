using UnityEngine;

public class CheckPointEventScriptQuestAction : QuestActionBase
{
    [SerializeField] private QuestActionDescriptionDefault _questActionDescriptionDefault;

    public override Sprite GetIcon => _questActionDescriptionDefault.Icon;
    public override string GetDescription => _questActionDescriptionDefault.GetDescription;

    public virtual void CallEvent()
    {
        Complete();
    }
}

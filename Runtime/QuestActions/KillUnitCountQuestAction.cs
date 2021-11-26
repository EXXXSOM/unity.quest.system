using System.Collections.Generic;
using UnityEngine;

public class KillUnitCountQuestAction : QuestActionBase
{
    [SerializeField] private int _killedUnit = 0;
    [SerializeField] private QuestActionDescriptionCount _questActionDescriptionCount;

    public override Sprite GetIcon => _questActionDescriptionCount.Icon;
    public override string GetDescription => _questActionDescriptionCount.Description(_killedUnit);

    public override void SaveQuestActionData(Dictionary<string, object> saveData)
    {
        saveData.Add("_killedUnit", _killedUnit);
    }

    public override void LoadQuestActionData(Dictionary<string, object> saveData)
    {
        _killedUnit = (int)saveData["_killedUnit"];
    }
}

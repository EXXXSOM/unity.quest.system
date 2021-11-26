using System.Collections.Generic;

[System.Serializable]
public class AllQuestData
{
    public readonly Dictionary<int, QuestData> QuestsData;
    public readonly Dictionary<int, Dictionary<string, object>> ActiveQuestActionsData;

    public AllQuestData(Dictionary<int, QuestData> questsData, Dictionary<int, Dictionary<string, object>> activeQuestActionsData)
    {
        QuestsData = questsData;
        ActiveQuestActionsData = activeQuestActionsData;
    }
}
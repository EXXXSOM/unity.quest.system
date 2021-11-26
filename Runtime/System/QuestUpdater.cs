public class QuestUpdater
{
    public void UpdateQuestData(QuestBase quest, QuestData questData)
    {
        questData.State = quest.GetState;
        questData.Status = quest.GetStatus;
    }

    public void UpdateQuestDataStatus(QuestData questData, QuestBase quest)
    {
        questData.Status = quest.GetStatus;
    }

    public void UpdateQuestDataState(QuestData questData, QuestBase quest)
    {
        questData.State = quest.GetState;
    }
}

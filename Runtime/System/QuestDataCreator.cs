public class QuestDataCreator
{
    public QuestData CreateData(QuestBase storyPart)
    {
        return new QuestData(storyPart.GetState, storyPart.GetStatus, storyPart.GetMainDescription.StoryLocationType);
    }

    public QuestData CreateEmptyDataWithOpenStatus()
    {
        return new QuestData(0, QuestStatus.AwaitingActivation, QuestLocationType.None);
    }
}

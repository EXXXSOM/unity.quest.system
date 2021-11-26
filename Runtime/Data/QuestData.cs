[System.Serializable]
public class QuestData
{
    public readonly QuestLocationType LocationType;
    public QuestStatus Status;
    public int State;

    public QuestData(int state, QuestStatus status, QuestLocationType locationType)
    {
        State = state;
        Status = status;
        LocationType = locationType;
    }
}
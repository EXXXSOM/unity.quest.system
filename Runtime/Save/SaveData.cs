using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public readonly AllQuestData AllQuestData;

    public SaveData(bool isEmpty){}

    public SaveData(AllQuestData allQuestData)
    {
        AllQuestData = allQuestData;
    }
}

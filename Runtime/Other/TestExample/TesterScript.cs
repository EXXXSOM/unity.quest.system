using System.Collections;
using UnityEngine;
using EXSOM.SaveSyste;

[DefaultExecutionOrder(-10200)]
public class TesterScript : MonoBehaviour
{
    public QuestBase StoryPartBase;

    public CheckPointColliderQuestAction CheckPointColliderQuestAction;
    public KillUnitCountQuestAction KillUnitCountQuestAction;

    public bool loadGame = false;

    private void Awake()
    {
        if (loadGame)
        {
            SaveSystem.LoadData();
        }

        AllQuestData allQuestData = null;
        SaveSystem.GetData("QuestSystem", out allQuestData);
        QuestSystem.Boodstrap(allQuestData);
    }

    private void Start()
    {
        if (!loadGame)
        {
            Debug.Log("Setup");
            StoryPartBase.Setup();

            StartCoroutine(Wait(4));
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("CheckPointColliderQuestAction");
        CheckPointColliderQuestAction.Complete();
        StartCoroutine(WaitKillUnitCountQuestAction(4));
    }

    IEnumerator WaitKillUnitCountQuestAction(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("KillUnitCountQuestAction");
        KillUnitCountQuestAction.Complete();
    }
}

using System.Collections;
using UnityEngine;

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

        QuestSystem.Boodstrap(SaveSystem.SaveData.AllQuestData);
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

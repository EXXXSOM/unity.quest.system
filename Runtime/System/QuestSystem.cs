using System;
using System.Collections.Generic;
using UnityEngine;
using EXSOM.SaveSyste;

public class QuestSystem : ISavable
{
    public const int START_CAPACITY_DATA = 10;
    public const int START_ACTIVE_QUEST = 5;
    public static QuestSystem Instance;

    public event Action<int, QuestDescription> OnUnlockQuest;
    public event Action<int, QuestDescription, string> OnSetupNewQuest;
    public event Action<int, QuestDescription> OnCompleteQuest;
    public event Action<int, QuestDescription> OnFailedQuest;
    public event Action<int, string> OnUpdateQuest;
    public event Action<int> OnSkipQuest;

    private static bool _initialized = false;
    private Dictionary<int, QuestData> _questsData;
    private Dictionary<int, Dictionary<string, object>> _activeQuestActionsData;
    private readonly Dictionary<int, QuestBase> _activeQuests = new Dictionary<int, QuestBase>(START_ACTIVE_QUEST);
    private readonly QuestDataCreator _questDataCreator = new QuestDataCreator();
    private readonly QuestUpdater _questUpdater = new QuestUpdater();

    public static void Boodstrap(AllQuestData data)
    {
        if (_initialized) return;

        _initialized = true;
        Instance = new QuestSystem(data);
    }

    private QuestSystem(AllQuestData data)
    {
        Debug.Log(data);
        RegisterInSaveSystem();
        if (data == null)
        {
            CreateEmptyData();
        }
        else
        {
            LoadData(data);
        }

        void CreateEmptyData()
        {
            _activeQuestActionsData = new Dictionary<int, Dictionary<string, object>>(0);
            _questsData = new Dictionary<int, QuestData>(START_CAPACITY_DATA);
        }
    }

    public void LoadData(AllQuestData data)
    {
        if (!_initialized) { Debug.LogError("Don't initialized!"); return; };
        if (data == null) return;

        _questsData = data.QuestsData;
        _activeQuestActionsData = data.ActiveQuestActionsData;
    }

    public void RegisterQuest(QuestBase quest)
    {
        int questKey = GetMetaData(quest);

        if (_questsData.ContainsKey(questKey))
        {
            if (_activeQuestActionsData.ContainsKey(questKey))
            {
                //Если существуют сохраненные данные квеста и его событий - загружаем
                quest.LoadData(_questsData[questKey], _activeQuestActionsData[questKey]);
            }
            else
            {
                //Если существуют сохраненные данные только квеста, загружаем их
                quest.LoadData(_questsData[questKey], null);
            }

            if (_questsData[questKey].Status == QuestStatus.Activated)
            {
                SetupQuest(quest);
            }
        }
        else
        {
            quest.LoadData(null, null);
        }
    }

    public void UnregisterQuest(QuestBase quest)
    {
        int questKey = GetMetaData(quest);

        if (_activeQuests.ContainsKey(questKey))
        {
            _activeQuests.Remove(questKey);
        }
    }

    public bool SetupNewQuest(QuestBase quest)
    {
        if (quest.GetStatus != QuestStatus.AwaitingActivation) return false;

        quest.SetStatus(QuestStatus.Activated);
        SetupQuest(quest);

        return true;
    }

    private void SetupQuest(QuestBase quest)
    {
        int hashKey = GetMetaData(quest);
        if (_questsData.ContainsKey(hashKey))
        {
            _questUpdater.UpdateQuestDataStatus(_questsData[hashKey], quest);
        }
        else
        {
            _questsData.Add(hashKey, _questDataCreator.CreateData(quest));
        }

        _activeQuests.Add(hashKey, quest);
        OnSetupNewQuest?.Invoke(hashKey, quest.GetMainDescription, quest.GetCurrentQuestActionDescription());
    }

    public void SkipQuest(QuestBase quest)
    {
        int questKey = GetMetaData(quest);
        if (_questsData.ContainsKey(questKey))
        {
            quest.SetStatus(QuestStatus.Skiped);
            _questUpdater.UpdateQuestDataStatus(_questsData[questKey], quest);
            OnSkipQuest?.Invoke(questKey);
        }
    }

    public void FailQuest(QuestBase quest)
    {
        int questKey = GetMetaData(quest);
        if (_questsData.ContainsKey(questKey))
        {
            quest.SetStatus(QuestStatus.Failed);
            _questUpdater.UpdateQuestDataStatus(_questsData[questKey], quest);
            OnFailedQuest?.Invoke(questKey, quest.GetMainDescription);
        }
    }

    public void CompleteQuest(QuestBase quest)
    {
        int questKey = GetMetaData(quest);
        if (_questsData.ContainsKey(questKey))
        {
            quest.SetStatus(QuestStatus.Completed);
            _questUpdater.UpdateQuestDataStatus(_questsData[questKey], quest);
            OnCompleteQuest?.Invoke(questKey, quest.GetMainDescription);

            List<QuestAward> storyPartAwards = quest.GetMainDescription.StoryPartAward;
            for (int i = 0; i < storyPartAwards.Count; i++)
            {
                storyPartAwards[i].Give();
            }
        }
    }

    public void UnlockQuest(QuestDescription questDescription)
    {
        int questKey = GetMetaData(questDescription.QuestName);
        if (_questsData.ContainsKey(questKey))
        {
            _questsData.Add(questKey, _questDataCreator.CreateEmptyDataWithOpenStatus());
            OnUnlockQuest?.Invoke(questKey, questDescription);
        }
    }

    public void UpdateQuestState(QuestBase quest)
    {
        int questKey = GetMetaData(quest);
        if (_questsData.ContainsKey(questKey))
        {
            _questUpdater.UpdateQuestDataState(_questsData[questKey], quest);
            OnUpdateQuest?.Invoke(questKey, quest.GetCurrentQuestActionDescription());
        }
    }

    public void UpdateDescriptionQuestAction(QuestBase quest)
    {
        int questKey = GetMetaData(quest);
        if (_questsData.ContainsKey(questKey))
        {
            OnUpdateQuest?.Invoke(questKey, quest.GetCurrentQuestActionDescription());
        }
    }

    public bool CheckQuestStatusByName(string questName, QuestStatus status)
    {
        int questKey = GetMetaData(questName);
        if (_questsData.ContainsKey(questKey) && _questsData[questKey].Status == status)
            return true;

        return false;
    }

    private int GetMetaData(QuestBase quest)
    {
        return quest.GetName.GetHashCode();
    }

    private int GetMetaData(string questName)
    {
        return questName.GetHashCode();
    }

    public AllQuestData GetSaveDataAllQuests()
    {
        Dictionary<int, Dictionary<string, object>> activeQuestActionsData = new Dictionary<int, Dictionary<string, object>>(_activeQuests.Count);

        foreach (var data in _activeQuests)
        {
            if (data.Value.GetStatus == QuestStatus.Activated)
            {
                //Если квест активный, сохраняем его данные и данные его событий
                Dictionary<string, object> savedQuestActionsData = data.Value.SaveQuestData();
                //Если словарь пустой, то квест и его события не хранят особых данных
                if (savedQuestActionsData != null)
                {
                    activeQuestActionsData.Add(data.Key, savedQuestActionsData);
                }

            }
        }

        if (_questsData != null)
        {
            return new AllQuestData(_questsData, activeQuestActionsData);
        }
        else
        {
            Dictionary<int, QuestData> _storyPartData = new Dictionary<int, QuestData>(START_CAPACITY_DATA);
            return new AllQuestData(_storyPartData, activeQuestActionsData);
        }
    }

    public void RegisterInSaveSystem()
    {
        SaveSystem.RegisterSavable(this);
    }

    public void UnregisterInSaveSystem()
    {
        SaveSystem.UnregisterSavable(this);
    }

    public void Save(SaveDataBase saveDataBase)
    {
        saveDataBase.Storage.Add("QuestSystem", GetSaveDataAllQuests());
    }
}
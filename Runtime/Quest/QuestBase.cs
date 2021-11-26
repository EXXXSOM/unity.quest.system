using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestBase : MonoBehaviour
{
    [SerializeField] private QuestStatus _status;
    [SerializeField] private QuestDescription _mainDescription;
    [SerializeField] private List<QuestActionBase> _actionQueue;
    [SerializeField] private QuestActionBase _currentQuestAction;
    private int _state = 0;

    public QuestStatus GetStatus => _status;
    public QuestDescription GetMainDescription => _mainDescription;
    public int GetState => _state;
    public string GetName => _mainDescription.QuestName;

    public Action OnCompleteHandler;

    private QuestSystem _questSystem;

    protected void Awake()
    {
#if UNITY_EDITOR
        if (_mainDescription == null)
            Debug.LogError("The quest has no description! " + this);

        if (_actionQueue.Count == 0)
            Debug.LogError("The quest has no tasks!  " + this);
#endif

        for (int i = 0; i < _actionQueue.Count; i++)
            _actionQueue[i].SetParentQuest(this);

        _questSystem = QuestSystem.Instance;
        _questSystem.RegisterQuest(this);
        OnAwake();
    }

    public void LoadData(QuestData data, Dictionary<string, object> actionData)
    {
        if (data != null)
        {
            _status = data.Status;
            _actionQueue[data.State].LoadQuestActionData(actionData);
            SetState(data.State);
        }
        else
        {
            _status = _mainDescription.StoryPartStatus;
            SetState(0);
        }
    }

    public void Setup()
    {
        if (_questSystem.SetupNewQuest(this))
        {
            if (_actionQueue.Count > _state)
            {
                Debug.Log("Quest setuped!");
                OnSetup();
            }
        }
    }

    public void Complete()
    {
        if (_status == QuestStatus.Activated)
        {
            _questSystem.CompleteQuest(this);

            OnComplete();
            OnCompleteHandler?.Invoke();
        }
    }

    public void Fail()
    {
        if (_status == QuestStatus.Activated)
        {
            _questSystem.FailQuest(this);
            OnFail();
        }
    }

    public void Skip()
    {
        if (_status == QuestStatus.Activated)
        {
            _questSystem.SkipQuest(this);
            OnSkip();
        }
    }

    public void SetStatus(QuestStatus storyPartStatus)
    {
        _status = storyPartStatus;
    }

    public string GetCurrentQuestActionDescription()
    {
        return _currentQuestAction.GetDescription;
    }

    protected void SetState(int newState)
    {
        _state = newState;
        SetCurrentQuestAction(_state);
        _questSystem.UpdateQuestState(this);
    }

    private void SetCurrentQuestAction(int state)
    {
        if (_actionQueue.Count > _state)
        {
            _currentQuestAction = _actionQueue[state];
            _currentQuestAction.StartAction();
        }
        else
        {
            Debug.Log("Quest complete!");
            Complete();
        }
    }

    public void NextState()
    {
        if (_status == QuestStatus.Activated)
        {
            if (_actionQueue.Count > _state)
            {
                Debug.Log("Quest next state!");
                _state++;
                SetState(_state);
            }
        }
    }

    public void QuestActionNeedUpdate()
    {
        if (_status == QuestStatus.Activated)
        {
            _questSystem.UpdateDescriptionQuestAction(this);
        }
    }

    public Dictionary<string, object> SaveQuestData()
    {
        Dictionary<string, object> questData = new Dictionary<string, object>();
        OnSave(questData);
        _currentQuestAction.SaveQuestActionData(questData);

        if (questData.Count == 0)
        {
            return null;
        }
        else
        {
            return questData;
        }
    }

    private void OnDestroy()
    {
        _questSystem.UnregisterQuest(this);
    }

    protected virtual void OnSave(Dictionary<string, object> saveData) { }
    protected virtual void OnAwake() { }
    protected virtual void OnSkip() { }
    protected virtual void OnSetup() { }
    protected virtual void OnComplete() { }
    protected virtual void OnFail() { }
}

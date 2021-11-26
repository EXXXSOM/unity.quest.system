using System.Collections.Generic;
using UnityEngine;

public abstract class QuestActionBase : MonoBehaviour
{
    private bool _started = false;
    private bool _completed = false;
    private QuestBase _quest;

    public abstract Sprite GetIcon { get; }
    public abstract string GetDescription { get; }

    public void StartAction()
    {
        if (!_started)
        {
            _started = true;
            OnStartAction();
        }
    }

    public void Complete()
    {
        if (_started)
        {
            _completed = true;
            OnComplete();
            FinishAction();
            _quest.NextState();
        }
    }

    public bool FinishAction()
    {
        if (_started)
        {
            _started = false;
            OnFinishAction();
        }

        return _completed;
    }

    public void SetParentQuest(QuestBase quest)
    {
#if UNITY_EDITOR
        if (_quest != null)
        {
            Debug.LogWarning("Two quests use an event! " + gameObject.name);
        }
#endif
        _quest = quest;
    }

    public virtual void SaveQuestActionData(Dictionary<string, object> saveData) {}
    public virtual void LoadQuestActionData(Dictionary<string, object> saveData) {}

    protected virtual void OnStartAction() { }
    protected virtual void OnFinishAction() { }
    protected virtual void OnComplete() { }
}

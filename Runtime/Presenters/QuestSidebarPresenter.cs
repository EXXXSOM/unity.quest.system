using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSidebarPresenter : MonoBehaviour
{
    [SerializeField] private int _countQuestsShow = 5;
    [SerializeField] private Transform _sidebarQuestsContainer;
    [SerializeField] private QuestTargetView _questTargetViewPrefab;

    private Dictionary<int, QuestTargetView> _showedQuestsView = new Dictionary<int, QuestTargetView>(10);

    private QuestSystem _questSystem;
    private bool _showSidebarQuests = false;

    private void Awake()
    {
        _questSystem = QuestSystem.Instance;
        _showedQuestsView = new Dictionary<int, QuestTargetView>(_countQuestsShow);

        _questSystem.OnSetupNewQuest += PresentNewQuest;
        _questSystem.OnCompleteQuest += CompleteQuest;
        _questSystem.OnSkipQuest += SkipQuest;
        _questSystem.OnUpdateQuest += UpdateQuest;

    }

    public void Show()
    {
        _sidebarQuestsContainer.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _sidebarQuestsContainer.gameObject.SetActive(false);
    }

    public void SkipQuest(int key)
    {
        RemovePresentedQuest(key);
    }

    public void UpdateQuest(int key, string description)
    {
        QuestTargetView questTargetView = null;
        if (_showedQuestsView.TryGetValue(key, out questTargetView))
        {
            questTargetView.Description.text = description;
        }
    }

    private void PresentNewQuest(int key, QuestDescription storyPartDescription, string description)
    {
        QuestTargetView questTargetView = Instantiate(_questTargetViewPrefab, _sidebarQuestsContainer);
        questTargetView.Name.text = storyPartDescription.QuestName;
        questTargetView.Description.text = description;

        _showedQuestsView.Add(key, questTargetView);
    }

    private void CompleteQuest(int key, QuestDescription storyPartDescription)
    {
        if (_showedQuestsView.ContainsKey(key))
        {
            _showedQuestsView[key].Description.fontStyle = TMPro.FontStyles.Strikethrough;
            StartCoroutine(RemovePresentedQuestWithDelay(3, key));
        }
    }

    private void RemovePresentedQuest(int key)
    {
        if (_showedQuestsView.ContainsKey(key))
        {
            QuestTargetView questTargetView = _showedQuestsView[key];
            _showedQuestsView.Remove(key);
            Destroy(questTargetView.gameObject);
        }
    }

    private IEnumerator RemovePresentedQuestWithDelay(float delay, int key)
    {
        yield return new WaitForSeconds(delay);
        RemovePresentedQuest(key);
    }
}

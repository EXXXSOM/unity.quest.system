using UnityEngine;

public abstract class QuestActionDescriptionBase : ScriptableObject
{
    [SerializeField] protected Sprite _icon;
    [SerializeField, TextArea(5,10)] protected string _description;

    public Sprite Icon => _icon;
    //public virtual string Description => _description;
}

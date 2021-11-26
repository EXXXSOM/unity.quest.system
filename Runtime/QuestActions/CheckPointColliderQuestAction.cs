using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckPointColliderQuestAction : QuestActionBase
{
    [Header("Settings checkpoint collider:")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private QuestActionDescriptionDefault _questActionDescriptionDefault;

    public override Sprite GetIcon => _questActionDescriptionDefault.Icon;
    public override string GetDescription => _questActionDescriptionDefault.GetDescription;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _layerMask)
        {
            Complete();
        }
    }
}

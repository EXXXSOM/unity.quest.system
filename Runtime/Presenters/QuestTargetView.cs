using UnityEngine;
using TMPro;

public class QuestTargetView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;

    public TMP_Text Name => _name;
    public TMP_Text Description => _description;
}

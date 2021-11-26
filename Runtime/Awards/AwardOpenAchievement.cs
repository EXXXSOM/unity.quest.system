using UnityEngine;

[CreateAssetMenu(fileName = "OpenAchievement", menuName = "StorySystem/Awards/OpenAchievement", order = 2)]
public class AwardOpenAchievement : QuestAward
{
    public override void Give()
    {
        Debug.Log("[Award] Achievement Test Example is opened");
    }
}

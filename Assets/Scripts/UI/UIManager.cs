using UnityEngine;
using VoodooDoodoo.Utils;

namespace VoodooDoodoo.UI
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        [SerializeField]
        public RequirementCardsContainer requirementCardsContainer;
        
        [SerializeField]
        public TimeLimitController timeLimitController;
    }
}

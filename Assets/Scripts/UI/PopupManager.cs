using UnityEngine;
using VoodooDoodoo.Utils;

namespace VoodooDoodoo.UI
{
    public sealed class PopupManager : SingletonMonoBehaviour<PopupManager>
    {
       [SerializeField]
       private WinPopup winPopup;
       [SerializeField]
       private LosePopup losePopup;

       protected override void Initialize ()
       {
           base.Initialize();

           winPopup.Init();
           losePopup.Init();
       }
       
       public void ShowWinPopup (float remainingTime, float maxTime)
       {
           winPopup.ShowWinPopup(remainingTime, maxTime);
       }
       
       public void ShowLosePopup (string reason)
       {
           losePopup.ShowPopup(reason);
       }
    }
}

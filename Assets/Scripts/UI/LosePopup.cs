using VoodooDoodoo.Gameplay;

namespace VoodooDoodoo.UI
{
    public sealed class LosePopup : BasePopup
    {
        public void RestartLevel ()
        {
            GameManager.Instance.RestartLevel();
            HidePopup();
        }
    }
}

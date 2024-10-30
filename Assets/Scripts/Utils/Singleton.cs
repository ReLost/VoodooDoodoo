using UnityEngine;

namespace VoodooDoodoo.Utils
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }
        
        protected void Awake ()
        {
            if (Instance == null)
            {
                Instance = this as T;
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Initialize ()
        {
            
        }

        protected void OnDestroy ()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
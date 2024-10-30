using Sirenix.OdinInspector;
using UnityEngine;

namespace VoodooDoodoo.Puzzle
{
    public sealed class PuzzleElement : MonoBehaviour
    {
        [Title("Data")]
        [SerializeField]
        public PuzzleElementData elementData;
        
        [Title("References")]
        [SerializeField]
        private Renderer renderer;
        [SerializeField]
        private Outline outline;
        [SerializeField]
        private Rigidbody rigidbody;
        [SerializeField]
        private Collider collider;

        public void SetMaterialColor (Color color)
        {
            renderer.material.color = color;
        }
        
        public void SetOutline (bool isActive)
        {
            outline.enabled = isActive;
        }

        public void SetAsCollected (bool isCollected)
        {
            rigidbody.isKinematic = isCollected;
            collider.enabled = isCollected == false;
            outline.enabled = false;
        }

        public void ResetElement ()
        {
            SetAsCollected(false);
            transform.localScale = Vector3.one * 0.5f;
        }
    }
}
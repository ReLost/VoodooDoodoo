using Sirenix.OdinInspector;
using UnityEngine;

namespace VoodooDoodoo.Puzzle
{
    [CreateAssetMenu(fileName = "PuzzleElementData", menuName = "VoodooDoodoo/PuzzleElementData")]
    public sealed class PuzzleElementData : ScriptableObject
    {
        [AssetsOnly, PreviewField(ObjectFieldAlignment.Left, Height = 125)]
        public GameObject prefab;
        public Color color;
        
        [AssetsOnly, PreviewField(ObjectFieldAlignment.Left, Height = 125)]
        public Sprite sprite;
    }
}
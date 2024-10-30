using UnityEngine;
using VoodooDoodoo.Utils;

namespace VoodooDoodoo.Puzzle
{
    public class PuzzleElementFactory : SingletonMonoBehaviour<PuzzleElementFactory>
    {
        public static GameObject CreateElement (PuzzleElementData elementData)
        {
            GameObject element = Instantiate(elementData.prefab);
            PuzzleElement puzzleElement = element.GetComponent<PuzzleElement>();
            puzzleElement.SetMaterialColor(elementData.color);
            puzzleElement.elementData = elementData;
            
            element.name = elementData.name;

            return element;
        }
    }
}
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VoodooDoodoo.Puzzle
{
    public sealed class PuzzleSpawner : MonoBehaviour
    {
        [SerializeField]
        [InlineEditor]
        private PuzzleLevelData currentLevel;

        [SerializeField]
        private List<GameObject> spawnedElements = new();
        
        [SerializeField]
        private List<Transform> spawnAreaCornerPoints = new();

        public void SetLevel (PuzzleLevelData levelData)
        {
            currentLevel = levelData;
        }
        
        public void StartCurrentLevel ()
        {
            if (spawnedElements.Count == 0)
            {
                foreach (PuzzleLevelElementData elementData in currentLevel.puzzleLevelElements)
                {
                    for (int i = 0; i < elementData.count; i++)
                    {
                        GameObject element = PuzzleElementFactory.CreateElement(elementData.elementData);
                        spawnedElements.Add(element);
                        element.transform.SetParent(transform);
                    }
                }
            }

            // Shuffle elements
            for (int i = spawnedElements.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (spawnedElements[i], spawnedElements[j]) = (spawnedElements[j], spawnedElements[i]);
            }

            foreach (GameObject element in spawnedElements)
            {
                element.transform.position = GetRandomPositionBetweenSpawnPoints();
                element.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                element.SetActive(true);
            }
        }

        private Vector3 GetRandomPositionBetweenSpawnPoints ()
        {
            float t1 = Random.Range(0f, 1f);
            float t2 = Random.Range(0f, 1f);
            
            Vector3 pointA = Vector3.Lerp(spawnAreaCornerPoints[0].position, spawnAreaCornerPoints[1].position, t1);
            Vector3 pointB = Vector3.Lerp(spawnAreaCornerPoints[2].position, spawnAreaCornerPoints[3].position, t1);
            Vector3 randomPosition = Vector3.Lerp(pointA, pointB, t2);
            
            float randomY = Random.Range(1.0f, 6.0f); 
            randomPosition.y += randomY;
            
            return randomPosition;
        }

        public void ClearLevel ()
        {
            foreach (GameObject element in spawnedElements)
            {
                Destroy(element);
            }
            
            spawnedElements.Clear();
        }
    }
}
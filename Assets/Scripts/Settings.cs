using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Settings : MonoBehaviour
    {
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard,
            Insane
        }

        public enum TargetSize
        {
            Small = 30,
            Medium = 75,
            Large = 125
        }

        [SerializeField] GameObject difficultyButton = null;
        [SerializeField] GameObject sizeButton = null;

        public Difficulty difficulty;
        public TargetSize targetSize;

        void Start()
        {
            difficulty = Difficulty.Easy;
            targetSize = TargetSize.Medium;
        }
        public void ChangeDifficulty()
        {
            if (difficultyButton != null)
            {
                List<Difficulty> difficultyList = new List<Difficulty>();
                difficultyList = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().ToList();

                difficulty = difficultyList[(difficultyList.IndexOf(difficulty) + 1) % difficultyList.Count];
                difficultyButton.GetComponentInChildren<Text>().text = difficulty.ToString();
            }
        }

        public void ChangeSize()
        {
            if (sizeButton != null)
            {
                List<TargetSize> sizeList = new List<TargetSize>();
                sizeList = Enum.GetValues(typeof(TargetSize)).Cast<TargetSize>().ToList();

                targetSize = sizeList[(sizeList.IndexOf(targetSize) + 1) % sizeList.Count];
                sizeButton.GetComponentInChildren<Text>().text = targetSize.ToString();
            }
        }
    }
}

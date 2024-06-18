using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class LevelProgressSimple
    {
        [SerializeField] private string id;
        [SerializeField] private bool isComplete;
        [SerializeField] private int foundedCount;
        [SerializeField] private long dateUpdated;
        [SerializeField] private long dateCompleted;
        [SerializeField] private long dateStarted;

        public LevelProgressSimple(string name)
        {
            id = name;
            foundedCount = -1;
        }

        public bool IsComplete
        {
            get => isComplete;
            set => isComplete = value;
        }

        public int FoundedCount
        {
            get => foundedCount;
            set => foundedCount = value;
        }

        public bool IsSimpleExist => DateStarted != default;
        public bool IsDefault => foundedCount == -1;
        public DateTime DateUpdated { get; set; }
        public DateTime DateCompleted { get; set; }
        public DateTime DateStarted { get; set; }


        public string ID
        {
            get => id;
            set => id = value;
        }
    }
}
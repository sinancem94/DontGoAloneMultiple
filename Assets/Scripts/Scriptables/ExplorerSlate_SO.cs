using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[CreateAssetMenu(fileName = "New Explorer Slate", menuName = "Board/ExplorerSlate", order = 3)]
public class ExplorerSlate_SO : ScriptableObject
{
    public enum Stats
    {
        Speed = 0,
        Might,
        Knowledge,
        Sanity
    }
    
    [System.Serializable]
    public struct ExplorerTrait
    {
        [Range(1,3)]
        public int min;
        [Range(3,6)]
        public int defaultVal;
        [Range(7,9)]
        public int max;
    }

    public string CharachterName;
    
    public ExplorerTrait Speed;
    public ExplorerTrait Might;
    public ExplorerTrait Knowledge;
    public ExplorerTrait Sanity;
}

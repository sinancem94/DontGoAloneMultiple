using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExplorerClient.ExplorerUI
{
    public class StatPanel : MonoBehaviour
    {
        private Text _characterName;
        private List<Transform> _stats;

        void Start()
        {
            _characterName = GetComponentInChildren<Text>();

            _stats = new List<Transform>();
            foreach (var statSlider in GetComponentsInChildren<Slider>())
            {
                Transform stat = statSlider.transform.parent;
                _stats.Add(stat);
            }
        }

        public void SetCharacterName(string cName)
        {
            _characterName.text = cName;
        }

        public void SetStats(ExplorerSlate_SO thisSlate)
        {
            for (int i = 0; i < _stats.Count; i++)
            {
                switch ((ExplorerSlate_SO.Stats) i)
                {
                    case ExplorerSlate_SO.Stats.Speed:
                        SetStatSlider(thisSlate.Speed, thisSlate.Speed.defaultVal , i);
                        break;
                    case ExplorerSlate_SO.Stats.Might:
                        SetStatSlider(thisSlate.Might,thisSlate.Might.defaultVal ,  i);
                        break;
                    case ExplorerSlate_SO.Stats.Knowledge:
                        SetStatSlider(thisSlate.Knowledge,thisSlate.Knowledge.defaultVal ,  i);
                        break;
                    case ExplorerSlate_SO.Stats.Sanity:
                        SetStatSlider(thisSlate.Sanity, thisSlate.Sanity.defaultVal , i);
                        break;
                    default:
                        break;
                }
            }
        }

        public void SetStat(ExplorerSlate_SO thisSlate, ExplorerSlate_SO.Stats stat, int value)
        {
            switch ((ExplorerSlate_SO.Stats) stat)
            {
                case ExplorerSlate_SO.Stats.Speed:
                    SetStatSlider(thisSlate.Speed, value, (int)stat);
                    break;
                case ExplorerSlate_SO.Stats.Might:
                    SetStatSlider(thisSlate.Might,  value, (int)stat);
                    break;
                case ExplorerSlate_SO.Stats.Knowledge:
                    SetStatSlider(thisSlate.Knowledge,  value, (int)stat);
                    break;
                case ExplorerSlate_SO.Stats.Sanity:
                    SetStatSlider(thisSlate.Sanity,  value, (int)stat);
                    break;
                default:
                    break;
            }
        }

        void SetStatSlider(ExplorerSlate_SO.ExplorerTrait trait,int currValue ,int statId)
        {
            Slider statSlider = _stats[statId].GetComponentInChildren<Slider>();

            statSlider.minValue = trait.min;
            statSlider.maxValue = trait.max;

            statSlider.value = currValue;
        
            _stats[statId].GetComponent<Text>().text = $"{(ExplorerSlate_SO.Stats)statId} (min:{trait.min}/max:{trait.max}) : {currValue}";
        }
    }
}

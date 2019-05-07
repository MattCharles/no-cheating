using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName ="New Card", menuName = "Card")]
    [InitializeOnLoad]
    public class Card : ScriptableObject
    {
        public new string name;
        public string description;
        public string tribes;

        public Sprite artwork;

        public ManaCost[] inspectorFriendlyManaCost;
        private Dictionary<Mana, int> costs = new Dictionary<Mana, int>();

        public int attack;
        public int health;
        public int movement;

        void OnEnable()
        {
            foreach(ManaCost cost in inspectorFriendlyManaCost)
            {
                costs.Add(cost.mana, cost.price);
            }
        }

        public bool HasCostIn(Mana mana)
        {
            return costs.ContainsKey(mana);
        }

        public int CostIn(Mana mana)
        {
            return costs.ContainsKey(mana) ? costs[mana] : 0;
        }

        public int ConvertedCost()
        {
            return costs.Values.Sum();
        }

        public List<Mana> ManasNeeded()
        {
            return costs.Keys.ToList();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
namespace Succesors
{
    [CreateAssetMenu(fileName = "New Stochastic Sucessor", menuName = "L-system/Sucessor/Stochastic Succesor")]
    public class StochasticSuccesor : Successor
    {
        public string[] succesors;
        [Tooltip("Value between 0, 10")]
        [Range(0, 10)]
        public int[] chances;
        [HideInInspector]
        public List<string> choices;
        public bool CalculateChances()
        {
            choices = new List<string>();
            if (succesors != null && chances != null && succesors.Length != chances.Length)
            {
                Debug.Log("You must have the same amount of each variable");
                return false;
            }
            else
            {
                for (int i = 0; i < succesors.Length; i++)
                {
                    for (int j = 0; j < (chances[i]); j++)
                    {
                        choices.Add(succesors[i]);
                    }
                }
                return true;
            }
        }

        public override string GetSuccesor()
        {
            int randValue = Random.Range(0, choices.Count);
            return choices[randValue];
        }

        public override void SetSuccesor(string value)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < succesors.Length; i++)
            {
                res += "> " + succesors[i] + " with a chance of " + chances[i] + "\n";
            }
            return res;
        }
    }
}

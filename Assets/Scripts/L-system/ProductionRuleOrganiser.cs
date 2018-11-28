using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ProductionRuleOrganiser: MonoBehaviour 
{
    public ProductionRule[] productionRules;
    public Dictionary<char, List<ProductionRule>> ruleDict;

    public bool readFromJSON;
    public string[] JSONProductionRuleFilenames;

    private void Awake()
    {
        if (readFromJSON)
        {
            productionRules = new ProductionRule[JSONProductionRuleFilenames.Length];
            for (int i = 0; i< JSONProductionRuleFilenames.Length; i++)
            {
                productionRules[i] = JSONRuleReader.ReadProductionRule(JSONProductionRuleFilenames[i]);
            }
        }
        SetupRuleDictionary();
    }
    /// <summary>
    /// Setup the dictionary with the production rule array
    /// </summary>
    public void SetupRuleDictionary()
    {
        ruleDict = new Dictionary<char, List<ProductionRule>>();
        //For each production rule that has been defined
        foreach (ProductionRule rule in productionRules)
        {
            //Check if the rule exists, get the old list, add the new rule to it, remove the old rule list and add the new one
            char rulePredecessor = rule.predecessorObject.GetPredecessor();
            if (ruleDict.ContainsKey(rulePredecessor))
            {
                List<ProductionRule> rules = new List<ProductionRule>();
                ruleDict.TryGetValue(rulePredecessor, out rules);
                rules.Add(rule);
                ruleDict.Remove(rulePredecessor);
                ruleDict.Add(rulePredecessor, rules);
            }
            else
            {
                List<ProductionRule> rules = new List<ProductionRule>();
                rules.Add(rule);
                ruleDict.Add(rulePredecessor, rules);
            }
        }
    }

    /// <summary>
    /// Get the production rulse for the sent letter
    /// </summary>
    /// <param name="letter">The letter for which we want the rule</param>
    /// <returns></returns>
    public List<ProductionRule> GetRuleForChar(char letter)
    {
        List<ProductionRule> rule;

        if (ruleDict.ContainsKey(letter))
        {
            if (ruleDict.TryGetValue(letter, out rule))
            {
                return rule;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

    }
}
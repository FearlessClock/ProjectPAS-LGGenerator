using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lsystem : MonoBehaviour {
    private string startingAxiom;
    public string axiom;
    [HideInInspector]
    public string newAxiom;
    public string[] alphabet;
    public ProductionRuleOrganiser productionRuleOrganiser;
    public TextMeshProUGUI visual;

    private bool isStepping;
    private float timer;
    public float countdownStep;

    public bool AxiomIsStepping;

    public GameObject parameterTextShowValues;
    public Transform transformOfObjects;

    public void ResetAxiom()
    {
        axiom = startingAxiom;
    }

    public void StartStepping()
    {
        isStepping = !isStepping;
    }
	// Use this for initialization
	void Start () {
        isStepping = false;
        timer = 0;
        visual.text = axiom;
        startingAxiom = axiom;
        AxiomIsStepping = false;
    }

    Coroutine updateAxiomInst = null;
	// Update is called once per frame
	void Update () {
        if (isStepping)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                if (!AxiomIsStepping)
                {
                    StartCoroutine("UpdateAxiom");
                }
                timer = countdownStep;
            }
        }
	}

    public IEnumerator StepAxiomNmbrOfTimes(int nmbrOfSteps)
    {
        int counter = nmbrOfSteps;
        int yieldStepper = 0;
        while(counter > 0)
        {
            if (StepAxiom())
            {
                counter--;
                yield return 1;
            }
            yieldStepper++;
            if(yieldStepper > 100)
            {
                yieldStepper = 0;
                yield return 1;
            }
        }
    }

    //TODO: Doesn't work
    public void LaunchUpdateAxiom()
    {
        isStepping = !isStepping;
    }

    public bool StepAxiom()
    {
        if (!AxiomIsStepping)
        {
            StartCoroutine("UpdateAxiom");
            return true;
        }
        return false;
    }

    public void StepAxiomCallback()
    {
        StartCoroutine("UpdateAxiom");
    }
     
    IEnumerator UpdateAxiom()
    {
        AxiomIsStepping = true;
        //Reset newAxiom
        newAxiom = "";

        //Check each letter and replace it with the rule if it exists
        for (int i = 0; i < axiom.Length; i++)
        {
            char currentChar = axiom[i];

            //Get the rules associated with the current char
            List<ProductionRule> currentRules = productionRuleOrganiser.GetRuleForChar(currentChar);
            if (currentRules != null)
            {
                bool aRuleWasApplied = false;
                //Check each rule to see if it is applicable for this char
                for(int j = 0; j < currentRules.Count; j++)
                {
                    ProductionRule prodRule = currentRules[j];
                    //Get the different char for the rule
                    char predecessor;
                    char preLetter;
                    char postLetter;
                    GetLetters(out predecessor, out preLetter, out postLetter, i, axiom.Length);

                    //Apply the rule to the predecessor, if the rule is applicable, apply it.
                    string replacement = prodRule.ApplyRule(predecessor, preLetter, postLetter);
                    if(replacement != null)
                    {
                        //Debug.Log("the rule " + replacement + " was applied on " + predecessor + " from " + prodRule.name);
                        newAxiom += replacement;
                        aRuleWasApplied = true;
                        break;
                    }
                }
                if (!aRuleWasApplied)
                {
                    newAxiom += currentChar;
                }
            }
            else
            {
                newAxiom += currentChar;
            }
            if(i%50 == 0)
            {
                yield return 1;
            }
        }

        axiom = newAxiom;
        visual.text = newAxiom;

        AxiomIsStepping = false;
    }

    private void GetLetters(out char preDec, out char preLet, out char postLet, int index, int axiomLength)
    {
        postLet = ' ';
        if (index + 1 <= axiomLength - 1)
        {
            postLet = axiom[index + 1];
        }

        preLet = ' ';
        if (index - 1 >= 0)
        {
            preLet = axiom[index - 1];
        }

        preDec = axiom[index];
    }

    public List<int> GetSuccessorChances()
    {
        List<int> chances = new List<int>();
        foreach (ProductionRule prod in productionRuleOrganiser.productionRules)
        {
            Succesors.StochasticSuccesor succ = (prod.succesorObject as Succesors.StochasticSuccesor);
            
            if(succ)
            {
                chances.AddRange(succ.chances);
            }
        }
        return chances;
    }

    public void SetSuccessorChances(List<int> newChances)
    {
        int counter = 0;
        foreach (Transform child in transformOfObjects)
        {
            Destroy(child.gameObject);
        }
        foreach (ProductionRule prod in productionRuleOrganiser.productionRules)
        {
            Succesors.StochasticSuccesor succ = (prod.succesorObject as Succesors.StochasticSuccesor);
            if (succ)
            {
                GameObject obj = Instantiate<GameObject>(parameterTextShowValues, transformOfObjects);
                TextMeshProUGUI textHolder = obj.GetComponent<TextMeshProUGUI>();
                textHolder.text = succ.name + ": ";
                for (int i = 0; i < succ.chances.Length; i++)
                {
                    succ.chances[i] = newChances[counter];
                    textHolder.text += succ.chances[i] + ", ";
                    counter++;
                }
            }
        }
    }
}

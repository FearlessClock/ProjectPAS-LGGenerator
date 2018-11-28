using System.IO;
using System;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

public class HandleTextFile
{
    public static void WriteString(string path, string text)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(text);
        writer.Close();
    }

    public static string ReadString(string path)
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        reader.Close();
        return text;
    }
}

class JSONRuleReader
{
    static public Predecessors.Predecessor ReadPredecessor(string path)
    {
        string jsonText = HandleTextFile.ReadString(path);
        var parsedJson = JSON.Parse(jsonText);
        if (parsedJson["type"] == "single")
        {
            Predecessors.SinglePredecessor single = ScriptableObject.CreateInstance<Predecessors.SinglePredecessor>();
            single.name = parsedJson["name"];
            single.SetPredecessor(parsedJson["predecessor"].Value[0]);
            return single;
        }
        else if (parsedJson["type"] == "context")
        {
            Predecessors.ContextSensitivePredecessor context = ScriptableObject.CreateInstance<Predecessors.ContextSensitivePredecessor>();
            context.name = parsedJson["name"];
            //These are supposed to be stored as chars, taking the first value from the string gives us that
            context.SetPredecessor(parsedJson["predecessor"].Value[0]); 
            context.SetLetters(parsedJson["preLetter"].Value, parsedJson["postLetter"].Value);

            Debug.Log(context);
            return context;
        }
        Debug.Log("The JSON was badly formed");
        return null;
    }

    static public Succesors.Successor ReadSuccessor(string path)
    {
        string jsonText = HandleTextFile.ReadString(path);
        var parsedJson = JSON.Parse(jsonText);
        if(parsedJson["type"] == "single")
        {
            Succesors.ContextFreeSuccesor single = ScriptableObject.CreateInstance<Succesors.ContextFreeSuccesor>();
            single.SetSuccesor(parsedJson["successor"].Value);

            return single;
        }
        else if(parsedJson["type"] == "stochastic")
        {
            Succesors.StochasticSuccesor stochastic = ScriptableObject.CreateInstance<Succesors.StochasticSuccesor>();
            stochastic.name = parsedJson["name"];
            //Get all the successors from the array in the JSON
            List<string> listOfSuccessors = new List<string>();
            foreach (JSONNode node in parsedJson["successors"])
            {
                listOfSuccessors.Add(node.Value);
            }
            stochastic.succesors = listOfSuccessors.ToArray();

            //Get all the chances from the array in the JSON
            List<int> listOfChances = new List<int>();

            foreach (JSONNode node in parsedJson["chances"])
            {
                listOfChances.Add((int)Convert.ToInt32(node.Value));
            }
            stochastic.chances = listOfChances.ToArray();

            stochastic.CalculateChances();

            Debug.Log(stochastic.ToString());
            return stochastic;
        }
        Debug.Log("The JSON " + path + " of type " + parsedJson["type"] + " was badly formed");
        return null;
    }

    static public ProductionRule ReadProductionRule(string path)
    {
        string jsonText = HandleTextFile.ReadString(path);
        var parsedJson = JSON.Parse(jsonText);

        ProductionRule rule = ScriptableObject.CreateInstance<ProductionRule>();
        rule.name = parsedJson["name"];
        rule.SetPredecessor(ReadPredecessor(parsedJson["predecessor"]));
        rule.SetSuccesor(ReadSuccessor(parsedJson["successor"]));

        return rule;
    }
}

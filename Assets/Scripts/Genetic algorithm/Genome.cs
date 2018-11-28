using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Genetic
{
    class Genome
    {
        public long id;
        public List<int> dna; // A list of all the parameters from all the successors in use
        public float fitness; // Value between 0, 1

        public Genome(List<int> newDna, long id)
        {
            dna = new List<int>();
            for(int i = 0; i < newDna.Count; i++)
            {
                dna.Add(UnityEngine.Random.Range(0, 10));
            }
            this.id = id;
            this.fitness = 0.1f;
        }

        public Genome(Genome father, Genome mother)
        {
            this.dna = new List<int>();
            this.id = (father.id + mother.id) / 2;
            this.fitness = 0.1f;
            RandomCrossover(father, mother);
        }

        private void RandomCrossover(Genome father, Genome mother)
        {
            for(int i = 0; i < father.dna.Count; i++)
            {
                if (UnityEngine.Random.value < 0.5f)
                {
                    this.dna.Add(father.dna[i]);
                }
                else
                {
                    this.dna.Add(mother.dna[i]);
                }
            }
        }

        /// <summary>
        /// The fitness of a level will be determined by:
        /// The number of rooms
        /// The number of Crossroads
        /// The length of each branch
        /// 
        /// To start the fitness calculations, 
        /// we need to first create the phenotype by generating a maze using the params
        /// for the L-system
        /// </summary>
        public IEnumerator CalculateFitness(Succesors.StochasticSuccesor[] successors, Lsystem lsystem, string startingAxiom, DungeonRemoval dungeonRemoval)
        {
            int counter = 0;
            foreach (Succesors.StochasticSuccesor succ in successors)
            {
                for (int i = 0; i < succ.chances.Length; i++)
                {
                    succ.chances[i] = dna[counter];
                    counter++;
                }
                succ.CalculateChances();
            }

            lsystem.axiom = startingAxiom;
            //Debug.Log("------------Starting stepping-----------");
            yield return lsystem.StepAxiomNmbrOfTimes(10);
            //Debug.Log("------------Finished stepping-----------");

            dungeonRemoval.axiom = lsystem.axiom;
            dungeonRemoval.ResetDungeon();
            dungeonRemoval.GenerateFull();
            yield return new WaitForSeconds(0.5f);
            string resultingAxiom = lsystem.axiom;
            float nmbrOfRooms = 0;
            float nmbrOfCrossRoads = 0;
            float longestBranch = 0;
            float fitnessResult = CountRules(resultingAxiom, out nmbrOfRooms, 'R', out nmbrOfCrossRoads, 'C', out longestBranch);
            this.fitness = fitnessResult;
        }

        private float CountRules(string axiom, out float nmbrOfRooms, char roomCounterChar, out float nmbrOfCrossRoads, char crossroadCounterChar, out float longestBranch)
        {
            nmbrOfRooms = 0;
            nmbrOfCrossRoads = 0;
            longestBranch = 0;
            foreach (char letter in axiom)
            {
                if (letter.Equals(roomCounterChar))
                {
                    nmbrOfRooms++;
                }
                else if (letter.Equals(crossroadCounterChar))
                {
                    //nmbrOfCrossRoads++;
                }
            }

            return (nmbrOfRooms + nmbrOfCrossRoads + longestBranch) / 3;
        }

        public override string ToString()
        {
            return "Genome " + id + ": fitness: " + fitness; 
        }
    }
}

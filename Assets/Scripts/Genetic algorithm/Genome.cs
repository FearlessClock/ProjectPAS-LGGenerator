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

        public float roomImportance;
        public float crossroadImportance;

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

        public Genome(Genome father, Genome mother, float mutateChance)
        {
            this.dna = new List<int>();
            this.id = (father.id + mother.id) / 2;
            this.fitness = 0.1f;
            RandomCrossover(father, mother);
            RandomMutate(mutateChance);
            this.crossroadImportance = father.crossroadImportance;
            this.roomImportance = father.roomImportance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mutateChance"> Mutate chance should be very small</param>
        private void RandomMutate(float mutateChance)
        {
            for (int i = 0; i < dna.Count; i++)
            {
                if(UnityEngine.Random.Range(0f, 1f) < mutateChance)
                {
                    dna[i] += UnityEngine.Random.Range(-1, 1);
                    if(dna[i] > 10)
                    {
                        dna[i] = 10;
                    }
                    else if(dna[i] < 0)
                    {
                        dna[i] = 0;
                    }
                }
            }
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
        public IEnumerator CalculateFitness(Succesors.StochasticSuccesor[] successors, Lsystem lsystem, string startingAxiom, DungeonRemoval dungeonRemoval, int nmbrOfLSystemIterations)
        {
            List<int> chances = lsystem.GetSuccessorChances();
            //Debug.Log(this.id + ") Count " + chances.Count);
            //string debug = "";
            //for (int i = 0; i < chances.Count; i++)
            //{
            //    debug += chances[i] + ", ";
            //}
            //Debug.Log("-------------------");
            //Debug.Log(debug);
            lsystem.SetSuccessorChances(dna);
            foreach (Succesors.StochasticSuccesor succ in successors)
            {
                succ.CalculateChances();
            }

            //debug = "";
            chances = lsystem.GetSuccessorChances();
            //foreach (int chance in chances)
            //{
            //    debug += chance + ", ";
            //}
            //Debug.Log(debug);
            //Debug.Log("-------------------");
            lsystem.axiom = startingAxiom;
            //Debug.Log("------------Starting stepping-----------");
            yield return lsystem.StepAxiomNmbrOfTimes(nmbrOfLSystemIterations);
            //Debug.Log("------------Finished stepping-----------");

            dungeonRemoval.axiom = lsystem.axiom;
            dungeonRemoval.ResetDungeon();
            dungeonRemoval.GenerateFull();
            yield return new WaitForSeconds(0.2f);
            string resultingAxiom = lsystem.axiom;
            float nmbrOfRooms = 0;
            float nmbrOfCrossRoads = 0;
            float longestBranch = 0;
            float fitnessResult = CountRules(resultingAxiom, out nmbrOfRooms, 'R', out nmbrOfCrossRoads, 'C', out longestBranch, 'F');
            this.fitness = fitnessResult;
        }

        private float CountRules(string axiom, out float nmbrOfRooms, char roomCounterChar, out float nmbrOfCrossRoads, char crossroadCounterChar, out float longestBranch, char corriderChar)
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
                    nmbrOfCrossRoads++;
                }
                else if (letter.Equals(corriderChar))
                {
                    longestBranch++;
                }
            }
            return ((nmbrOfRooms * roomImportance + nmbrOfCrossRoads * crossroadImportance + longestBranch) / axiom.Length) * 10;     //This returns a value between 0 and 10
        }

        public override string ToString()
        {
            return "Genome " + id + ": fitness: " + fitness; 
        }

        public string GetDNA()
        {

            string debug = "";
            foreach (int item in this.dna)
            {
                debug += item + " ";
            }
            return debug;
        }
    }
}

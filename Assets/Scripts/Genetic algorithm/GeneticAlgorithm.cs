using Genetic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class used to run the whole genetic algorithm.
/// Has a list containing the current population of Genomes
/// Has the fitessess genome in memory
/// </summary>
public class GeneticAlgorithm : MonoBehaviour {
    List<Genome> currentPopulation;
    List<Genome> previousPopulation;
    public int poolSize;
    public Succesors.StochasticSuccesor[] successors;
    public List<int> successorParameters;

    public int generationCounter;

    public Lsystem lsystem;
    public DungeonRemoval dungeonRemoval;
    //-----Flags-----
    private bool geneticAlgorithmStarted;
    private bool calculatingFitness;

    public int nmbrOfLSystemIterations;
    public float roomImportance;
    public float crossroadImportance;

    public TextMeshProUGUI generationText;

	// Use this for initialization
	void Start () {
        generationCounter = 0;
        calculatingFitness = false;
        currentPopulation = new List<Genome>();
        previousPopulation = new List<Genome>();
        successorParameters = new List<int>();
        foreach (Succesors.StochasticSuccesor succ in successors)
        {
            foreach(int param in succ.chances)
            {
                successorParameters.Add(param);
            }
        }
		for(int i = 0; i < poolSize; i++)
        {
            Genome genome = new Genome(successorParameters, i);
            genome.crossroadImportance = crossroadImportance;
            genome.roomImportance = roomImportance;
            currentPopulation.Add(genome);
        }

        StartGeneticAlgorithm();

    }

    Coroutine calculateFitnessCoroutine;
	// Update is called once per frame
	void Update () {
        if (!geneticAlgorithmStarted)
        {
            StartGeneticAlgorithm();
        }
    }

    public void StartGeneticAlgorithm()
    {
        generationCounter++;
        generationText.text = "Generation " + generationCounter.ToString();
        geneticAlgorithmStarted = true;
        previousPopulation.Clear();
        previousPopulation.AddRange(currentPopulation);
        currentPopulation.Clear();

        //Step 1: Create the pool with all the genomes.
        List<Genome> genePool = CreatePool(previousPopulation);
        //Step 2: Create the next population of genomes.
        currentPopulation = CreateNewPopulation(genePool);
        //Step 3: Calculate the fitness of each genome.
        calculateFitnessCoroutine = StartCoroutine("CalculateFitness");
    }

    private List<Genome> CreatePool(List<Genome> population)
    {
        List<Genome> genomePool = new List<Genome>();
        foreach (Genome genome in population)
        {
            //TODO: Make fit genomes much more prevalent
            //The higher the fitness of the genome, the more it is placed into the pool, the more chances it has of being selected
            for (int i = 0; i < genome.fitness * 5 +1; i++)
            {
                genomePool.Add(genome);
            }
        }
        return genomePool;
    }

    private List<Genome> CreateNewPopulation(List<Genome> pool)
    {
        List<Genome> population = new List<Genome>();
        Debug.Log("Pool size: " + pool.Count);
        for (int i = 0; i < poolSize; i++)
        {
            Genome mother = pool[Random.Range(0, pool.Count)];
            Genome father = pool[Random.Range(0, pool.Count)];

            Genome child = new Genome(father, mother, 0.01f);

            population.Add(child);
        }

        return population;
    }

    private IEnumerator CalculateFitness()
    {
        foreach (Genome genome in currentPopulation)
        {
            //Debug.Log("Calculating fitness");
            yield return genome.CalculateFitness(successors, lsystem, "FFFF", dungeonRemoval, nmbrOfLSystemIterations);
            Debug.Log("- - - - Id: " + genome.id + " is " + genome.fitness + " fit - - - - -");
        }

        geneticAlgorithmStarted = false;
        calculatingFitness = false;
        calculateFitnessCoroutine = null;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    private List<Genome> genomes = new List<Genome>();
    private int population = 50;
    private int keep_best = 10;
    private int lucky_few = 10;
    private float chance_of_mutation = 0.3f;
    private float mutationFloat = 1f;

    private void Start()
    {
        for (int i = 0; i < population; i++)
        {
            NewGenomes();
        }
        //SetBestGenomes();
        //Mutations();
    }
    public List<Genome> Genomes
    {
        get { return genomes; }
    }

    public Genome NewGenomes()
    {
        Genome newGenome = new Genome();
        newGenome.Initialize();
        genomes.Add(newGenome);
        return newGenome;
    }

    public void SetBestGenomes()
    {
        genomes.Sort(delegate (Genome aa, Genome bb)
        {
            if (aa.Score > bb.Score) return -1;
            else if (aa.Score < bb.Score) return 1;
            return 0;
        });

        while(genomes.Count > keep_best)
        {
            genomes.RemoveAt(keep_best);
        }
    }

    private Genome CopyGenome(Genome old)
    {
        Genome temp = new Genome(old);

        return temp;
    }
    
    public void Mutations()
    {
        while (genomes.Count < population - lucky_few)
        {
            int rand1 = Random.Range(0, keep_best);
            int rand2 = Random.Range(0, keep_best);
            var temp1 = CrossOver(genomes[rand1], genomes[rand2], rand1, rand2, genomes.Count);
            var temp2 = Mutate(temp1);
            genomes.Add(temp1);
        }

        while (genomes.Count < population)
        {
            genomes.Add(Mutate(genomes[Random.Range(0, genomes.Count)]));
        }

        for (int i = 0; i < genomes.Count; i++)
        {
            genomes[i].Score = 0;
        }        
    }

    private void DebugTest(string str)
    {
        for (int l = 0; l < genomes.Count; l++)
        {
            int sum = 0;
            for (int i = 0; i < genomes[0].WW1.GetLength(1); i++)
            {
                for (int j = 0; j < genomes[0].WW1.GetLength(0); j++)
                {
                    if (genomes[0].WW1[j, i] == genomes[l].WW1[j, i])
                    {
                        sum++;
                    }
                }
            }

            for (int i = 0; i < genomes[0].WW2.GetLength(1); i++)
            {
                for (int j = 0; j < genomes[0].WW2.GetLength(0); j++)
                {
                    if (genomes[0].WW2[j, i] == genomes[l].WW2[j, i])
                    {
                        sum++;
                    }
                }
            }

            for (int i = 0; i < genomes[0].WW3.GetLength(1); i++)
            {
                for (int j = 0; j < genomes[0].WW3.GetLength(0); j++)
                {
                    if (genomes[0].WW3[j, i] == genomes[l].WW3[j, i])
                    {
                        sum++;
                    }
                }
            }

            for (int i = 0; i < genomes[0].WW4.GetLength(1); i++)
            {
                for (int j = 0; j < genomes[0].WW4.GetLength(0); j++)
                {
                    if (genomes[0].WW4[j, i] == genomes[l].WW4[j, i])
                    {
                        sum++;
                    }
                }
            }
        }
    }

    public Genome CrossOver(Genome g1, Genome g2, int aa, int bb, int count)
    {
        Genome newG1 = CopyGenome(g1);
        Genome newG2 = CopyGenome(g2);

        for (int i = 0; i < g1.WW1.GetLength(1); i++)
        {
            for (int j = 0; j < g1.WW1.GetLength(0); j++)
            {
                newG1.WW1[j, i] = Random.Range(0, 2) == 0 ? newG1.WW1[j, i] : newG2.WW1[j, i];
            }
        }

        for (int i = 0; i < g1.WW2.GetLength(1); i++)
        {
            for (int j = 0; j < g1.WW2.GetLength(0); j++)
            {
                newG1.WW2[j, i] = Random.Range(0, 2) == 0 ? newG1.WW2[j, i] : newG2.WW2[j, i];
            }
        }

        for (int i = 0; i < g1.WW3.GetLength(1); i++)
        {
            for (int j = 0; j < g1.WW3.GetLength(0); j++)
            {
                newG1.WW3[j, i] = Random.Range(0, 2) == 0 ? newG1.WW3[j, i] : newG2.WW3[j, i];
            }
        }

        for (int i = 0; i < g1.WW4.GetLength(1); i++)
        {
            for (int j = 0; j < g1.WW4.GetLength(0); j++)
            {
                newG1.WW4[j, i] = Random.Range(0, 2) == 0 ? newG1.WW4[j, i] : newG2.WW4[j, i];
            }
        }

        return newG1;
    }

    public Genome Mutate(Genome g1)
    {
        Genome temp = CopyGenome(g1);

        if (Random.Range(0f, 1f) < chance_of_mutation)
        {
            for (int i = 0; i < temp.WW1.GetLength(1); i++)
            {
                for (int j = 0; j < temp.WW1.GetLength(0); j++)
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        temp.WW1[j, i] += temp.WW1[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    else
                    {
                        temp.WW1[j, i] -= temp.WW1[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                }
            }
        }

        if (Random.Range(0f, 1f) < chance_of_mutation)
        {
            for (int i = 0; i < temp.WW2.GetLength(1); i++)
            {
                for (int j = 0; j < temp.WW2.GetLength(0); j++)
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        temp.WW2[j, i] += temp.WW2[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    else
                    {
                        temp.WW2[j, i] -= temp.WW2[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                }
            }
        }

        if (Random.Range(0f, 1f) < chance_of_mutation)
        {
            for (int i = 0; i < temp.WW3.GetLength(1); i++)
            {
                for (int j = 0; j < temp.WW3.GetLength(0); j++)
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        temp.WW3[j, i] += temp.WW3[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    else
                    {
                        temp.WW3[j, i] -= temp.WW3[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                }
            }
        }

        if (Random.Range(0f, 1f) < chance_of_mutation)
        {
            for (int i = 0; i < temp.WW4.GetLength(1); i++)
            {
                for (int j = 0; j < temp.WW4.GetLength(0); j++)
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        temp.WW4[j, i] += temp.WW4[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    else
                    {
                        temp.WW4[j, i] -= temp.WW4[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                }
            }
        }

        return temp;
    }

    public void ShuffleGenome()
    {
        int random1 = 0;
        int random2 = 0;
        Genome temp = null;


        for (int i = 0; i < genomes.Count * 2; i++)
        {
            random1 = UnityEngine.Random.Range(0, genomes.Count);
            random2 = UnityEngine.Random.Range(0, genomes.Count);

            temp = genomes[random1];
            genomes[random1] = genomes[random2];
            genomes[random2] = temp;
        }
    }
}

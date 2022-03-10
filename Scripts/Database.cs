using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    float[,] offsetscore;
    int[,] offsettries;
    int totaltries;

    int bestOffset;
    int bestDist;
    int leastOffset;
    int leastDist;

    float bestScore;
    int leastTries;

    MapGrid map;

    // Start is called before the first frame update
    void Start()
    {
        offsetscore = new float[9, 2];
        offsettries = new int[9, 2];
        for (int i = 0; i < 9; i++)
        {
            offsetscore[i, 0] = 0;
            offsetscore[i, 1] = 0;
            offsettries[i, 0] = 0;
            offsettries[i, 1] = 0;

        }

        totaltries = 0;
        leastTries = 99999;

        bestOffset = 0;
        bestDist = 0;
        bestScore = Mathf.NegativeInfinity;
    }

    public void addData(int offset, int dist, float score)
    {
        totaltries++;
        offsettries[offset, dist] += 1;
        offsetscore[offset, dist] += (score - offsetscore[offset, dist]) / (float) offsettries[offset, dist];

        bestScore = Mathf.NegativeInfinity;
        leastTries = 99999;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (offsetscore[i, j] > bestScore)
                {
                    bestScore = offsetscore[i, j];
                    bestOffset = i;
                    bestDist = j;
                }

                if (offsettries[i, j] < leastTries)
                {
                    leastTries = offsettries[i, j];
                    leastOffset = i;
                    leastDist = j;
                }
            }
        }
    }

    public int getBestOffset()
    {
        return bestOffset;
    }

    public int getBestDist()
    {
        return bestDist;
    }

    public int getLeastOffset()
    {
        return leastOffset;
    }

    public int getLeastDist()
    {
        return leastDist;
    }

    public int getTotalTries()
    {
        return totaltries;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

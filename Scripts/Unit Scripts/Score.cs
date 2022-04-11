using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int scoreTotal;
    [SerializeField] private int scoreValue;

    public int getScoreValue()
    {
        return scoreValue;
    }

    public void addScoreValue(int value)
    {
        //scoreTotal += value;
        decrementRemaining();
    }

    private void decrementRemaining()
    {
        scoreTotal--;
    }

    public int getScoreTotal()
    {
        return scoreTotal;
    }
}

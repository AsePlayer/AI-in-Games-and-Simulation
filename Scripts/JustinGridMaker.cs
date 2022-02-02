using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustinGridMaker : MonoBehaviour
{
    public GameObject grid;

    // Start is called before the first frame update
    void Start()
    {
        MapGrid gen = grid.GetComponent <MapGrid>();
        gen.generate(gen.width, gen.height, gen.goals);

        for (int i = 0; i < gen.width; i++)
        {
            for (int j = 0; j < gen.height; j++)
            {
                GameObject cell = Instantiate(gen.getCell(i, j), new Vector3(i, j, 0), Quaternion.identity);
                cell.transform.parent = grid.transform;
            }
        }

        AstarPath.active = FindObjectOfType<AstarPath>(); AstarPath.active.Scan();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct POINT
{
    int x;
    int y;
}
//0,0 | 1,0 | 2,0 | 3,0 | 4,0 | 12,0 | 13.0 | 14.0 | 15.0
//0,1 | 1,2 | 1,


public class Diffusion1 : MonoBehaviour
{
    bool[,] grid;

    public void Awake()
    {
        for(int y = 0; y<16; y++)
        {
            for(int x = 0; x<16; x++)
            {
                grid[y, x] = false;
            }
        }
    }

}

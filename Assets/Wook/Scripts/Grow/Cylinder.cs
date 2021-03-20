using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public int x;
    public int y;
}

public class Cylinder : MonoBehaviour
{

    List<Point> None = new List<Point>(); //실린더의 빈공간이 담기 리스트
    Point pos;
    bool[,] cylinder = new bool[16, 16];


    public GameObject Col_Prefab;


    void Awake()
    {

    

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                cylinder[y, x] = false;
            }
        }

        None_DataSet();
        Point pos;
        pos.x = 8;
        pos.y = 8;
        Creat_Col(pos);
    }

    public void Creat_Col(Point pos)
    {
        string str;
        cylinder[pos.y, pos.x] = true;
        GameObject newObject = Instantiate(Col_Prefab);
        str = "Col (" + pos.x + ")";
        newObject.name = str;
        newObject.GetComponentInChildren<Col_Group>().cylinder = this.gameObject.GetComponent<Cylinder>();
        newObject.transform.parent = transform.GetChild(pos.y).transform;
        newObject.GetComponentInChildren<Col_Group>().SetPos(pos.x, pos.y);
        newObject.transform.localPosition = new Vector3(-pos.x, 0, 0);

    }


    void None_DataSet()
    {
        for (int i = 0; i < 5; i++)
        {
            pos.y = 0;
            pos.x = i;
            None.Add(pos);

            pos.x = i + 11;
            None.Add(pos);

            pos.y = 15;
            pos.x = i;
            None.Add(pos);

            pos.x = i + 11;
            None.Add(pos);

        }
        for (int i = 0; i < 3; i++)
        {
            pos.y = 1;
            pos.x = i;
            None.Add(pos);


            pos.x = i + 13;
            None.Add(pos);

            pos.y = 14;
            pos.x = i;
            None.Add(pos);

            pos.x = i + 13;
            None.Add(pos);
        }
        for (int i = 0; i < 2; i++)
        {
            pos.y = 2;
            pos.x = i;
            None.Add(pos);


            pos.x = i + 14;
            None.Add(pos);

            pos.y = 13;
            pos.x = i;
            None.Add(pos);

            pos.x = i + 14;
            None.Add(pos);
        }


        pos.y = 3;
        pos.x = 0;
        None.Add(pos);
        pos.x = 15;
        None.Add(pos);

        pos.y = 4;
        pos.x = 0;
        None.Add(pos);
        pos.x = 15;
        None.Add(pos);

        pos.y = 11;
        pos.x = 0;
        None.Add(pos);
        pos.x = 15;
        None.Add(pos);

        pos.y = 12;
        pos.x = 0;
        None.Add(pos);
        pos.x = 15;
        None.Add(pos);
    }


    public bool Search(Point pos) //해당 좌표에 값이 있으면 true
    {

        if (None.Contains(pos)) 
            return true;


        return cylinder[pos.y, pos.x];
    }

}

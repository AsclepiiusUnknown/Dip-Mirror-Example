using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MyBinarySearch : MonoBehaviour
{
    private int[] inputArray = { 10, 44, 50, 77, 81, 100, 300, 333 };
    public int key = 50;

    int count = 0;

    void Start()
    {
        List<int> list = new List<int>();
        ChangeList(list);

        inputArray= list.ToArray();

        int foundIndex = -1;
        System.DateTime startTime;
        System.DateTime endTime;

        startTime = System.DateTime.Now;
        for (int i = 0; i < 10; i++)
        {
            foundIndex = LinerySearchIteractive(inputArray, key);
        }
        endTime = System.DateTime.Now;
        Debug.Log("Linear: " + foundIndex + "in " + (endTime - startTime).TotalMilliseconds + " seconds");




        startTime = System.DateTime.Now;
        for (int i = 0; i < 10; i++)
        {
            foundIndex = BinarySearchRecursive(inputArray, key,0, inputArray.Length - 1);
        }
        endTime = System.DateTime.Now;
        Debug.Log("Iterative: " + foundIndex + "in " + (endTime - startTime).TotalMilliseconds + " seconds");


        //foundIndex = BinarySearchRecursive(list.ToArray(), key, 0, inputArray.Length - 1);

        //Debug.Log("Recursive: " + foundIndex);

    }

    void ChangeList(List<int> listtochange)
    {
        listtochange.Clear();
        int limit = 999999;//Random.Range(20, 9999);

        int last = 0;
        for (int n = 0; n < limit; n++)
        {
            int x = Random.Range(1, 5);
            int a = last + x;
            last = a;
            listtochange.Add(a);

            //Debug.Log(a);
        }
        Debug.Log(listtochange.Count);
    }


    public int LinerySearchIteractive(int[] inputArray, int key)
    {
        int min = 0;
        while (key <= inputArray[min])
        {
            count++;
            if (key == inputArray[min])
            {
                Debug.Log(count);
                return min;
            }
            else
            {
                min++;
            }
        }
        return -1;
    }

    public int BinarySearchIterative(int[] inputArray, int key)
    {
        int min = 0;
        int max = inputArray.Length - 1;

        while (min <= max)
        {
            int mid = (min + max) / 2;

            if(key == inputArray[mid])
            {
                return mid;
            }
            else if(key< inputArray[mid])
            {
                max = mid - 1;
            }
            else //if(key > inputArray[mid])
            {
                min = mid + 1;
            }
        }
        return -1;
    }

    public int BinarySearchRecursive(int[] inputArray, int key, int min, int max)
    {
        if(min > max)
        {
            return -1;
        }
        else
        {
            int mid = (min + max) / 2;
            if(key == inputArray[mid])
            {
                return mid;
            }
            else if(key < inputArray[mid])
            {
                return BinarySearchRecursive(inputArray, key, min, mid - 1);
            }
            else
            {
                return BinarySearchRecursive(inputArray, key, mid + 1, max);
            }
        }
    }


}

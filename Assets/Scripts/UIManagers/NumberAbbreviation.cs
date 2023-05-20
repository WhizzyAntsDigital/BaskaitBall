using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Test : MonoBehaviour
{
    void Start()
    {
        //Some tests
        Debug.Log("123000 is " + AbbrevationUtility.AbbreviateNumber(123000));
        Debug.Log("456000000 is " + AbbrevationUtility.AbbreviateNumber(456000000));
        Debug.Log("789000000000 is " + AbbrevationUtility.AbbreviateNumber(789000000000));
        Debug.Log("1000 is " + AbbrevationUtility.AbbreviateNumber(1000));
        Debug.Log("1000000 is " + AbbrevationUtility.AbbreviateNumber(1000000));
    }
}

public static class AbbrevationUtility
{
    private static readonly SortedDictionary<int, string> abbrevations = new SortedDictionary<int, string>
     {
         {1000,"K"},
         {1000000, "M" },
         {1000000000, "B" }
     };

    public static string AbbreviateNumber(float number)
    {
        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<int, string> pair = abbrevations.ElementAt(i);
            if (Mathf.Abs(number) >= pair.Key)
            {
                int roundedNumber = Mathf.FloorToInt(number / pair.Key);
                return roundedNumber.ToString() + pair.Value;
            }
        }
        return number.ToString();
    }
}
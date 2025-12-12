using System;
using System.Globalization;

public static class BoxingDemo
{
    public static void Main()
    {
        int total = 0;
        for (int i = 0; i < 5; i++)
        {
            // Likely boxing: composite formatting to object params
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}", i));

            // Interpolation may box when it flows to object[]; here it becomes string via int.ToString() -> no extra box
            Console.WriteLine($"{i}");

            // Explicit non-boxing path
            Console.WriteLine(i.ToString(CultureInfo.InvariantCulture));

            total += i;
            Console.WriteLine("Running total=" + total);
        }
        Console.WriteLine("Total=" + total);
    }
}
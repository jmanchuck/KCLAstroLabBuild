// Created by jmanchuck January 2021

using System.Collections.Generic;
class RecentTimes
{
    private static int capacity = 5;
    private static Queue<float> times = new Queue<float>();

    public static void Push(float time)
    {
        times.Enqueue(time);
        if (times.Count > capacity)
        {
            times.Dequeue();
        }
    }

    public static Queue<float> GetTimes()
    {
        return times;
    }

    public static void Reset()
    {
        times = new Queue<float>();
    }

}
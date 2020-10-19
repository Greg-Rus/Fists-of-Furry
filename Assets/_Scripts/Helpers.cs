using UnityEngine;

public static class Helpers
{
    public static bool InRange(this float value, float min, float max)
    {
        return value > min && value < max;
    }

    public static bool InTolerance(this float value, float goal, float tolerance)
    {
        var first = goal - tolerance;
        var second = goal + tolerance;
        return InRange(value, Mathf.Min(first, second), Mathf.Max(first, second));
    }

    public static float Sqrd(this float f)
    {
        return f * f;
    }
}
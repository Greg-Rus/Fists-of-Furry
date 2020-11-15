using System;
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

    public static Vector3 SnapPositionToUnitFromTarget(Vector3 ownPosition, Vector3 targetPosition)
    {
        var directionFromTargetToSelf = ownPosition - targetPosition;
        return targetPosition + directionFromTargetToSelf.normalized;
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        if(alpha < 0 || alpha > 1) throw new ArgumentException("Alpha must be 0-1");
        
        return new Color(color.r, color.g, color.b, alpha);
    }
}
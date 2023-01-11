namespace MathSearch.Expression;
public enum MathType {
    Nothing     = 0b000,
    Set         = 0b001,
    Boolean     = 0b010,
    Universe    = 0b111,
}

public static class UtilMathType {
    public static bool IsSubtypeOf(this MathType sub, MathType super) {
        return sub.HasFlag(super);
    }

    public static bool IsProperSubtypeOf(this MathType sub, MathType super) {
        return sub != super && IsSubtypeOf(sub, super);
    }

    public static MathType Intersect(this MathType type, MathType other) {
        return type & other;
    }

    public static MathType Union(this MathType type, MathType other) {
        return type | other;
    }
}

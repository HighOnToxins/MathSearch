using MathSearch.Expressions;

namespace MathSearch.Expression;

public enum MathType {
    Nothing = 0b000,
    Set = 0b001,
    Boolean = 0b010,
    Universe = 0b111,
}

public static class UtilMathType {

    public static bool TryContains(this MathType type, MathExpression expression, out bool result, MathSystem? context = null) {
        MathType expressionType = context == null ? expression.DetermineType() : context.DetermineType(expression);

        if(expressionType.IsSubTypeOf(type)) {
            result = true;
            return true;
        }

        if(!expressionType.Overlaps(type)) {
            result = false;
            return true;
        }

        result = default;
        return false;
    }

    public static bool IsSubTypeOf(this MathType sub, MathType super) {
        return sub.IntersectWith(super) == sub;
    }

    public static bool IsSuperTypeOf(this MathType super, MathType sub) {
        return sub.IntersectWith(super) == sub;
    }

    public static bool Overlaps(this MathType type, MathType other) {
        return type.IntersectWith(other) != MathType.Nothing;
    }

    public static MathType IntersectWith(this MathType type, MathType other) {
        return type & other;
    }

    public static MathType UnionWith(this MathType type, MathType other) {
        return type | other;
    }
}

namespace MathSearch.Expression;

public enum ExpressionType {
    Nothing,
    Set,
    Boolean,
    Universe,
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class IndependentAttribute: Attribute {

    public ExpressionType Type { get; private init; }

    public IndependentAttribute(ExpressionType type) {
        Type = type;
    }
}
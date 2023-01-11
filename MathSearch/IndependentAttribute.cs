namespace MathSearch.Expression;

public enum MathType {
    Nothing,
    Set,
    Boolean,
    Universe,
}

//TODO: Remove attribute.

[AttributeUsage(AttributeTargets.Class)]
public sealed class IndependentAttribute: Attribute {

    public MathType Type { get; private init; }

    public IndependentAttribute(MathType type) {
        Type = type;
    }
}
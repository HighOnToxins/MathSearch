namespace MathSearch.Expressions;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PrecedenceAttribute: Attribute {
    internal int Value { get; }

    public PrecedenceAttribute(int value) {
        Value = value;
    }
}
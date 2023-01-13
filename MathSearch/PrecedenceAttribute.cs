namespace MathSearch.Expressions;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class PrecedenceAttribute: Attribute {
    internal int Value { get; }

    public PrecedenceAttribute(int value) {
        Value = value;
    }
}
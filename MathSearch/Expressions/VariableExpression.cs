
namespace MathSearch.Expressions;

public sealed class VariableExpression: AtomExpression<string> {
    public VariableExpression(string value) : base(value) {
    }

    public override MathExpression Clone() => 
        new VariableExpression(Value);
}

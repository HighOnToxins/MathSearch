
using MathSearch.Expression;

namespace MathSearch.Expressions;

public sealed class VariableExpression: AtomExpression<string> {
    public override MathType Type => MathType.Universe;

    public VariableExpression(string value) : base(value) {
    }

    public override MathExpression Clone() => 
        new VariableExpression(Value);
}


using MathSearch.Expression;

namespace MathSearch.Expressions;

public sealed class VariableExpression: AtomExpression<string>, INonSimpleExpression {

    public VariableExpression(string value) : base(value) {
    }

    public override MathExpression Clone() => new VariableExpression(Value);

    protected override MathType ComputeType(MathSystem context) => MathType.Universe;
}

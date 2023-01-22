
namespace MathSearch.Expressions.Propersitions;

public sealed class BooleanExpression: AtomExpression<bool> {

    public BooleanExpression(bool value) : base(value) {
    }

    public override MathExpression Clone() => new BooleanExpression(Value);

}

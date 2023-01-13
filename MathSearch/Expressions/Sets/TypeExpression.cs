
using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions.Sets;

public sealed class TypeExpression: AtomExpression<MathType> {
    public override MathType Type => MathType.Set;

    public TypeExpression(MathType value) : base(value) {
    }


    public override MathExpression Simplify(MathSystem? context = null) {
        if(Value == MathType.Boolean) {
            return new SetExpression(new BooleanExpression(false), new BooleanExpression(true));
        } else {
            return base.Simplify(context);
        }
    }

    public override MathExpression Clone() =>
        new TypeExpression(Value);

}

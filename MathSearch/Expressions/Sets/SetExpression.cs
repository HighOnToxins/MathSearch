using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions.Sets;

public sealed class SetExpression: UnorderedExpression {

    public override int Precedence => 0;

    public SetExpression(params MathExpression[] operands) : base(operands) { }

    public SetExpression(IEnumerable<MathExpression> operands) : base(operands) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> operands, MathSystem context) => true;

    protected override bool TrySimplify(ref IEnumerable<MathExpression> operands, MathSystem context, out MathExpression? result) {
        result = null;
        return false;

        //TODO: Simplify set expressions into type expressions
        /*
        if(Value == MathType.Boolean) {
            return new SetExpression(new BooleanExpression(false), new BooleanExpression(true));
        } else if(Value == MathType.Nothing) {
            return new SetExpression();
        } else {
            return base.Simplify(context);
        }
        */
    }

    protected override MathType ComputeType(MathSystem context) => MathType.Set;

    public override IEnumerable<MathType> TypeOfOperands(MathType typeOfThis) {
        foreach(MathExpression _ in operands) {
            yield return MathType.Universe;
        }
    }

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) => new SetExpression(children);

}

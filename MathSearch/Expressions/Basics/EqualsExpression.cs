
using MathSearch.Expression;

namespace MathSearch.Expressions.Basics;

public sealed class EqualsExpression: OperatorExpression {

    public override int Precedence => 6;

    public IReadOnlyList<MathExpression> Children => operands;

    public EqualsExpression(params MathExpression[] children) : base(children) { }

    public EqualsExpression(IEnumerable<MathExpression> children) : base(children) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) => true;

    protected override bool TrySimplify(ref IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) {

        if(children.Count() == 1) {
            result = children.First();
            return true;
        }

        //TODO: Add equality check that also uses the context.

        result = null;
        return false;
    }

    protected override MathType ComputeType(MathSystem context) {
        throw new NotImplementedException();
    }

    public override IEnumerable<MathType> TypeOfOperands(MathType typeOfThis) {
        foreach(MathExpression _ in operands) {
            yield return MathType.Universe;
        }
    }

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) => new EqualsExpression(children);


    //TODO: add context stuff for in expression

}

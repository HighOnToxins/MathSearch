using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

[Precedence(1)]
public class NotExpression: UnaryExpression {
    public NotExpression(MathExpression child) : base(child) {
    }

    protected override bool Condition(MathExpression child, MathSystem context) =>
        MathType.Boolean.Contains(child, context);

    protected override bool TrySimplify(MathExpression child, MathSystem context, out MathExpression? result) {

        //Destribute on conjunction

        //Destribute on disjunction

        //remove double not
        if(child is NotExpression notExpression) {
            result = notExpression.Child.Simplify(context);
            return true;
        }

        //evaluate
        if(child is BooleanExpression booleanExpression) {
            result = new BooleanExpression(!booleanExpression.Value);
            return true;
        }

        result = null;
        return false;

    }

    protected override MathType ComputeType(MathExpression child, MathSystem context) => MathType.Boolean;

    protected override MathExpression CreateInstance(MathExpression child) => new NotExpression(child);

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) { yield break; }
}

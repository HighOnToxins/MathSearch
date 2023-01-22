using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

public class NotExpression: UnaryExpression {

    public override int Precedence => 1;

    public NotExpression(MathExpression child) : base(child) {
    }

    protected override bool ConditionIsMet(MathExpression child, MathSystem context) =>
        MathType.Boolean.TryContains(child, out bool result, context) && result;

    protected override bool TrySimplify(MathExpression child, MathSystem context, out MathExpression? result) {

        //TODO: add destribute on conjunction

        //TODO: add destribute on disjunction

        //remove double not
        if(child is NotExpression notExpression) {
            result = notExpression.Operand.Simplify(context);
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

    protected override MathType ComputeType(MathSystem context) => MathType.Boolean;

    public override IEnumerable<MathType> TypeOfOperands(MathType typeOfThis) {
        if(typeOfThis.IsSubTypeOf(MathType.Boolean)) {
            yield return MathType.Boolean;
        } else {

        }
    }

    protected override MathExpression CreateInstance(MathExpression operand) => new NotExpression(operand);

}

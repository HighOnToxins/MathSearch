
using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

public class ConjunctionExpression: UnorderedExpression {

    public override int Precedence => 2;

    public ConjunctionExpression(params MathExpression[] operands) : base(operands) { }

    public ConjunctionExpression(IEnumerable<MathExpression> operands) : base(operands) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> operands, MathSystem context) =>
        operands.All(e => MathType.Boolean.TryContains(e, out bool result, context) && result);

    protected override bool TrySimplify(ref IEnumerable<MathExpression> operands, MathSystem context, out MathExpression? result) {
        operands = operands
            .Where(e => e is not BooleanExpression booleanExpression || !booleanExpression.Value)
            .SelectMany(e => e.ExtractOperandsOf<ConjunctionExpression>());

        if(operands.Count() == 1) {
            result = operands.First();
            return true;
        } else if(operands.All(e => e is BooleanExpression booleanExpression && booleanExpression.Value)) {
            result = new BooleanExpression(true);
            return true;
        } else if(operands.Any(e => e is BooleanExpression booleanExpression && !booleanExpression.Value)) {
            result = new BooleanExpression(false);
            return true;
        }

        result = Factorize<DisjunctionExpression>().Simplify();
        return true;
    }

    protected override MathType ComputeType(MathSystem context) => MathType.Boolean;

    public override IEnumerable<MathType> TypeOfOperands(MathType typeOfThis) {
        if(typeOfThis.IsSubTypeOf(MathType.Boolean)) {
            foreach(MathExpression _ in operands) {
                yield return MathType.Boolean;
            }
        } else {
            foreach(MathExpression _ in operands) {
                yield return MathType.Universe;
            }
        }
    }

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> operands) => new ConjunctionExpression(operands);


}

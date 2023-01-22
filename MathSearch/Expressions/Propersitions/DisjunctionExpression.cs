
using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

public sealed class DisjunctionExpression: UnorderedExpression {

    public override int Precedence => 3;

    public DisjunctionExpression(params MathExpression[] operands) : base(operands) { }

    public DisjunctionExpression(IEnumerable<MathExpression> operands) : base(operands) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> Operands, MathSystem context) =>
        Operands.All(e => MathType.Boolean.TryContains(e, out bool result, context) && result);

    protected override bool TrySimplify(ref IEnumerable<MathExpression> operands, MathSystem context, out MathExpression? result) {
        operands = operands
            .Where(e => e is not BooleanExpression booleanExpression || booleanExpression.Value)
            .SelectMany(e => e.ExtractOperandsOf<DisjunctionExpression>());

        if(operands.Count() == 1) {
            result = operands.First();
            return true;
        } else if(operands.All(e => e is BooleanExpression booleanExpression && !booleanExpression.Value)) {
            result = new BooleanExpression(false);
            return true;
        } else if(operands.Any(e => e is BooleanExpression booleanExpression && booleanExpression.Value)) {
            result = new BooleanExpression(true);
            return true;
        }

        result = null;
        return false;
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

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> operands) => new DisjunctionExpression(operands);

}

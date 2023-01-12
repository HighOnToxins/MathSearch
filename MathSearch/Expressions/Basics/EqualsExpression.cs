
using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions.Basics;

[Precedence(6)]
public sealed class EqualsExpression: OperatorExpression {

    public EqualsExpression(params MathExpression[] children) : base(children.OrderBy(e => e)) { }

    public EqualsExpression(IEnumerable<MathExpression> children) : base(children.OrderBy(e => e)){ }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) => true;

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected override bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) {

        if(children.Count() == 1) {
            result = new BooleanExpression(true); //TODO: FIX: ?? things got removed, because it is a hash set ??
            return true;
        } else if(context.TryEvaluateEquality(children, out bool equalityResult)) {
            result = new BooleanExpression(equalityResult);
            return true;
        } else if(children.All(e => e is AtomExpression atomExpression)) {
            result = null;
            return false;
        }

        result = null; 
        return false;
    }

    protected override MathType ComputeType(IEnumerable<MathExpression> children, MathSystem context) => MathType.Boolean;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) => new EqualsExpression(children);

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) { yield break; }

}

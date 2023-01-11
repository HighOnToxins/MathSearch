
using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions;

public sealed class MathSystem: OperatorExpression {

    public override IReadOnlySet<MathExpression> Children { get; }

    public bool IsInconsistent { get => Children.Contains(new BooleanExpression(false)); }

    public MathSystem(params MathExpression[] children) {
        Children = new HashSet<MathExpression>(children);
    }

    public MathSystem(IEnumerable<MathExpression> children) {
        Children = new HashSet<MathExpression>(children);
    }

    public MathSystem(HashSet<MathExpression> children) {
        Children = children;
    }

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, Context context) => true;

    protected override bool TrySimplify(IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result) {
        result = null;
        return false;
    }

    protected override MathType ComputeType(IEnumerable<MathExpression> children, Context context) => MathType.Boolean;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) =>
        new MathSystem(children);

    protected override IEnumerable<MathExpression> AddToContext(IEnumerable<MathExpression> children) => children;
}

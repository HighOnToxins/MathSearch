using MathSearch.Expression;

namespace MathSearch.Expressions.Sets;

public sealed class SetExpression: GroupExpression {

    public override HashSet<MathExpression> Children { get; }

    public SetExpression(params MathExpression[] children) {
        Children = new(children);
    }

    public SetExpression(HashSet<MathExpression> children) {
        Children = children;
    }

    protected override MathType ComputeType(IEnumerable<MathType> simplifiedChild, Context context) =>
        MathType.Set;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren) =>
        new SetExpression(Children);

    protected override bool Condition(IEnumerable<MathExpression> children, Context context) {
        return false;
    }

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected override bool TrySimplify(IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result) {
        result = null;
        return false;
    }

    public override void AddToContext(Context context) { }

    public override Context CreateSubContext(Context context, IEnumerable<MathExpression> expressions) {
        return context;
    }
}

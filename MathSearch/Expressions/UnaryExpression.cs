using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class UnaryExpression: OperatorExpression {

    private static T GetChild<T>(IEnumerable<T> children) => children.First();

    public MathExpression Child { get => GetChild(children); }

    public override int ChildCount => 1;

    public UnaryExpression(MathExpression child) : base(child) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) =>
        Condition(GetChild(children), context);

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected abstract bool Condition(MathExpression child, MathSystem context);

    protected abstract bool TrySimplify(MathExpression child, MathSystem context, out MathExpression? result);

    protected override bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) =>
        TrySimplify(GetChild(children), context, out result);

    protected override MathType DetermineType(IEnumerable<MathExpression> children, MathSystem context) =>
        ComputeType(GetChild(children), context);

    protected abstract MathType ComputeType(MathExpression child, MathSystem context);
    protected abstract MathExpression CreateInstance(MathExpression child);

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) =>
        CreateInstance(GetChild(children));

}
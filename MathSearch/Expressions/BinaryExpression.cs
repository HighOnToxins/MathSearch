using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class BinaryExpression: OperatorExpression {

    private static T GetLeftChild<T>(IEnumerable<T> children) => children.First();
    private static T GetRightChild<T>(IEnumerable<T> children) => children.ElementAt(1);

    public MathExpression LeftChild { get => GetLeftChild(children); }

    public MathExpression RightChild { get => GetRightChild(children); }

    public override int ChildCount => 2;

    public BinaryExpression(MathExpression leftChild, MathExpression rightChild) : base(leftChild, rightChild) {

    }

    protected abstract bool Condition(MathExpression leftChild, MathExpression rightChild, MathSystem context);

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) =>
        Condition(GetLeftChild(children), GetRightChild(children), context);

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected abstract bool TrySimplify(MathExpression leftChild, MathExpression rightChild, MathSystem context, out MathExpression? result);

    protected override bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) =>
        TrySimplify(GetLeftChild(children), GetRightChild(children), context, out result);

    protected abstract MathType ComputeType(MathExpression leftChild, MathExpression rightChild, MathSystem context);

    protected override MathType DetermineType(IEnumerable<MathExpression> children, MathSystem context) =>
        ComputeType(GetLeftChild(children), GetRightChild(children), context);

    protected abstract MathExpression CreateInstance(MathExpression leftChild, MathExpression rightChild);

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) =>
        CreateInstance(GetLeftChild(children), GetRightChild(children));

}

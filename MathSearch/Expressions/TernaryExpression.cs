using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class TernaryExpression : OperatorExpression{

    private static T GetLeftChild<T>(IEnumerable<T> children) => children.First();
    private static T GetMiddleChild<T>(IEnumerable<T> children) => children.ElementAt(1);
    private static T GetRightChild<T>(IEnumerable<T> children) => children.ElementAt(2);
    private static void SetLeftChild<T>(T[] children, T expression) { children[0] = expression; }
    private static void SetMiddleChild<T>(T[] children, T expression) { children[1] = expression; }
    private static void SetRightChild<T>(T[] children, T expression) { children[2] = expression; }

    public override IReadOnlyList<MathExpression> Children { get; }

    public MathExpression LeftChild { get => GetLeftChild(Children); }

    public MathExpression MiddleChild { get => GetMiddleChild(Children); }

    public MathExpression RightChild { get => GetRightChild(Children); }

    public override int ChildCount => 3;

    public TernaryExpression(MathExpression leftChild, MathExpression middleChild, MathExpression rightChild) {
        MathExpression[] children = new MathExpression[ChildCount];
        Children = children;
        SetLeftChild(children, leftChild);
        SetMiddleChild(children, middleChild);
        SetRightChild(children, rightChild);
    }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) =>
        Condition(GetLeftChild(children), GetMiddleChild(children), GetRightChild(children), context);

    protected abstract bool Condition(MathExpression leftChild, MathExpression middleChild, MathExpression rightChild, MathSystem context);

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected override bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) =>
        TrySimplify(GetLeftChild(children), GetMiddleChild(children), GetRightChild(children), context, out result);

    protected abstract bool TrySimplify(MathExpression leftChild, MathExpression middleChild, MathExpression rightChild, MathSystem context, out MathExpression? result);

    protected override MathType ComputeType(IEnumerable<MathExpression> children, MathSystem context) =>
        ComputeType(GetLeftChild(children), GetMiddleChild(children), GetRightChild(children), context);

    protected abstract MathType ComputeType(MathExpression leftChild, MathExpression middleChild, MathExpression rightChild, MathSystem context);

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) =>
        CreateInstance(GetLeftChild(children), GetMiddleChild(children), GetRightChild(children));

    protected abstract MathExpression CreateInstance(MathExpression leftChild, MathExpression middleChild, MathExpression rightChild);

}

using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;

namespace MathSearch.Expressions;

public abstract class BinaryExpression : GroupExpression{

    private static T GetLeftChild<T>(IEnumerable<T> children) => children.First();
    private static T GetRightChild<T>(IEnumerable<T> children) => children.ElementAt(1);
    private static void SetLeftChild<T>(T[] children, T expression) { children[0] = expression; }
    private static void SetRightChild<T>(T[] children, T expression) { children[1] = expression; }

    public override IReadOnlyList<MathExpression> Children { get; }

    public MathExpression LeftChild { get => GetLeftChild(Children); }

    public MathExpression RightChild { get => GetRightChild(Children); }

    public override int ChildCount => 2;

    public BinaryExpression(MathExpression leftChild, MathExpression rightChild) {
        MathExpression[] children = new MathExpression[ChildCount];
        Children = children;
        SetLeftChild(children, leftChild);
        SetRightChild(children, rightChild);
    }

    protected override bool Condition(IEnumerable<MathExpression> children, Context context) =>
        Condition(GetLeftChild(children), GetRightChild(children), context);

    protected abstract bool Condition(MathExpression leftChild, MathExpression rightChild, Context context);

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected override bool TrySimplify(IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result) =>
        TrySimplify(GetLeftChild(simplifiedChildren), GetRightChild(simplifiedChildren), context, out result);

    protected abstract bool TrySimplify(MathExpression simplifiedLeft, MathExpression simplifiedRight, Context context, out MathExpression? result);

    protected override MathType ComputeType(IEnumerable<MathType> childTypes, Context context) =>
        ComputeType(GetLeftChild(childTypes), GetRightChild(childTypes), context);

    protected abstract MathType ComputeType(MathType leftType, MathType rightType, Context context);

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren) =>
        CreateInstance(GetLeftChild(simplifiedChildren), GetRightChild(simplifiedChildren));

    protected abstract MathExpression CreateInstance(MathExpression leftChild, MathExpression rightChild);

    public override void AddToContext(Context context) {
        if(RightChild is TypeExpression typeExpression) {
            context.AddType(LeftChild, typeExpression.Value);
        }
    }

}

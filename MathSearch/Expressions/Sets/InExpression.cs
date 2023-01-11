
using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions.Sets;

public sealed class InExpression: BinaryExpression {
    public InExpression(MathExpression leftChild, MathExpression rightChild) : base(leftChild, rightChild) {
    }

    protected override bool Condition(MathExpression leftChild, MathExpression rightChild, Context context) =>
        rightChild.DetermineType(context) == MathType.Set;

    protected override bool TrySimplify(MathExpression simplifiedLeft, MathExpression simplifiedRight, Context context, out MathExpression? result) {

        if(simplifiedRight is SetExpression setExpression) {
            result = new BooleanExpression(setExpression.Children.Contains(simplifiedLeft));
            return true;
        }

        //Check context for typeInfo or in equality with true or false

        //TODO: Add SetBuilder extractor.

        //if(simplifiedRightChild is SetExpression setExpression) {
        //    IEnumerable<MathExpression> children = setExpression.Children.Select(e => new EqualsExpression(LeftChild, e));
        //    result = new DisjunctionExpression(children);
        //}

        result = null;
        return false;
    }

    protected override MathType ComputeType(MathType leftType, MathType rightType, Context context) =>
        rightType == MathType.Set ? MathType.Boolean : MathType.Universe;

    protected override MathExpression CreateInstance(MathExpression leftChild, MathExpression rightChild) =>
        new InExpression(leftChild, rightChild);

    public override void AddToContext(Context context) {
        if(RightChild is TypeExpression typeExpression) {
            context.AddType(LeftChild, typeExpression.Value);
        }
    }

    public override Context CreateSubContext(Context context, IEnumerable<MathExpression> expressions) {
        return context;
    }
}


using MathSearch.Expression;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions.Sets;

public sealed class InExpression: BinaryExpression {
    public InExpression(MathExpression leftChild, MathExpression rightChild) : base(leftChild, rightChild) {
    }

    public override bool Condition(in MathExpression leftChild, in MathExpression rightChild, Context context) =>
        rightChild.DetermineType(context) == ExpressionType.Set;

    public override bool TryDetermineType(in ExpressionType leftType, in ExpressionType rightType, Context context, out ExpressionType result) {
        result = ExpressionType.Boolean;
        return true;
    }

    public override bool TrySimplifyChildren(in MathExpression leftChild, in MathExpression rightChild, Context context, out MathExpression? leftResult, out MathExpression? rightResult) {
        leftResult = null;
        rightResult = null;
        return false;
    }

    public override bool TryCompute(MathExpression leftChild, in MathExpression rightChild, out MathExpression? result) {
        if(rightChild is SetExpression setExpression) {
            result = new BooleanExpression(setExpression.Children.Contains(leftChild));
            return true;
        }

        result = null;
        return false;
    }

    public override bool TrySimplifyDown(in MathExpression leftChild, in MathExpression rightChild, Context context, out MathExpression? result) {
        result = null;
        return false;
    }

    public override bool TrySimplifyUp(in MathExpression simplifiedLeftChild, in MathExpression simplifiedRightChild, Context context, out MathExpression? result) {

        if(simplifiedRightChild is SetExpression setExpression) {
            IEnumerable<MathExpression> children = setExpression.Children.Select(e => new EqualsExpression(LeftChild, e));
            result = new DisjunctionExpression(children);
        }

        result = null;
        return false;
    }

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

using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class BinaryExpression : MathExpression{

    public MathExpression LeftChild { get; private init; }

    public MathExpression RightChild { get; private init; }

    public override int ChildCount => 2;

    public BinaryExpression(MathExpression leftChild, MathExpression rightChild) {
        LeftChild = leftChild;
        RightChild = rightChild;
    }

    public override IEnumerable<MathExpression> GetChildren() { 
        yield return LeftChild;
        yield return RightChild;
    }

    public override MathExpression Simplify(Context? context = null) {
        context ??= new();

        //down
        if(context.TryDetermineReplacement(this, out MathExpression? replacement) && replacement != null) {
            return replacement;
        }

        MathExpression leftChild = LeftChild;
        MathExpression rightChild = RightChild;

        if(!Condition(leftChild, rightChild, context)) {
            return Clone();
        }

        if(TrySimplifyChildren(leftChild, rightChild, context, out MathExpression? result1Left, out MathExpression? result1Right)) {
            if(result1Left != null) {
                leftChild = result1Left;
            }

            if(result1Right != null) {
                rightChild = result1Right;
            }
        }

        if(TrySimplifyDown(leftChild, rightChild, context, out MathExpression? result2) && result2 != null) {
            return result2;
        }

        if(TryCompute(leftChild, rightChild, out MathExpression? result3) && result3 != null) {
            return result3;
        }

        //children
        MathExpression simplifiedLeftChild = leftChild.Simplify(CreateSubContext(context, new MathExpression[] { RightChild }));
        MathExpression simplifiedRightChild = rightChild.Simplify(CreateSubContext(context, new MathExpression[] { LeftChild }));

        //up
        if(!Condition(simplifiedLeftChild, simplifiedRightChild, context)) {
            return CreateInstance(simplifiedLeftChild, simplifiedRightChild);
        }

        if(TrySimplifyChildren(simplifiedLeftChild, simplifiedRightChild, context, out MathExpression? result4Left, out MathExpression? result4Right)) {
            if(result4Left != null) {
                simplifiedLeftChild = result4Left;
            }
            if(result4Right != null) {
                simplifiedRightChild = result4Right;
            }
        }

        if(TrySimplifyUp(simplifiedLeftChild, simplifiedRightChild, context, out MathExpression? result5) && result5 != null) {
            return result5;
        }

        if(TryCompute(simplifiedLeftChild, simplifiedRightChild, out MathExpression? result6) && result6 != null) {
            return result6;
        }

        return CreateInstance(simplifiedLeftChild, simplifiedRightChild);
    }

    public abstract bool Condition(in MathExpression leftChild, in MathExpression rightChild, Context context);

    public abstract bool TrySimplifyChildren(in MathExpression leftChild, in MathExpression rightChild, Context context, out MathExpression? leftResult, out MathExpression? rightResult);

    public abstract bool TrySimplifyDown(in MathExpression leftChild, in MathExpression rightChild, Context context, out MathExpression? result);

    public abstract bool TrySimplifyUp(in MathExpression simplifiedLeftChild, in MathExpression simplifiedRightChild, Context context, out MathExpression? result);

    public abstract bool TryCompute(MathExpression leftChild, in MathExpression rightChild, out MathExpression? result);

    public override ExpressionType DetermineType(Context? context = null) {
        context ??= new();

        //TODO: Check for the actual output, and determine which of the type are the smallest

        if(context.TryDetermineType(this, out ExpressionType type)) {
            return type;
        }

        if(Condition(LeftChild, RightChild, context) &&
            TryDetermineType(
                LeftChild.DetermineType(CreateSubContext(context, new MathExpression[] { RightChild })),
                RightChild.DetermineType(CreateSubContext(context, new MathExpression[] { LeftChild})), 
                context, out ExpressionType result)) {
            return result;
        } else {
            return ExpressionType.Universe;
        }
    }

    public abstract bool TryDetermineType(in ExpressionType leftType, in ExpressionType rightType, Context context, out ExpressionType result);

    protected abstract MathExpression CreateInstance(MathExpression leftChild, MathExpression rightChild);

    public override MathExpression Clone() => CreateInstance(LeftChild, RightChild);

}

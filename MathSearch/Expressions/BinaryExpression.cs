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

    //TODO: Use a better method for creating expressions of different sizes.

    public override MathExpression Simplify(Context? context = null) {
        context ??= new();
        return context.ReplaceEquality(EvaluateSimplification(context));
    }

    private MathExpression EvaluateSimplification(Context context) {

        if(!Condition(LeftChild, RightChild, context)) {
            return Clone();
        }

        //down
        if(Condition(LeftChild, RightChild, context) &&
                TrySimplify(LeftChild, RightChild, context, out MathExpression? resultDown) && resultDown != null) {
            return resultDown;
        }

        //children
        MathExpression simplifiedLeft = LeftChild.Simplify(CreateSubContext(context, new MathExpression[] { RightChild }));
        MathExpression simplifiedRight = RightChild.Simplify(CreateSubContext(context, new MathExpression[] { LeftChild }));

        if(Condition(simplifiedLeft, simplifiedRight, context) && 
                TrySimplify(simplifiedLeft, simplifiedRight, context, out MathExpression? resultUp) && resultUp != null) {
            return resultUp;
        }

        return CreateInstance(simplifiedLeft, simplifiedRight);
    }

    public abstract bool Condition(MathExpression leftChild, MathExpression rightChild, Context context);

    public abstract bool TrySimplify(MathExpression leftChild, MathExpression rightChild, Context context, out MathExpression? result);

    public override MathType DetermineType(Context? context = null) {
        context ??= new();

        //TODO: Check for the actual output, and determine which of the type are the smallest

        if(Condition(LeftChild, RightChild, context)) {
            return TryDetermineType(
                LeftChild.DetermineType(CreateSubContext(context, new MathExpression[] { RightChild })),
                RightChild.DetermineType(CreateSubContext(context, new MathExpression[] { LeftChild })),
                context);
        } else {
            return context.DetermineType(this);
        }
    }

    public abstract MathType TryDetermineType(MathType leftType, MathType rightType, Context context);

    protected abstract MathExpression CreateInstance(MathExpression leftChild, MathExpression rightChild);

    public override MathExpression Clone() => CreateInstance(LeftChild, RightChild);

}

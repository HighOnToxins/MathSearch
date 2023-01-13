
using MathSearch.Expression;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions.Sets;

public sealed class InExpression: BinaryExpression {
    public InExpression(MathExpression leftChild, MathExpression rightChild) : base(leftChild, rightChild) {
    }

    public InExpression(MathExpression leftChild, MathType type) : base(leftChild, new TypeExpression(type)) {
    }

    protected override bool Condition(MathExpression leftChild, MathExpression rightChild, MathSystem context) =>
        rightChild.DetermineType(context) == MathType.Set;

    protected override bool TrySimplify(MathExpression leftChild, MathExpression rightChild, MathSystem context, out MathExpression? result) {

        if(rightChild is SetExpression setExpression) {

            //simplifies to a dijunction of equals
            IEnumerable<MathExpression> dijunctionChildren = setExpression.Children
                .Select(e => new EqualsExpression(leftChild.Clone(), e));

            result = new DisjunctionExpression(dijunctionChildren).Simplify(context);
            return true;

        }else if(rightChild is TypeExpression typeExpression) {

            MathType type = context.DetermineTypeOf(leftChild);

            //If all possible values of the expression (type) are in the type (typeExpression.Value)
            if(type.IsSubTypeOf(typeExpression.Value)) {
                result = new BooleanExpression(true);
                return true;
            }

            //If they have nothing in common then false
            if(!type.Overlaps(typeExpression.Value)) {
                result = new BooleanExpression(false);
                return true;
            }
        }

        result = null;
        return false;
    }

    protected override MathType ComputeType(MathExpression leftChild, MathExpression rightChild, MathSystem context) =>
        rightChild.DetermineType(context) == MathType.Set ? MathType.Boolean : MathType.Universe;

    protected override MathExpression CreateInstance(MathExpression leftChild, MathExpression rightChild) =>
        new InExpression(leftChild, rightChild);

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();

}

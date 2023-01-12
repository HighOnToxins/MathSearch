
using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions.Sets;

public sealed class InExpression: BinaryExpression {
    public InExpression(MathExpression leftChild, MathExpression rightChild) : base(leftChild, rightChild) {
    }

    protected override bool Condition(MathExpression leftChild, MathExpression rightChild, MathSystem context) =>
        rightChild.DetermineType(context) == MathType.Set;

    protected override bool TrySimplify(MathExpression simplifiedLeft, MathExpression simplifiedRight, MathSystem context, out MathExpression? result) {

        if(simplifiedRight is SetExpression setExpression) {
            result = new BooleanExpression(setExpression.Children.Contains(simplifiedLeft));
            return true;
        }

        //TODO: Write simplification for in-operator.

        //Check context for typeInfo or in equality with true or false

        // Add SetBuilder extractor.

        //if(simplifiedRightChild is SetExpression setExpression) {
        //    IEnumerable<MathExpression> children = setExpression.Children.Select(e => new EqualsExpression(LeftChild, e));
        //    result = new DisjunctionExpression(children);
        //}

        //simplify in-boolean-set 

        //check if context agrees

        //anything contained within empty type is false

        result = null;
        return false;
    }

    protected override MathType ComputeType(MathExpression leftChild, MathExpression rightChild, MathSystem context) =>
        rightChild.DetermineType(context) == MathType.Set ? MathType.Boolean : MathType.Universe;

    protected override MathExpression CreateInstance(MathExpression leftChild, MathExpression rightChild) =>
        new InExpression(leftChild, rightChild);

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();

}

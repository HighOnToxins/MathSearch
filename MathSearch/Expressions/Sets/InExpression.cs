
using MathSearch.Expression;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;

namespace MathSearch.Expressions.Sets;

public sealed class InExpression: BinaryExpression {
    public override int Precedence => 7;

    public InExpression(MathExpression leftChild, MathExpression rightChild) : base(leftChild, rightChild) {
    }

    public InExpression(MathExpression leftChild, MathType type) : base(leftChild, new TypeExpression(type)) {
    }

    protected override bool Condition(MathExpression leftChild, MathExpression rightChild, MathSystem context) =>
        MathType.Set.TryContains(rightChild, out bool result, context) && result;

    protected override bool TrySimplify(MathExpression leftChild, MathExpression rightChild, MathSystem context, out MathExpression? result) {

        if(rightChild is SetExpression setExpression) {

            //simplifies to a dijunction of equals
            IEnumerable<MathExpression> dijunctionChildren = setExpression.Operands
                .Select(e => new EqualsExpression(leftChild.Clone(), e));

            result = new DisjunctionExpression(dijunctionChildren).Simplify(context);
            return true;

        } else if(rightChild is TypeExpression typeExpression) {

            MathType type = leftChild.GetMathType(context);

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

    protected override MathType ComputeType(MathSystem context) =>
        RightOperand.GetMathType(context) == MathType.Set ? MathType.Boolean : MathType.Universe;

    public override IEnumerable<MathType> TypeOfOperands(MathType typeOfThis) {
        yield return MathType.Universe;

        if(typeOfThis.IsSubTypeOf(MathType.Boolean)) {
            yield return MathType.Set;
        } else {
            yield return MathType.Universe;
        }
    }

    protected override MathExpression CreateInstance(MathExpression leftChild, MathExpression rightChild) =>
        new InExpression(leftChild, rightChild);

    //TODO: add context stuff for in expression

}

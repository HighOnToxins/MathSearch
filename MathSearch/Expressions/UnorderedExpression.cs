
namespace MathSearch.Expressions;

public abstract class UnorderedExpression: OperatorExpression {

    public IReadOnlyList<MathExpression> Operands => operands;

    #region Constructors

    public UnorderedExpression(IEnumerable<MathExpression> operands) : base(operands) { }

    public UnorderedExpression(params MathExpression[] operands) : base(operands) { }

    #endregion

    public override MathExpression CleanUp() {
        List<MathExpression> result = operands.ToList();
        result.Sort();
        return CreateInstance(result);
    }

    #region Equality Methods

    public override bool Equals(MathExpression? other) {
        if(other == null || !other.GetType().IsEquivalentTo(GetType())) {
            return false;
        }

        List<MathExpression> operands = GetOperands().ToList();
        List<MathExpression> otherOperands = other.GetOperands().ToList();

        if(operands.Count != otherOperands.Count) {
            return false;
        }

        foreach(MathExpression operand in operands) {
            if(!otherOperands.Remove(operand)) {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() {
        int result = GetType().Name.GetHashCode();

        unchecked {
            foreach(MathExpression operand in GetOperands()) {
                result *= operand.GetHashCode();
            }
        }

        return result;
    }

    #endregion
}

namespace MathSearch.Expressions;

public abstract class BinaryExpression: OperatorExpression {

    private static T GetLeftOperand<T>(IEnumerable<T> operands) => operands.First();
    private static T GetRightOperand<T>(IEnumerable<T> operands) => operands.ElementAt(1);

    public MathExpression LeftOperand { get => GetLeftOperand(operands); }

    public MathExpression RightOperand { get => GetRightOperand(operands); }

    public override int OperandCount => 2;

    public BinaryExpression(MathExpression leftOperand, MathExpression rightOperand) : base(leftOperand, rightOperand) { }

    protected abstract bool Condition(MathExpression leftOperand, MathExpression rightOperand, MathSystem context);

    protected override bool ConditionIsMet(IEnumerable<MathExpression> operands, MathSystem context) =>
        Condition(GetLeftOperand(operands), GetRightOperand(operands), context);

    protected abstract bool TrySimplify(MathExpression leftOperand, MathExpression rightOperand, MathSystem context, out MathExpression? result);

    protected override bool TrySimplify(ref IEnumerable<MathExpression> operands, MathSystem context, out MathExpression? result) =>
        TrySimplify(GetLeftOperand(operands), GetRightOperand(operands), context, out result);

    protected abstract MathExpression CreateInstance(MathExpression leftOperand, MathExpression rightOperand);

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> operands) =>
        CreateInstance(GetLeftOperand(operands), GetRightOperand(operands));

}

using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;
using System.Collections;

namespace MathSearch;

public sealed class MathSystem : IEnumerable {

    public IEnumerator GetEnumerator() => expressions.GetEnumerator();

    private readonly HashSet<MathExpression> expressions;

    public bool IsEmpty => expressions.Count == 0;

    public bool IsInconsistent => expressions.Contains(new BooleanExpression(false));

    public MathSystem() {
        expressions = new HashSet<MathExpression>();
    }

    public MathSystem(IEnumerable<MathExpression> expressions) {
        this.expressions = new HashSet<MathExpression>(expressions);
    }

    public MathSystem CombineWith(MathSystem system) =>
        new(expressions.Concat(system.expressions));

    public void Add(MathExpression expression) {
        expressions.Add(expression);
    }

    internal MathExpression Simplify(MathExpression mathExpression) {
        throw new NotImplementedException();
    }

    internal MathType TypeOf(OperatorExpression operatorExpression) {
        throw new NotImplementedException();
    }

    public MathExpression Determine(MathExpression determinant) {
        throw new NotImplementedException();
    }

    public void Simplify() {
        throw new NotImplementedException();
    }

    public bool IsTrue(MathExpression expression) {
        MathExpression result = Determine(expression);
        return result is BooleanExpression booleanExpression && booleanExpression.Value;
    }

    public bool IsFalse(EqualsExpression expression) {
        MathExpression result = Determine(expression);
        return result is BooleanExpression booleanExpression && !booleanExpression.Value;
    }

    public MathType DetermineType(VariableExpression variableExpression) {
        throw new NotImplementedException();
    }
}
using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;
using System.Collections;
using System.Linq.Expressions;

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
        foreach(MathExpression expression in expressions) {
            expressions.Add(new InExpression(expression, MathType.Boolean).Simplify(GetContextFor(expression)));
        }

        foreach(MathExpression expression in expressions) {
            expressions.Add(expression.Simplify(GetContextFor(expression)));
        }

        //TODO: Extract conjunction
        //TODO: Combine equality.
    }

    private MathSystem GetContextFor(MathExpression expression) =>
        new(expressions.Except(new MathExpression[] { expression }));

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
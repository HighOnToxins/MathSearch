using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;
using System.Collections;

namespace MathSearch;

public sealed class MathSystem : IEnumerable {

    private HashSet<MathExpression> expressions;

    public IReadOnlySet<MathExpression> Expressions => expressions;

    public bool IsInconsistent { get => expressions.Contains(new BooleanExpression(false)); }

    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public MathSystem(params MathExpression[] expressions) {
        this.expressions = new HashSet<MathExpression>(expressions);
    }

    public MathSystem(IEnumerable<MathExpression> expressions) {
        this.expressions = new HashSet<MathExpression>(expressions);
    }

    public void Add(MathExpression expression) {
        expressions.Add(expression);
    }

    public void Add(IEnumerable<MathExpression> expressions) {
        foreach(MathExpression expression in expressions) {
            this.expressions.Add(expression);
        }
    }

    public void Simplify() {
        expressions = new(expressions.Select(e => e.Simplify()));
    }

    public MathExpression Simplify(MathExpression expression) {
        if(expressions.Contains(expression)) {
            return new BooleanExpression(true);
        }

        NotExpression? not = expressions.OfType<NotExpression>().FirstOrDefault(e => e.Child.Equals(expression));
        if(not != null) {
            return new BooleanExpression(false);
        }

        return expression.Clone();
    }

    public MathExpression Determine(MathExpression expression) {
        return Simplify(expression.Simplify(this));
    }

    internal MathType EvaluateTypeOf(MathExpression expression) {
        IEnumerable<MathType> types = expressions
            .OfType<InExpression>()
            .Where(i => i.LeftChild.Equals(expression) && i.RightChild is TypeExpression)
            .Select(i => i.RightChild is TypeExpression typeExpression ? typeExpression.Value : MathType.Universe);

        MathType type = MathType.Universe;
        foreach(MathType t in types) {
            type = type.Intersect(t);
        }

        return type;
    }

    IEnumerator IEnumerable.GetEnumerator() => expressions.GetEnumerator();
}

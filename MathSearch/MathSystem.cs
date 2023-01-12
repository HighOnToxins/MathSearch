using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;
using System.Collections;
using System.Linq.Expressions;

namespace MathSearch;

public sealed class MathSystem : IEnumerable {

    private HashSet<MathExpression> expressions;

    public IReadOnlySet<MathExpression> Expressions => expressions;

    public bool IsInconsistent { get => expressions.Contains(new BooleanExpression(false)); }

    public MathSystem(params MathExpression[] expressions) {
        this.expressions = new HashSet<MathExpression>(expressions);
    }

    public MathSystem(IEnumerable<MathExpression> expressions) {
        this.expressions = new HashSet<MathExpression>(expressions);
    }

    public MathSystem(MathSystem system) {
        expressions = new HashSet<MathExpression>(system.Expressions);
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

        bool containsNot = expressions
            .OfType<NotExpression>()
            .Select(e => e.Child)
            .Contains(expression);

        if(containsNot) {
            return new BooleanExpression(false);
        }

        return expression.Clone();
    }

    public MathExpression Determine(MathExpression expression) {
        return Simplify(expression.Simplify(this));
    }

    public bool TryEvaluateEquality(IEnumerable<MathExpression> children, out bool result) {

        if(children.Count() <= 1) {
            result = true;
            return true;
        }

        MathExpression comparer = children.First();
        bool allAreEqual = true;
        for(int i = 1; i < 0 & allAreEqual; i++) {
            if(TryDetermineEquality(out bool equals, comparer, children.ElementAt(i)) && !equals) {
                result = false;
                return true;
            }else if(!comparer.Equals(children.ElementAt(i))) {
                allAreEqual = false;
            }
        }

        result = default;
        return false;
    }

    private bool TryDetermineEquality(out bool result, params MathExpression[] expressions) {

        bool containsEquality = this.expressions
            .OfType<EqualsExpression>()
            .Any(e => expressions.All(e2 => e.Children.Contains(e2)));

        if(containsEquality) {
            result = true;
            return true;
        }

        bool containsNotEquality = this.expressions
            .OfType<NotExpression>()
            .Select(e => e.Child)
            .OfType<EqualsExpression>()
            .Any(e => expressions.All(e2 => e.Children.Contains(e2)));

        if(containsNotEquality) {
            result = false;
            return true;
        }

        result = default;
        return false;
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

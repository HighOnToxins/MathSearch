
using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;
using System.Linq;

namespace MathSearch;

public sealed class Context {

    private readonly HashSet<MathExpression> expressions;

    private readonly List<MathExpression> currentLayerContext;

    //TODO: Cache sub contexts.

    internal Context() {
        expressions = new();
        currentLayerContext = new();
    }

    private Context(IEnumerable<MathExpression> expressions) {
        this.expressions = new(expressions);
        currentLayerContext = new();
    }

    internal MathExpression Simplify(MathExpression expression) {
        if(expressions.Contains(expression)) {
            return new BooleanExpression(true);
        }

        return expression;
    }

    internal MathType DetermineType(MathExpression expression) {
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

    internal void AddContext(IEnumerable<MathExpression> expressions) {
        currentLayerContext.AddRange(expressions);
    }

    internal Context CreateSubContext(MathExpression e) {
        return new Context(currentLayerContext
            .Except(new MathExpression[] { e })
            .Concat(expressions));
    }
}

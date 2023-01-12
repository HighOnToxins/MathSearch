
using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;
using System.Linq;

namespace MathSearch;

public sealed class Context {

    private readonly HashSet<MathExpression> expressions;

    private readonly List<MathExpression> currentLayerContext;

    //TODO: Try to cache sub contexts.

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

        NotExpression? not = expressions.OfType<NotExpression>().FirstOrDefault(e => e.Child.Equals(expression));
        if(not != null) {
            return new BooleanExpression(false);
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
        currentLayerContext.AddRange(expressions); //TODO: Try to simplify to the children beeing added to the context
    }

    internal Context CreateSubContext(MathExpression e) {
        return new Context(currentLayerContext
            .Except(new MathExpression[] { e }) //TODO: FIX: Except does not work for Noted exceptions.
            .Concat(expressions));
    }
}

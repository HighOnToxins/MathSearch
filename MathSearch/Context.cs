
using MathSearch.Expression;
using MathSearch.Expressions;

namespace MathSearch;

//TODO: Structure the context such that it becomes easy to create sub-contexts.

public sealed class Context {

    private readonly Dictionary<MathExpression, MathExpression> replacements;

    private readonly Dictionary<MathExpression, MathType> typeInfo;

    internal Context() {
        replacements = new Dictionary<MathExpression, MathExpression>();
        typeInfo = new Dictionary<MathExpression, MathType>();
    }

    private Context(Dictionary<MathExpression, MathExpression> replacements,  Dictionary<MathExpression, MathType> typeInfo) {
        this.replacements = new(replacements);
        this.typeInfo = new();
    }

    internal MathExpression ReplaceEquality(MathExpression expression) {
        if(replacements.TryGetValue(expression, out MathExpression? replacement) && replacement != null) {
            return replacement;
        } else {
            return expression;
        }
    }

    internal MathType DetermineType(MathExpression expression) { 
        if(typeInfo.TryGetValue(expression, out MathType type)) {
            return type;
        } else {
            return MathType.Universe;
        }
    }

    internal void AddReplacement(MathExpression expression, MathExpression replacement) =>
        replacements.Add(expression, replacement);

    internal void AddType(MathExpression expression, MathType type) =>
        typeInfo.Add(expression, type);

    internal Context Clone() => new(replacements, typeInfo);

}

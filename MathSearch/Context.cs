
using MathSearch.Expression;
using MathSearch.Expressions;

namespace MathSearch;

public sealed class Context {

    private readonly Dictionary<MathExpression, MathExpression> replacements;

    private readonly Dictionary<MathExpression, ExpressionType> typeInfo;

    public Context() {
        replacements = new Dictionary<MathExpression, MathExpression>();
        typeInfo = new Dictionary<MathExpression, ExpressionType>();
    }

    private Context(Dictionary<MathExpression, MathExpression> replacements,  Dictionary<MathExpression, ExpressionType> typeInfo) {
        this.replacements = new(replacements);
        this.typeInfo = new();
    }


    public bool TryDetermineReplacement(in MathExpression expression, out MathExpression? replacement) =>
        replacements.TryGetValue(expression, out replacement);

    public bool TryDetermineType(in MathExpression expression, out ExpressionType type) =>
        typeInfo.TryGetValue(expression, out type);

    public ExpressionType DetermineType(MathExpression expression) { 
        if(TryDetermineType(expression, out ExpressionType type)) {
            return type;
        } else {
            return ExpressionType.Universe;
        }
    }

    public void AddReplacement(MathExpression expression, MathExpression replacement) =>
        replacements.Add(expression, replacement);

    public void AddType(MathExpression expression, ExpressionType type) =>
        typeInfo.Add(expression, type);

    public Context Clone() => new(replacements, typeInfo);
}

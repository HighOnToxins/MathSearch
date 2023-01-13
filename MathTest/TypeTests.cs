
using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions;

namespace MathTest; 

public sealed class TypeTests {

    [Test]
    public void TypeOfConjunctionIsBoolean() {

        MathExpression expression = new ConjunctionExpression(
            new DisjunctionExpression(
                new BooleanExpression(false),
                new BooleanExpression(true)
            ),
            new BooleanExpression(true)
        );

        MathType type = expression.DetermineType();

        Assert.That(type, Is.EqualTo(MathType.Boolean));
    }

    [Test]
    public void TypeOfVariableIsUniverse() {

        MathExpression expression = new ConjunctionExpression(
            new ConjunctionExpression(
                new VariableExpression("a"),
                new BooleanExpression(false)
            ),
            new BooleanExpression(true)
        );

        MathType type = expression.DetermineType();

        Assert.That(type, Is.EqualTo(MathType.Universe));
    }

}
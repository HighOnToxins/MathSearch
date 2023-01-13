
using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions;
using MathSearch.Expressions.Sets;
using MathSearch;

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

    [Test]
    public void EqualityDeterminesByType() {

        MathExpression expression1 = new SetExpression();
        MathExpression expression2 = new ConjunctionExpression();

        MathSystem system = new();

        if(system.TryEvaluateEquality(out bool result, expression1, expression2)) {
            Assert.That(result, Is.False);
        } else {
            Assert.Fail();
        }

    }

}
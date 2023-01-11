using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;

namespace MathTest;

public class Tests {

    [Test]
    public void TypeOfConjunctionIsBoolean() {

        MathExpression expression = new ConjunctionExpression(
            new InExpression(new VariableExpression("a"), new TypeExpression(MathType.Boolean)),
            new VariableExpression("a"),
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
    public void SimplificationOfConjunctionIsFalse() {

        MathExpression expression = new ConjunctionExpression(
            new InExpression(new VariableExpression("a"), new TypeExpression(MathType.Boolean)),
            new VariableExpression("a"),
            new BooleanExpression(false)
        );

        MathExpression result = expression.Simplify();

        Assert.That(result, Is.EqualTo(new BooleanExpression(false)));
    }


    [Test]
    public void CheckSimplfingConjunctionWorks() {

        MathExpression expression = new ConjunctionExpression(
            new InExpression(new VariableExpression("a"), new TypeExpression(MathType.Boolean)),
            new InExpression(new VariableExpression("b"), new TypeExpression(MathType.Boolean)),
            new InExpression(new VariableExpression("c"), new TypeExpression(MathType.Boolean)),
            new ConjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("b")
            ),
             new DisjunctionExpression(
                 new VariableExpression("a"),
                 new VariableExpression("c")
             )
        );

        MathExpression expected = new ConjunctionExpression(
            new InExpression(new VariableExpression("a"), new TypeExpression(MathType.Boolean)),
            new InExpression(new VariableExpression("b"), new TypeExpression(MathType.Boolean)),
            new InExpression(new VariableExpression("c"), new TypeExpression(MathType.Boolean)),
            new VariableExpression("a"),
            new VariableExpression("b")
        );

        MathExpression result = expression.Simplify();


        Assert.That(result, Is.EqualTo(expected));
    }
}
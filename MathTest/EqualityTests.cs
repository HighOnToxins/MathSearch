
using MathSearch;
using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;

namespace MathTest;

public sealed class EqualityTests {

    [Test]
    public void EqualityDoesNotCreateInfiniteLoop() {

        MathSystem system = new(){
            new EqualsExpression(new VariableExpression("a"), new VariableExpression("b")),
            new EqualsExpression(new VariableExpression("b"), new VariableExpression("c")),
        };

        system.Simplify();

        Assert.Pass();
    }

    [Test]
    public void VariablesAreEqualToThemselves() {
        MathExpression determinant = new EqualsExpression(new VariableExpression("a"), new VariableExpression("a"));
        MathExpression expected = new BooleanExpression(true);

        MathExpression result = determinant.Simplify();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void UndefinedVariablesAreUnsimplifiable() {
        MathExpression determinant = new EqualsExpression(new VariableExpression("a"), new VariableExpression("b"));
        MathExpression expected = new EqualsExpression(new VariableExpression("a"), new VariableExpression("b"));

        MathExpression result = determinant.Simplify();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void EqualitySimplifies() {

        MathSystem system = new(){
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
            new InExpression(new VariableExpression("c"), MathType.Boolean),
        };

        MathExpression determinant = new ConjunctionExpression(
            new EqualsExpression(new VariableExpression("a"), new VariableExpression("b")),
            new EqualsExpression(new VariableExpression("c"), new VariableExpression("b")),
            new VariableExpression("a"),
            new VariableExpression("b"),
            new VariableExpression("c")
        );

        MathExpression expected = new ConjunctionExpression(
            new VariableExpression("a"),
            new VariableExpression("b"),
            new VariableExpression("c")
        );

        MathExpression result = system.Determine(determinant);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void SimplifiedExpressionReplacesNonsimplified() {

        MathSystem system = new(){
            new EqualsExpression(new VariableExpression("a"), new BooleanExpression(false))
        };

        MathExpression determinant = new VariableExpression("a");
        MathExpression expected = new BooleanExpression(false);

        MathExpression result = system.Determine(determinant);

        Assert.That(result, Is.EqualTo(expected));
    }

    //[Test]
    //public void AndedEqualitiesAreGrouped() {

    //    MathExpression expression = new ConjunctionExpression(
    //            new EqualsExpression(new VariableExpression("a"), new VariableExpression("b")),
    //            new EqualsExpression(new VariableExpression("a"), new VariableExpression("c")),
    //            new EqualsExpression(new VariableExpression("a"), new VariableExpression("d"))
    //    );

    //    MathExpression expected = new EqualsExpression(
    //        new VariableExpression("a"), 
    //        new VariableExpression("b"),
    //        new VariableExpression("c"),
    //        new VariableExpression("d")
    //    );

    //    MathExpression result = expression.Simplify();

    //    Assert.That(result, Is.EqualTo(expected));

    //}

}


using MathSearch;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions;
using MathSearch.Expression;
using MathSearch.Expressions.Sets;
using MathSearch.Expressions.Basics;
using System.Linq;

namespace MathTest;

public sealed class SystemTests {

    [Test]
    public void SystemUsesSimplestEquation() {

        MathSystem system = new() {
            new EqualsExpression(
                new ConjunctionExpression(
                    new VariableExpression("a"),
                    new VariableExpression("b")
                ),
                new BooleanExpression(true)
            ),
            new EqualsExpression(
                new ConjunctionExpression(
                    new VariableExpression("a"),
                    new NotExpression(new VariableExpression("b"))
                ),
                new BooleanExpression(false)
            ),
        };

        HashSet<MathExpression> expected = new() {
            new EqualsExpression(
                new ConjunctionExpression(
                    new VariableExpression("a"),
                    new VariableExpression("b")
                ),
                new BooleanExpression(true)
            ),
        };

        system.Simplify();

        Assert.That(system.Expressions, Is.EqualTo(expected));

    }

    [Test]
    public void SystemExtractsConjunction() {

        MathSystem system = new() {
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
            new VariableExpression("c"),
            new ConjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("b")
            )
        };

        MathExpression expect1 = new VariableExpression("c");
        MathExpression expect2 = new VariableExpression("a");
        MathExpression expect3 = new VariableExpression("b");

        system.Simplify();

        Assert.That(system.Expressions, Contains.Item(expect1));
        Assert.That(system.Expressions, Contains.Item(expect2));
        Assert.That(system.Expressions, Contains.Item(expect3));

    }

    [Test]
    public void SystemRemovesTrue() {

        MathSystem system = new() {
            new BooleanExpression(true)
        };

        HashSet<MathExpression> expect = new() {
        };

        system.Simplify();

        Assert.That(system.Expressions, Is.EqualTo(expect));

    }

    [Test]
    public void SystemRemovesEmptyExpression() {

        MathSystem system = new() {
            new EmptyExpression()
        };

        HashSet<MathExpression> expect = new() {
        };

        system.Simplify();

        Assert.That(system.Expressions, Is.EqualTo(expect));

    }

    [Test]
    public void CheckInconsistencyWorks() {

        MathSystem system = new() {
            new VariableExpression("a"),
            new NotExpression(new VariableExpression("a"))
        };

        bool expect = true;

        system.Simplify();

        Assert.That(system.IsInconsistent, Is.EqualTo(expect));

    }

    [Test]
    public void ADoesNotDisapear() {

        MathSystem system = new() {
            new VariableExpression("a"),
        };

        MathExpression expect = new VariableExpression("a");

        system.Simplify();

        Assert.That(system.Expressions.First(), Is.EqualTo(expect));

    }
}

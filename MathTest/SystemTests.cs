
using MathSearch;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions;
using MathSearch.Expression;
using MathSearch.Expressions.Sets;
using MathSearch.Expressions.Basics;

namespace MathTest;

public sealed class SystemTests {

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

        HashSet<MathExpression> expect = new() {
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
            new VariableExpression("c"),
            new VariableExpression("a"),
            new VariableExpression("b")
        };

        system.Simplify();

        Assert.That(system.Expressions, Is.EqualTo(expect));

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

}
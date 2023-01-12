
using MathSearch;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions;
using MathSearch.Expression;
using MathSearch.Expressions.Sets;

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

}
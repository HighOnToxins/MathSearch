﻿
using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

//TODO: Add independent/simplified expression.

[Precedence(0)]
public sealed class BooleanExpression: AtomExpression<bool> {
    public override MathType Type => MathType.Boolean;

    public BooleanExpression(bool value) : base(value) {
    }

    public override MathExpression Clone() =>
        new BooleanExpression(Value);
}

﻿using MathSearch.Expression;
using MathSearch.Expressions.Basics;

namespace MathSearch.Expressions; 

public abstract class UnaryExpression : OperatorExpression {

    private static T GetChild<T>(IEnumerable<T> children) => children.First();
    private static void SetChild<T>(T[] children, T expression) { children[0] = expression; }

    public override IReadOnlyList<MathExpression> Children { get; }

    public MathExpression Child { get => GetChild(Children); }

    public override int ChildCount => 1;

    public UnaryExpression(MathExpression child) {
        MathExpression[] children = new MathExpression[ChildCount];
        Children = children;
        SetChild(children, child);
    }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, Context context) =>
        Condition(GetChild(children), context);

    protected abstract bool Condition(MathExpression child, Context context);

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected override bool TrySimplify(IEnumerable<MathExpression> children, Context context, out MathExpression? result) =>
        TrySimplify(GetChild(children), context, out result);

    protected abstract bool TrySimplify(MathExpression child, Context context, out MathExpression? result);

    protected override MathType ComputeType(IEnumerable<MathExpression> childTypes, Context context) =>
        ComputeType(GetChild(childTypes), context);

    protected abstract MathType ComputeType(MathExpression child, Context context);

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) =>
        CreateInstance(GetChild(children));

    protected abstract MathExpression CreateInstance(MathExpression child);

}
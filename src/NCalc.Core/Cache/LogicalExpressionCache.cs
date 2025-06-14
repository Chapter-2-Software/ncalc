﻿using NCalc.Domain;

namespace NCalc.Cache;

public sealed class LogicalExpressionCache() : ILogicalExpressionCache
{
    private readonly ConcurrentDictionary<string, WeakReference<LogicalExpression>> _compiledExpressions = new();

    private static readonly LogicalExpressionCache Instance;

    static LogicalExpressionCache()
    {
        Instance = new LogicalExpressionCache();
    }

    public static LogicalExpressionCache GetInstance() => Instance;

    public bool TryGetValue(string expression, out LogicalExpression? logicalExpression)
    {
        logicalExpression = null;

        if (!_compiledExpressions.TryGetValue(expression, out var wr))
            return false;
        if (!wr.TryGetTarget(out logicalExpression))
            return false;

        return true;
    }

    public void Set(string expression, LogicalExpression logicalExpression)
    {
        _compiledExpressions[expression] = new WeakReference<LogicalExpression>(logicalExpression);
        ClearCache();
    }

    private void ClearCache()
    {
        foreach (var kvp in _compiledExpressions)
        {
            if (kvp.Value.TryGetTarget(out _))
                continue;

            _compiledExpressions.TryRemove(kvp.Key, out _);
        }
    }
}

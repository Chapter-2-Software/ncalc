using NCalc.Domain;
using NCalc.Exceptions;
using NCalc.Logging;
using NCalc.Parser;

namespace NCalc.Factories;

/// <summary>
/// Class responsible to create <see cref="LogicalExpression"/> objects. Parlot is used for parsing strings.
/// </summary>
public sealed class LogicalExpressionFactory() : ILogicalExpressionFactory
{
    private static readonly LogicalExpressionFactory Instance;

    static LogicalExpressionFactory()
    {
        Instance = new LogicalExpressionFactory();
    }

    public static LogicalExpressionFactory GetInstance() => Instance;

    LogicalExpression ILogicalExpressionFactory.Create(string expression, ExpressionOptions options)
    {
        try
        {
            return Create(expression, options);
        }
        catch (Exception exception)
        {
            throw new NCalcParserException("Error parsing the expression.", exception);
        }
    }

    public static LogicalExpression Create(string expression, ExpressionOptions options = ExpressionOptions.None)
    {
        var parserContext = new LogicalExpressionParserContext(expression, options);
        return LogicalExpressionParser.Parse(parserContext);
    }
}
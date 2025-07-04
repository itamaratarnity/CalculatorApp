namespace CalculatorApp;

public enum CalculatorAction
{
    None,
    Add,
    Subtract,
    Multiply,
    Divide
}

public class CalculatorResponse
{
    public string Result { get; }
    public bool Success { get; } 
    public string Error { get; } 

    public CalculatorResponse(string Result, bool Success = true, string Error = "")
    {
        this.Result = Result;
        this.Success = Success;
        this.Error = Error;
    }
}

public class Calculator
{
    private CalculatorAction _action = CalculatorAction.None;
    private int _argument1 = -1;
    private int _argument2 = -1;
    private bool _completed = false;

    private readonly List<string> _history = new();

    public IReadOnlyList<string> History => _history.AsReadOnly();

    public void SetArgument1(string value)
    {
        int.TryParse(value, out _argument1);
    }

    public void SetArgument2(string value)
    {
        int.TryParse(value, out _argument2);
    }

    public void SetAction(CalculatorAction action) => _action = action;

    public CalculatorResponse IsValid()
    {
        if (_action == CalculatorAction.None)
            return new CalculatorResponse(string.Empty, false, "No action set.");
        if (_argument1 == -1)
            return new CalculatorResponse(string.Empty, false, "First argument is not set.");
        if (_argument2 == -1 && _action != CalculatorAction.Add && _action != CalculatorAction.Subtract)
            return new CalculatorResponse(string.Empty, false, "Second argument is not set.");
        _completed = true;
        return new CalculatorResponse(string.Empty, true, string.Empty);
    }

    public bool IsCompleted() => _completed;

    public void Reset()
    {
        _completed = false;
        _action = CalculatorAction.None;
        _argument1 = -1;
        _argument2 = -1;
    }

    public CalculatorResponse Perform()
    {
        try
        {
            CalculatorResponse response = IsValid();
            if (!response.Success)
                return response;

            int result = _action switch
            {
                CalculatorAction.Add => _argument1 + _argument2,
                CalculatorAction.Subtract => _argument1 - _argument2,
                CalculatorAction.Multiply => _argument1 * _argument2,
                CalculatorAction.Divide => _argument2 == 0
                    ? throw new DivideByZeroException()
                    : _argument1 / _argument2,
                _ => _argument1
            };

            string historyItem = $"{_argument1} {_actionToSymbol(_action)} {_argument2} = {result}";
            _history.Add(historyItem);

            return new CalculatorResponse(result.ToString(), true, string.Empty);
        }
        catch (DivideByZeroException)
        {
            return new CalculatorResponse(string.Empty, false, "Division by zero is not allowed");
        }
        catch (Exception)
        {
            return new CalculatorResponse(string.Empty, false, "Unknown error");
        }
    }

    private string _actionToSymbol(CalculatorAction action) => action switch
    {
        CalculatorAction.Add => "+",
        CalculatorAction.Subtract => "-",
        CalculatorAction.Multiply => "*",
        CalculatorAction.Divide => "/",
        _ => "?"
    };

    public void ClearHistory()
    {
        _history.Clear();
    }
}


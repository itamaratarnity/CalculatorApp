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
    public int Result { get; } = 0;
    public bool Success { get; } = true;
    public string Error { get; } = string.Empty;

    public CalculatorResponse(int Result, bool Success = true, string Error = "")
    {
        this.Result = Result;
        this.Success = Success;
        this.Error = Error;
    }
}

public class Calculator
{
    private CalculatorAction _action = CalculatorAction.None;
    private int _argument1 = 0;
    private int _argument2 = 0;
    private bool _completed = false;

    public void SetArgumentOne(int value) => _argument1 = value;
    public void SetArgumentTwo(int value) => _argument2 = value;
    public void SetAction(CalculatorAction action) => _action = action;


    public bool IsCompleted() => _completed;

    public void Reset()
    {
        _completed = false;
        _action = CalculatorAction.None;
        _argument1 = 0;
        _argument2 = 0;
    }

    public CalculatorResponse Perform()
    {
        if (_action == CalculatorAction.None)
            return new CalculatorResponse(-1, false, "No action set.");
        else if (_argument1 == 0)
            return new CalculatorResponse(-1, false, "First argument is not set.");
        else if (_argument2 == 0)
            return new CalculatorResponse(-1, false, "Second argument is not set.");

        int result = _action switch
        {
            CalculatorAction.Add => _argument1 + _argument2,
            CalculatorAction.Subtract => _argument1 - _argument2,
            CalculatorAction.Multiply => _argument1 * _argument2,
            CalculatorAction.Divide => _argument2 != 0 ? _argument1 / _argument2 : 0,
            _ => _argument1
        };

        return new CalculatorResponse(result, true, string.Empty);
    }
}

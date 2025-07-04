using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace CalculatorApp
{
    public class CalculatorController
    {
        private Calculator _calculator;

        public CalculatorController()
        {
            _calculator = new Calculator();
        }

        public void OnButtonClicked(string buttonText, Label resultLabel, Label messageLabel, Label historyLabel)
        {
            try
            {
                if (int.TryParse(buttonText, out int number))
                {
                    resultLabel.Text += buttonText;
                }
                else if (buttonText == "+" || buttonText == "-" || buttonText == "*" || buttonText == "/")
                {
                    _calculator.SetArgument1(resultLabel.Text);
                    CalculatorAction action = buttonText switch
                    {
                        "+" => CalculatorAction.Add,
                        "-" => CalculatorAction.Subtract,
                        "*" => CalculatorAction.Multiply,
                        "/" => CalculatorAction.Divide,
                        _ => CalculatorAction.None
                    };
                    _calculator.SetAction(action);
                    resultLabel.Text = "";
                }
                else if (buttonText == "C")
                {
                    _calculator.Reset();
                    resultLabel.Text = "";
                    messageLabel.Text = "";
                }
                else if (buttonText == "=")
                {
                    _calculator.SetArgument2(resultLabel.Text);
                    var response = _calculator.IsValid();
                    if (!response.Success)
                    {
                        messageLabel.Text = response.Error;
                        return;
                    }

                    response = _calculator.Perform();
                    if (!response.Success)
                    {
                        messageLabel.Text = response.Error;
                        return;
                    }

                    resultLabel.Text = response.Result.ToString();
                    messageLabel.Text = string.Empty;
                    _calculator.SetAction(CalculatorAction.None);
                    historyLabel.Text = string.Join(Environment.NewLine, _calculator.History.Reverse().Take(3));
                }
            }
            catch (Exception)
            {
                messageLabel.Text = "שגיאה כללית";
            }
        }

        public void ClearHistory(Label historyLabel)
        {
            _calculator.ClearHistory();
            historyLabel.Text = string.Empty;
        }
    }
}

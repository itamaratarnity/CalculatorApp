
namespace CalculatorApp
{
    public partial class CalculatorPage : ContentPage
    {
        #region Memebers
        private Calculator _calculator = new Calculator();
        #endregion

        public CalculatorPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void GridButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button btn)
                {

                    string value = btn.Text;

                    if (int.TryParse(value, out int number)) // Number button
                        ResultLabel.Text += value;
                    else if (value == "+" || value == "-" || value == "*" || value == "/")
                    {
                        _calculator.SetArgument1(ResultLabel.Text);
                        CalculatorAction action = value switch
                        {
                            "+" => CalculatorAction.Add,
                            "-" => CalculatorAction.Subtract,
                            "*" => CalculatorAction.Multiply,
                            "/" => CalculatorAction.Divide,
                            _ => CalculatorAction.None
                        };
                        _calculator.SetAction(action);
                        ResultLabel.Text = "";

                    }
                    else if (value == "C") // Optional: Clear button
                    {
                        _calculator.Reset();
                        ResultLabel.Text = "";
                        MessageLabel.Text = "";
                    }
                    else if (value == "=")
                    {
                        // set second agrument
                        _calculator.SetArgument2(ResultLabel.Text);
                        var response = _calculator.IsValid();
                        if (!response.Success)
                        {
                            MessageLabel.Text = response.Error;
                            return;
                        }
                        response = _calculator.Perform();
                        if (!response.Success)
                        {
                            MessageLabel.Text = response.Error;
                            return;
                        }

                        ResultLabel.Text = response.Result.ToString();
                        HistoryLabel.Text = string.Join(Environment.NewLine, _calculator.History.Reverse().Take(3));
                        MessageLabel.Text = string.Empty;
                        _calculator.SetAction(CalculatorAction.None);
                    }
                }

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}

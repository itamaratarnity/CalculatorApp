namespace CalculatorApp
{
    public partial class CalculatorPage : ContentPage
    {
        #region Memebers
        private Calculator _calculator = new Calculator();
        private string _inputBuffer = "";
        private bool _isSecondArgument = false;
        #endregion

        public CalculatorPage()
        {
            InitializeComponent();
       }

        private void GridButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string value = btn.Text;

                if (int.TryParse(value, out int number)) // Number button
                {
                    _inputBuffer += value;
                    ResultLabel.Text = _inputBuffer;
                    MessageLabel.Text = ""; // Clear message on number input
                }
                else if (value == "+" || value == "-" || value == "*" || value == "/")
                {
                    if (int.TryParse(_inputBuffer, out int arg1))
                    {
                        _calculator.SetArgumentOne(arg1);
                        _inputBuffer = "";
                        _isSecondArgument = true;

                        CalculatorAction action = value switch
                        {
                            "+" => CalculatorAction.Add,
                            "-" => CalculatorAction.Subtract,
                            "*" => CalculatorAction.Multiply,
                            "/" => CalculatorAction.Divide,
                            _ => CalculatorAction.None
                        };
                        _calculator.SetAction(action);
                        MessageLabel.Text = ""; // Clear message on action set
                    }
                }
                else if (value == "=")
                {
                    if (_isSecondArgument && int.TryParse(_inputBuffer, out int arg2))
                    {
                        _calculator.SetArgumentTwo(arg2);
                        var response = _calculator.Perform();
                        if (response.Success)
                        {
                            ResultLabel.Text = response.Result.ToString();
                            MessageLabel.Text = ""; // Clear message on success
                            _calculator.SetArgumentOne(response.Result); // For chaining
                            _inputBuffer = "";
                            _isSecondArgument = false;
                        }
                        else
                        {
                            ResultLabel.Text = "0";
                            MessageLabel.Text = response.Error; // Show error
                            _inputBuffer = "";
                            _isSecondArgument = false;
                        }
                    }
                }
                else if (value == "C") // Optional: Clear button
                {
                    _calculator.Reset();
                    _inputBuffer = "";
                    _isSecondArgument = false;
                    ResultLabel.Text = "0";
                    MessageLabel.Text = "";
                }
            }
        }
    }
}

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace CalculatorApp;

public class CalculatorCodeOnlyPage : ContentPage
{
    private Label ResultLabel;
    private Label MessageLabel;
    private Calculator _calculator = new Calculator();

    public CalculatorCodeOnlyPage()
    {
        Title = "Calculator (Code Only)";
        BackgroundColor = Colors.White;

        ResultLabel = new Label
        {
            Text = "",
            FontSize = 48,
            TextColor = Colors.DodgerBlue,
            HorizontalOptions = LayoutOptions.Fill, // Updated to remove obsolete FillAndExpand  
            HorizontalTextAlignment = TextAlignment.End,
            VerticalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(10)
        };

        var resultBorder = new Border
        {
            Stroke = Colors.DodgerBlue,
            StrokeThickness = 2,
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            Content = ResultLabel,
            Margin = new Thickness(10),
            HeightRequest = 100
        };

        var buttonGrid = CreateButtonGrid();

        MessageLabel = new Label
        {
            Text = "",
            FontSize = 24,
            TextColor = Colors.Red,
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(10)
        };

        Content = new VerticalStackLayout
        {
            Padding = new Thickness(20),
            Spacing = 20,
            Children =
           {
               resultBorder,
               buttonGrid,
               MessageLabel
           }
        };
    }

    private Grid CreateButtonGrid()
    {
        var grid = new Grid
        {
            RowSpacing = 10,
            ColumnSpacing = 10
        };

        for (int i = 0; i < 4; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        string[,] buttons = new string[,]
        {
           { "1", "2", "3", "+" },
           { "4", "5", "6", "-" },
           { "7", "8", "9", "*" },
           { "C", "0", "/", "=" }
        };

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                var label = buttons[row, col];
                var button = new Button
                {
                    Text = label,
                    FontSize = 22,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White,
                    Background = CreateGradientBrush(label),
                    CornerRadius = 16,
                    HeightRequest = 56
                };

                button.Clicked += GridButton_Clicked;

                grid.Add(button, col, row);
            }
        }

        return grid;
    }

    private void GridButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button btn)
            {
                string value = btn.Text;

                if (int.TryParse(value, out int number))
                {
                    ResultLabel.Text += value;
                }
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
                else if (value == "C")
                {
                    _calculator.Reset();
                    ResultLabel.Text = "";
                    MessageLabel.Text = "";
                }
                else if (value == "=")
                {
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
                    MessageLabel.Text = string.Empty;
                    _calculator.SetAction(CalculatorAction.None);
                }
            }
        }
        catch (Exception ex)
        {
            MessageLabel.Text = "שגיאה כללית";
        }
    }

    private Brush CreateGradientBrush(string label)
    {
        bool isOperator = "+-*/=C".Contains(label);
        return new LinearGradientBrush(
            new GradientStopCollection
            {
               new GradientStop { Color = isOperator ? Colors.Orange : Colors.Teal, Offset = 0 },
               new GradientStop { Color = isOperator ? Colors.Yellow : Colors.Blue, Offset = 1 }
            },
            new Point(0, 0),
            new Point(1, 1)
        );
    }
}

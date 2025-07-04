using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace CalculatorApp;

public class CalculatorCodeOnlyPage : ContentPage
{
    private Label ResultLabel;
    private Label MessageLabel;
    private Label HistoryLabel;

    private Button ClearHistoryButton;

    private CalculatorController _controller;


    public CalculatorCodeOnlyPage()
    {
        _controller = new CalculatorController();

        Title = "Calculator (Code Only)";
        BackgroundColor = Colors.White;


        // History Label בתוך Border
        HistoryLabel = new Label
        {
            Text = "",
            FontFamily = "Roboto",
            TextColor = Colors.Gray,
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Center,
            LineBreakMode = LineBreakMode.WordWrap,
            MaxLines = 3,
            FontSize = 16,
            Padding = new Thickness(10, 0)
        };

        var historyBorder = new Border
        {
            Stroke = Colors.LightGray,
            StrokeThickness = 1,
            BackgroundColor = Color.FromArgb("#EEE"),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            Padding = 10,
            Content = HistoryLabel,
            VerticalOptions = LayoutOptions.Center
        };

        // כפתור ניקוי היסטוריה, ממורכז עם מרווח עליון קטן
        ClearHistoryButton = new Button
        {
            Text = "נקה היסטוריה",
            FontSize = 14,
            Padding = new Thickness(10, 5),
            Margin = new Thickness(0, 5, 0, 0),
            HorizontalOptions = LayoutOptions.Center
        };
        ClearHistoryButton.Clicked +=  (s,e) => { _controller.ClearHistory(HistoryLabel); } ;

        // היסטוריה עם כפתור בתוך Grid של 2 שורות
        var historyGrid = new Grid
        {
            Margin = new Thickness(0, 10, 0, 0),
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            }
        };
        historyGrid.Add(historyBorder, 0, 0);
        historyGrid.Add(ClearHistoryButton, 0, 1);


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
               historyGrid,
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

                button.Clicked += (s, e) =>
                {
                    if (s is Button btn)
                    {
                        _controller.OnButtonClicked(btn.Text, ResultLabel, MessageLabel, HistoryLabel);
                    }
                };

                grid.Add(button, col, row);
            }
        }

        return grid;
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

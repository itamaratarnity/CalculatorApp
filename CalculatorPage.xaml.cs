
namespace CalculatorApp
{
    public partial class CalculatorPage : ContentPage
    {
        #region Members
        private CalculatorController _controller;
        #endregion

        public CalculatorPage()
        {
            try
            {
                InitializeComponent();
                _controller = new CalculatorController();
            }
            catch (Exception ex)
            {
                DisplayAlert("שגיאה", $"אירעה שגיאה: {ex.Message}", "אישור");
            }
        }

        private async void GridButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button btn)
                {
                    _controller.OnButtonClicked(btn.Text, ResultLabel, MessageLabel, HistoryLabel);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("שגיאה", $"אירעה שגיאה: {ex.Message}", "אישור");
            }
        }

        private void ClearHistoryButton_Clicked(object sender, EventArgs e)
        {
            _controller.ClearHistory(HistoryLabel);
        }
    }
}

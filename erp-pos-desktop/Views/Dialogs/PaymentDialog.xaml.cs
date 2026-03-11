using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TatweerPOS.Views.Dialogs
{
    /// <summary>
    /// Modal dialog for completing a payment transaction.
    /// </summary>
    public partial class PaymentDialog : Window
    {
        private decimal _orderTotal;
        private decimal _amountTendered;
        private string _paymentMethod = "Cash";

        /// <summary>
        /// Gets or sets the order total amount.
        /// </summary>
        public decimal OrderTotal
        {
            get => _orderTotal;
            set
            {
                _orderTotal = value;
                OrderTotalText.Text = value.ToString("C2", CultureInfo.InvariantCulture);
                UpdateValidation();
            }
        }

        /// <summary>
        /// Gets the selected payment method (Cash, Card, Mobile, Split).
        /// </summary>
        public string PaymentMethod => _paymentMethod;

        /// <summary>
        /// Gets the amount tendered by the customer.
        /// </summary>
        public decimal AmountTendered => _amountTendered;

        /// <summary>
        /// Gets the computed change amount (AmountTendered - OrderTotal), only relevant for Cash.
        /// </summary>
        public decimal ChangeAmount => _paymentMethod == "Cash"
            ? Math.Max(0, _amountTendered - _orderTotal)
            : 0m;

        /// <summary>
        /// Gets whether the current payment state is valid for completion.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (_paymentMethod == "Cash")
                    return _amountTendered >= _orderTotal && _orderTotal > 0;
                return _orderTotal > 0;
            }
        }

        public PaymentDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlayEntranceAnimation();
        }

        private void PlayEntranceAnimation()
        {
            var duration = TimeSpan.FromMilliseconds(250);
            var easing = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };

            var scaleXAnim = new DoubleAnimation(0.85, 1.0, new Duration(duration)) { EasingFunction = easing };
            var scaleYAnim = new DoubleAnimation(0.85, 1.0, new Duration(duration)) { EasingFunction = easing };
            var opacityAnim = new DoubleAnimation(0, 1, new Duration(duration)) { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };

            var transform = (TransformGroup)DialogCard.RenderTransform;
            var scaleTransform = (ScaleTransform)transform.Children[0];

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnim);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnim);
            DialogCard.BeginAnimation(OpacityProperty, opacityAnim);
        }

        private void PaymentMethod_Changed(object sender, RoutedEventArgs e)
        {
            if (CashRadio == null) return;

            if (CashRadio.IsChecked == true)
                _paymentMethod = "Cash";
            else if (CardRadio.IsChecked == true)
                _paymentMethod = "Card";
            else if (MobileRadio.IsChecked == true)
                _paymentMethod = "Mobile";
            else if (SplitRadio.IsChecked == true)
                _paymentMethod = "Split";

            bool isCash = _paymentMethod == "Cash";
            if (CashSection != null)
                CashSection.Visibility = isCash ? Visibility.Visible : Visibility.Collapsed;
            if (ChangeSection != null)
                ChangeSection.Visibility = isCash ? Visibility.Visible : Visibility.Collapsed;

            UpdateValidation();
        }

        private void AmountTendered_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (decimal.TryParse(AmountTenderedInput.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                _amountTendered = amount;
            else
                _amountTendered = 0;

            UpdateChangeDisplay();
            UpdateValidation();
        }

        private void QuickAmount_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                string tag = btn.Tag?.ToString() ?? "";
                if (tag == "exact")
                {
                    _amountTendered = _orderTotal;
                    AmountTenderedInput.Text = _orderTotal.ToString("F2", CultureInfo.InvariantCulture);
                }
                else if (decimal.TryParse(tag, out var quickAmount))
                {
                    _amountTendered = quickAmount;
                    AmountTenderedInput.Text = quickAmount.ToString("F2", CultureInfo.InvariantCulture);
                }

                UpdateChangeDisplay();
                UpdateValidation();
            }
        }

        private void UpdateChangeDisplay()
        {
            if (ChangeAmountText != null)
                ChangeAmountText.Text = ChangeAmount.ToString("C2", CultureInfo.InvariantCulture);
        }

        private void UpdateValidation()
        {
            if (CompleteButton != null)
                CompleteButton.IsEnabled = IsValid;
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

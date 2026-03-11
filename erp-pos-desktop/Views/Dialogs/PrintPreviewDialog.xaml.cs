using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TatweerPOS.Views.Dialogs
{
    /// <summary>
    /// Receipt preview dialog for dev mode printing.
    /// Displays a formatted receipt on a simulated paper strip.
    /// </summary>
    public partial class PrintPreviewDialog : Window
    {
        private string _receiptText = string.Empty;
        private string _orderId = string.Empty;

        /// <summary>
        /// Gets or sets the formatted receipt text to display.
        /// </summary>
        public string ReceiptText
        {
            get => _receiptText;
            set
            {
                _receiptText = value;
                if (ReceiptContent != null)
                    ReceiptContent.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the order ID for this receipt.
        /// </summary>
        public string OrderId
        {
            get => _orderId;
            set => _orderId = value;
        }

        public PrintPreviewDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_receiptText))
                ReceiptContent.Text = _receiptText;

            PlayEntranceAnimation();
        }

        private void PlayEntranceAnimation()
        {
            var duration = TimeSpan.FromMilliseconds(250);
            var easing = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };

            var scaleXAnim = new DoubleAnimation(0.85, 1.0, new Duration(duration)) { EasingFunction = easing };
            var scaleYAnim = new DoubleAnimation(0.85, 1.0, new Duration(duration)) { EasingFunction = easing };
            var opacityAnim = new DoubleAnimation(0, 1, new Duration(duration))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var transform = (TransformGroup)DialogCard.RenderTransform;
            var scaleTransform = (ScaleTransform)transform.Children[0];

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnim);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnim);
            DialogCard.BeginAnimation(OpacityProperty, opacityAnim);
        }

        private void ShowToast(string message)
        {
            ToastText.Text = message;

            var fadeIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(200)));
            var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(300)))
            {
                BeginTime = TimeSpan.FromSeconds(2)
            };

            var storyboard = new Storyboard();
            Storyboard.SetTarget(fadeIn, ToastBorder);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));
            Storyboard.SetTarget(fadeOut, ToastBorder);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(OpacityProperty));

            storyboard.Children.Add(fadeIn);
            storyboard.Children.Add(fadeOut);
            storyboard.Begin();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            // Simulate print success in dev mode
            string message = TryFindResource("str_PrintSuccess") as string ?? "Receipt printed successfully";
            ShowToast(message);
        }

        private void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            // Simulate PDF export in dev mode
            string message = TryFindResource("str_ExportPDFSuccess") as string ?? "PDF exported successfully";
            ShowToast(message);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

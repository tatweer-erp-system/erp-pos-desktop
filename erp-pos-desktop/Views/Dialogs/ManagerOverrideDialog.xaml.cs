using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TatweerPOS.Views.Dialogs
{
    /// <summary>
    /// Manager PIN override dialog for restricted actions (e.g., discount > 20%, void).
    /// Validates against mock PIN "0000". Locks after 3 failed attempts.
    /// </summary>
    public partial class ManagerOverrideDialog : Window
    {
        private const string MockManagerPin = "0000";
        private const int MaxFailAttempts = 3;

        private string _actionDescription = string.Empty;
        private string _errorMessage = string.Empty;
        private int _failCount;
        private bool _isLocked;

        /// <summary>
        /// Gets or sets the description of the action requiring manager override.
        /// </summary>
        public string ActionDescription
        {
            get => _actionDescription;
            set
            {
                _actionDescription = value;
                if (ActionDescriptionText != null)
                    ActionDescriptionText.Text = value;
            }
        }

        /// <summary>
        /// Gets the current error message, if any.
        /// </summary>
        public string ErrorMessage => _errorMessage;

        /// <summary>
        /// Gets the number of failed PIN attempts.
        /// </summary>
        public int FailCount => _failCount;

        public ManagerOverrideDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_actionDescription))
                ActionDescriptionText.Text = _actionDescription;

            PlayEntranceAnimation();
            Pin1.Focus();
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

        private void PinDigit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only allow digits
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void PinDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isLocked) return;

            if (sender is TextBox currentBox && currentBox.Text.Length == 1)
            {
                // Auto-focus next PIN box
                if (currentBox == Pin1) Pin2.Focus();
                else if (currentBox == Pin2) Pin3.Focus();
                else if (currentBox == Pin3) Pin4.Focus();
            }

            // Enable authorize when all 4 digits are entered
            bool allFilled = Pin1.Text.Length == 1 && Pin2.Text.Length == 1
                          && Pin3.Text.Length == 1 && Pin4.Text.Length == 1;
            AuthorizeButton.IsEnabled = allFilled && !_isLocked;
        }

        private void PinDigit_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isLocked) return;

            if (e.Key == Key.Back && sender is TextBox currentBox && currentBox.Text.Length == 0)
            {
                // Move focus to previous box on backspace when empty
                if (currentBox == Pin4) Pin3.Focus();
                else if (currentBox == Pin3) Pin2.Focus();
                else if (currentBox == Pin2) Pin1.Focus();
            }
            else if (e.Key == Key.Enter)
            {
                bool allFilled = Pin1.Text.Length == 1 && Pin2.Text.Length == 1
                              && Pin3.Text.Length == 1 && Pin4.Text.Length == 1;
                if (allFilled)
                    ValidatePin();
            }
        }

        private void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            ValidatePin();
        }

        private void ValidatePin()
        {
            if (_isLocked) return;

            string enteredPin = $"{Pin1.Text}{Pin2.Text}{Pin3.Text}{Pin4.Text}";

            if (enteredPin == MockManagerPin)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                _failCount++;
                if (_failCount >= MaxFailAttempts)
                {
                    LockDialog();
                }
                else
                {
                    ShowError(TryFindResource("str_WrongPinMessage") as string
                              ?? "Incorrect PIN. Please try again.");
                    PlayShakeAnimation();
                    ClearPinFields();
                }
            }
        }

        private void ShowError(string message)
        {
            _errorMessage = message;
            ErrorMessageText.Text = message;
            ErrorMessageText.Visibility = Visibility.Visible;
        }

        private void LockDialog()
        {
            _isLocked = true;
            string lockedMessage = TryFindResource("str_DialogLocked") as string
                                   ?? "Too many failed attempts. Dialog locked.";
            ShowError(lockedMessage);

            Pin1.IsEnabled = false;
            Pin2.IsEnabled = false;
            Pin3.IsEnabled = false;
            Pin4.IsEnabled = false;
            AuthorizeButton.IsEnabled = false;

            PlayShakeAnimation();
        }

        private void PlayShakeAnimation()
        {
            var duration = TimeSpan.FromMilliseconds(50);
            var storyboard = new Storyboard();

            // Shake sequence: 0 -> 10 -> -10 -> 8 -> -8 -> 4 -> -4 -> 0
            double[] offsets = { 10, -10, 8, -8, 4, -4, 0 };
            var totalTime = TimeSpan.Zero;

            foreach (var offset in offsets)
            {
                var anim = new DoubleAnimation
                {
                    To = offset,
                    Duration = new Duration(duration),
                    BeginTime = totalTime
                };
                Storyboard.SetTarget(anim, PinContainer);
                Storyboard.SetTargetProperty(anim,
                    new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                storyboard.Children.Add(anim);
                totalTime += duration;
            }

            storyboard.Begin();
        }

        private void ClearPinFields()
        {
            Pin1.Text = string.Empty;
            Pin2.Text = string.Empty;
            Pin3.Text = string.Empty;
            Pin4.Text = string.Empty;
            Pin1.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

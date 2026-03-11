using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;

namespace TatweerPOS.Views.Dialogs
{
    /// <summary>
    /// Generic confirmation dialog for destructive or important actions.
    /// Use the static Show() helper for quick invocation.
    /// </summary>
    public partial class ConfirmDialog : Window
    {
        private string _title = string.Empty;
        private string _message = string.Empty;
        private string _confirmText = string.Empty;
        private string _cancelText = string.Empty;
        private PackIconKind _iconKind = PackIconKind.AlertCircleOutline;
        private bool _isDestructive;

        /// <summary>
        /// Gets or sets the dialog title.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                if (TitleText != null)
                    TitleText.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog message.
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                if (MessageText != null)
                    MessageText.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the confirm button text. Defaults to "Confirm" string resource.
        /// </summary>
        public string ConfirmText
        {
            get => _confirmText;
            set
            {
                _confirmText = value;
                if (ConfirmButton != null)
                    ConfirmButton.Content = value;
            }
        }

        /// <summary>
        /// Gets or sets the cancel button text. Defaults to "Cancel" string resource.
        /// </summary>
        public string CancelText
        {
            get => _cancelText;
            set
            {
                _cancelText = value;
                if (CancelButton != null)
                    CancelButton.Content = value;
            }
        }

        /// <summary>
        /// Gets or sets the icon displayed in the dialog.
        /// </summary>
        public PackIconKind IconKind
        {
            get => _iconKind;
            set
            {
                _iconKind = value;
                if (DialogIcon != null)
                    DialogIcon.Kind = value;
            }
        }

        /// <summary>
        /// Gets or sets whether this is a destructive action (confirm button will be red).
        /// </summary>
        public bool IsDestructive
        {
            get => _isDestructive;
            set
            {
                _isDestructive = value;
                ApplyButtonStyle();
            }
        }

        public ConfirmDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Apply properties that were set before window loaded
            if (!string.IsNullOrEmpty(_title))
                TitleText.Text = _title;
            if (!string.IsNullOrEmpty(_message))
                MessageText.Text = _message;
            if (!string.IsNullOrEmpty(_confirmText))
                ConfirmButton.Content = _confirmText;
            if (!string.IsNullOrEmpty(_cancelText))
                CancelButton.Content = _cancelText;

            DialogIcon.Kind = _iconKind;
            ApplyButtonStyle();
            PlayEntranceAnimation();
        }

        private void ApplyButtonStyle()
        {
            if (ConfirmButton == null) return;

            if (_isDestructive)
            {
                var style = TryFindResource("DestructiveButtonStyle") as Style;
                if (style != null)
                    ConfirmButton.Style = style;

                // Set icon to red/warning color
                if (DialogIcon != null)
                    DialogIcon.Foreground = TryFindResource("ErrorBrush") as Brush
                                            ?? new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44));
            }
            else
            {
                var style = TryFindResource("PrimaryButtonStyle") as Style;
                if (style != null)
                {
                    ConfirmButton.Style = style;
                    ConfirmButton.Height = 42;
                    ConfirmButton.MinWidth = 100;
                }

                if (DialogIcon != null)
                    DialogIcon.Foreground = TryFindResource("WarningBrush") as Brush
                                            ?? new SolidColorBrush(Color.FromRgb(0xF5, 0x9E, 0x0B));
            }
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

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Static helper to quickly show a confirmation dialog.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="message">Dialog message.</param>
        /// <param name="isDestructive">If true, confirm button is styled with ErrorBrush (red).</param>
        /// <returns>True if confirmed, false if cancelled, null if closed.</returns>
        public static bool? Show(Window owner, string title, string message, bool isDestructive = false)
        {
            var dialog = new ConfirmDialog
            {
                Owner = owner,
                Title = title,
                Message = message,
                IsDestructive = isDestructive
            };

            return dialog.ShowDialog();
        }
    }
}

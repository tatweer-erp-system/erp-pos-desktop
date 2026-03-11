using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TatweerPOS.Helpers;

/// <summary>
/// Animation helper per CLAUDE.md:
/// - Rule #4: Every screen has staggered entrance (60-80ms per element)
/// - Rule #5: Every interaction has feedback (shake on error)
/// - Standard durations: micro=100ms, fast=150ms, normal=200ms, page=300ms, entrance stagger=60ms
/// - Easing: CubicEaseOut for entrances, Linear for shake
/// </summary>
public static class AnimationHelper
{
    /// <summary>
    /// Staggered entrance: elements slide up + fade in sequentially.
    /// Per CLAUDE.md: 60ms stagger, 400ms fade, CubicEaseOut.
    /// </summary>
    public static void StaggeredEntrance(UIElement[] elements,
        double slideDistance = 30, int fadeDurationMs = 400, int staggerMs = 70)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            var el = elements[i] as FrameworkElement;
            if (el == null) continue;

            el.Opacity = 0;
            el.RenderTransform = new TranslateTransform(0, slideDistance);

            int delay = i * staggerMs;

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(fadeDurationMs))
            {
                BeginTime = TimeSpan.FromMilliseconds(delay),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var slideUp = new DoubleAnimation(slideDistance, 0, TimeSpan.FromMilliseconds(fadeDurationMs))
            {
                BeginTime = TimeSpan.FromMilliseconds(delay),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            el.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            el.RenderTransform.BeginAnimation(TranslateTransform.YProperty, slideUp);
        }
    }

    /// <summary>
    /// Shake animation for error feedback (420ms total, linear).
    /// Per CLAUDE.md Rule #5: destructive actions use shake animation.
    /// </summary>
    public static void Shake(FrameworkElement element)
    {
        if (element.RenderTransform is not TranslateTransform)
            element.RenderTransform = new TranslateTransform();

        var animation = new DoubleAnimationUsingKeyFrames();
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.Zero)));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(-10, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(60))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(10, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(120))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(-8, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(180))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(8, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(240))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(-4, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(4, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(360))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(420))));

        element.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
    }

    /// <summary>
    /// Logo scale-in with elastic ease (spring-like, 600ms).
    /// </summary>
    public static void ElasticScaleIn(FrameworkElement element, ScaleTransform scaleTransform, int delayMs = 300)
    {
        scaleTransform.ScaleX = 0;
        scaleTransform.ScaleY = 0;

        var easing = new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = 1, Springiness = 5 };

        var scaleX = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(600))
        {
            BeginTime = TimeSpan.FromMilliseconds(delayMs),
            EasingFunction = easing
        };
        var scaleY = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(600))
        {
            BeginTime = TimeSpan.FromMilliseconds(delayMs),
            EasingFunction = easing
        };

        scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
        scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);
    }

    /// <summary>
    /// Float animation for decorative orbs (SineEase per CLAUDE.md).
    /// </summary>
    public static void FloatAnimation(TranslateTransform transform, bool vertical, double distance, double durationSec)
    {
        var prop = vertical ? TranslateTransform.YProperty : TranslateTransform.XProperty;
        var anim = new DoubleAnimation(0, distance, TimeSpan.FromSeconds(durationSec))
        {
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever,
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
        };
        transform.BeginAnimation(prop, anim);
    }

    /// <summary>
    /// Scale pulse animation for decorative orbs.
    /// </summary>
    public static void ScalePulse(ScaleTransform transform, double maxScale, double durationSec)
    {
        var easing = new SineEase { EasingMode = EasingMode.EaseInOut };
        var animX = new DoubleAnimation(1, maxScale, TimeSpan.FromSeconds(durationSec))
        {
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever,
            EasingFunction = easing
        };
        var animY = new DoubleAnimation(1, maxScale, TimeSpan.FromSeconds(durationSec))
        {
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever,
            EasingFunction = easing
        };
        transform.BeginAnimation(ScaleTransform.ScaleXProperty, animX);
        transform.BeginAnimation(ScaleTransform.ScaleYProperty, animY);
    }

    /// <summary>
    /// Spinner rotation for loading state (700ms per rotation, linear).
    /// </summary>
    public static Storyboard CreateSpinnerStoryboard(string targetName)
    {
        var sb = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };
        var anim = new DoubleAnimation(0, 360, TimeSpan.FromMilliseconds(700));
        Storyboard.SetTargetName(anim, targetName);
        Storyboard.SetTargetProperty(anim, new PropertyPath(RotateTransform.AngleProperty));
        sb.Children.Add(anim);
        return sb;
    }

    /// <summary>
    /// Fade out animation (for success transition).
    /// </summary>
    public static DoubleAnimation CreateFadeOut(int delayMs = 600, int durationMs = 400)
    {
        return new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(durationMs))
        {
            BeginTime = TimeSpan.FromMilliseconds(delayMs),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
        };
    }
}

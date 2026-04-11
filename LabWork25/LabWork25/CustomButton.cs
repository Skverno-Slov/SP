using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LabWork25
{
    class CustomButton : FrameworkElement
    {
        private VisualCollection _children;
        private DrawingVisual _visual;

        private bool _isHovered;
        private bool _isPressed;

        private double _scale = 1.0;
        private double _targetScale = 1.0;

        public string Text = "";
        public int TextSize = 14;
        public Color BackgroundColor = Colors.DodgerBlue;
        public SolidColorBrush TextColor = Brushes.White;

        public CustomButton()
        {
            _children = new VisualCollection(this);
            _visual = new DrawingVisual();
            _children.Add(_visual);
            CompositionTarget.Rendering += OnRenderFrame;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
        }

        protected override int VisualChildrenCount => _children.Count;
        protected override Visual GetVisualChild(int index) => _children[index];

        private void Draw()
        {
            var text = new FormattedText(
                Text,
                CultureInfo.CurrentCulture, 
                FlowDirection.LeftToRight, 
                new Typeface("Segoe UI"), 
                TextSize, 
                TextColor, 
                1.25);

            var center = new Point(Width/2, Height/2);

            using (var dc = _visual.RenderOpen())
            {
                dc.PushTransform(new ScaleTransform(_scale, _scale, center.X, center.Y));

                dc.DrawRoundedRectangle(
                    new SolidColorBrush(Color.FromArgb(80, 0, 0, 0)),
                    null,
                    new Rect(7, 7, Width, Height),
                    10, 10);
                dc.DrawRoundedRectangle(
                    new SolidColorBrush(GetButtonColor()),
                    new Pen(new SolidColorBrush(Colors.DarkBlue), 1),
                    new Rect(0, 0, Width, Height),
                    10, 10);
                dc.DrawText(text, 
                    GetTextPosition(text));
                dc.Pop();
            }
        }


        private Color GetButtonColor()
        {
            if (!_isHovered)
            {
                _isPressed = false;
                return BackgroundColor;
            }
            if(_isPressed)
                return Colors.LightBlue;
            if (_isHovered)
                return GetHovereColor(BackgroundColor);
            return BackgroundColor;
        }

        private Color GetHovereColor(Color baseColor)
        {
            float delta = 1.2f;

            return Color.FromArgb(baseColor.A,
                (byte)Math.Min(255, baseColor.R * delta),
                (byte)Math.Min(255, baseColor.G * delta),
                (byte)Math.Min(255, baseColor.B * delta));
        }

        private Point GetTextPosition(FormattedText text) 
            => new Point((Width / 2) - (text.Width / 2), (Height / 2) - (text.Height / 2));

        private void OnRenderFrame(object? sender, EventArgs e)
        {
            _scale += (_targetScale - _scale) * 0.15;
            Draw();
        }

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent(
            "Click",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(CustomButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }


        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.GetPosition(this).X >= 0 && e.GetPosition(this).Y >= 0 && e.GetPosition(this).X <= Width && e.GetPosition(this).Y <= Height && _isPressed)
            {
                RaiseEvent(new RoutedEventArgs(ClickEvent, this));
            }
            _targetScale = 1.1;
            _isPressed = false;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _targetScale = 0.95;
            _isPressed = true;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _targetScale = 1.0;
            _isHovered = false;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            _targetScale = 1.1;
            _isHovered = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using BetterShell.Utils;

namespace BetterShell.StartMenu.Controls
{
    public partial class Folder : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(Folder), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
            nameof(Children), typeof(List<StartMenuItem>), typeof(Folder),
            new PropertyMetadata(default(List<StartMenuItem>)));

        public static readonly DependencyProperty ExpandedProperty = DependencyProperty.Register(
            nameof(Expanded), typeof(bool), typeof(Folder), new PropertyMetadata(default(bool)));

        public bool Expanded
        {
            get => (bool) GetValue(ExpandedProperty);
            set => SetValue(ExpandedProperty, value);
        }

        public List<StartMenuItem> Children
        {
            get => (List<StartMenuItem>) GetValue(ChildrenProperty);
            set => SetValue(ChildrenProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private readonly Storyboard _expandStoryboard = new Storyboard();
        private readonly Storyboard _collapseStoryboard = new Storyboard();

        public Folder()
        {
            InitializeComponent();
            UpdateLayout();

            var expandAnim = new DoubleAnimation(0, ItemsControl.DesiredSize.Height,
                new Duration(TimeSpan.FromSeconds(.25)));
            Storyboard.SetTarget(expandAnim, ItemsControl);
            Storyboard.SetTargetProperty(expandAnim, new PropertyPath(HeightProperty));
            _expandStoryboard.Children.Add(expandAnim);

            var collapseAnim = new DoubleAnimation(ItemsControl.DesiredSize.Height, 0,
                new Duration(TimeSpan.FromSeconds(.25)));
            Storyboard.SetTarget(collapseAnim, ItemsControl);
            Storyboard.SetTargetProperty(collapseAnim, new PropertyPath(HeightProperty));
            _collapseStoryboard.Children.Add(collapseAnim);

            VisibilityToggle.Checked += delegate
            {
                ItemsControl.Visibility = Visibility.Visible;
                _expandStoryboard.Begin(this);
            };


            VisibilityToggle.Unchecked += delegate
            {
                ItemsControl.Visibility = Visibility.Visible;
                _collapseStoryboard.Begin(this);
            };
            Loaded += delegate
            {
                ItemsControl.Visibility = Visibility.Visible;
                UpdateLayout();
                ((DoubleAnimation) _collapseStoryboard.Children[0]).From = ItemsControl.DesiredSize.Height;
                ((DoubleAnimation) _expandStoryboard.Children[0]).To = ItemsControl.DesiredSize.Height;
                ItemsControl.Visibility = Visibility.Collapsed;
                UpdateLayout();
            };
        }
    }
    
}
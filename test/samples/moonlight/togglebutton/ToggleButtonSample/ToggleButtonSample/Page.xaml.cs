﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ToggleButtonSample
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }
        private void togglebutton_click(object sender, RoutedEventArgs e)
        {
            var tb = sender as System.Windows.Controls.Primitives.ToggleButton;
            switch (tb.IsChecked)
            {
                case true:
                    if (tb.Name == "togglebutton1")
                    {
                        this.togglebutton1.Content = "Disable button2";
                        this.togglebutton2.IsEnabled = false;
                    }
                    if (tb.Name == "togglebutton2")
                        this.togglebutton2.Content = "Checked";
                    break;
                case false:
                    if (tb.Name == "togglebutton1")
                    {
                        this.togglebutton1.Content = "Enable button2";
                        this.togglebutton2.IsEnabled = true;
                    }
                    if (tb.Name == "togglebutton2")
                        this.togglebutton2.Content = "UnChecked";
                    break;
                case null:
                    if (tb.Name == "togglebutton2")
                        this.togglebutton2.Content = "Indetermined";
                    break;
            }
        }
    }
}

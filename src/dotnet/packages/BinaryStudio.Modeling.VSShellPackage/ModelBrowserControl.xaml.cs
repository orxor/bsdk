﻿using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    //[ProvideToolboxControl("BinaryStudio.Modeling.VSShellPackage.ModelBrowserToolboxControl", true)]
    public partial class ModelBrowserControl : UserControl
        {
        public ModelBrowserControl()
            {
            InitializeComponent();
            DataContext = null;
            }

        private void Button1_Click(object sender, RoutedEventArgs e)
            {
            MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.Button1_Click()", this.ToString()));
            }
        }
    }

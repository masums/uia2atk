#pragma checksum "C:\Users\Neville\Documents\Visual Studio 2008\Projects\MultiScaleImageSample\MultiScaleImageSample\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D7D21D99CFBBB8198CE2284EECA861DB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MultiScaleImageSample {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.MultiScaleImage msi;
        
        internal System.Windows.Controls.Button button1;
        
        internal System.Windows.Controls.Button button2;
        
        internal System.Windows.Controls.Button button3;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MultiScaleImageSample;component/Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.msi = ((System.Windows.Controls.MultiScaleImage)(this.FindName("msi")));
            this.button1 = ((System.Windows.Controls.Button)(this.FindName("button1")));
            this.button2 = ((System.Windows.Controls.Button)(this.FindName("button2")));
            this.button3 = ((System.Windows.Controls.Button)(this.FindName("button3")));
        }
    }
}
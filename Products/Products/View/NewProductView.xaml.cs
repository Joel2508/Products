﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Products.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewProductView : ContentPage
    {
        public NewProductView()
        {
            InitializeComponent();
        }
    }
}
﻿using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Admin;

public partial class AdminControl : IViewWithVM<AdminControlVM>
{
    public AdminControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<AdminControlVM>();
        DataContext = this;
    }

    public AdminControlVM ViewModel { get; set; }
}


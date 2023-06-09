﻿using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Admin.Testing;

public partial class EditAnswerControl : IViewWithVM<EditAnswerControlVM>
{
    public EditAnswerControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<EditAnswerControlVM>();
        DataContext = this;
    }

    public EditAnswerControlVM ViewModel { get; set; }
}




﻿#region MIT License (c) 2018 Dan Brandt

// Copyright 2018 Dan Brandt
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion MIT License (c) 2018 Dan Brandt

using AirSimApp.ViewModels;
using System.Windows;

namespace AirSimApp
{
    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs args)
        {
            _mainWindow.Close();
            _mainWindow = null;

            _viewModel.Dispose();
            _viewModel = null;

            base.OnExit(args);
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            _viewModel = new MainWindowViewModel();
            _mainWindow = new MainWindow();
            _mainWindow.DataContext = _viewModel;
            _mainWindow.Show();
        }

        private MainWindow _mainWindow;
        private MainWindowViewModel _viewModel;
    }
}
﻿using System.Windows.Controls;
using Testura.Mutation.Wpf.Helpers.Openers.Tabs;

namespace Testura.Mutation.Wpf.Sections.Shell
{
    public class TabContainer : IMainTabContainer
    {
        public void AddTab(TabItem userControl)
        {
            var shell = (ShellWindow)System.Windows.Application.Current.MainWindow;
            shell.AddTab(userControl);
        }

        public void RemoveTab(string name)
        {
            var shell = (ShellWindow)System.Windows.Application.Current.MainWindow;
            shell.RemoveTab(name);
        }

        public void RemoveAllTabs()
        {
            var shell = (ShellWindow)System.Windows.Application.Current.MainWindow;
            shell.RemoveAllTabs();
        }
    }
}
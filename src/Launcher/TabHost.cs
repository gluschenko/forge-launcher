﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Launcher
{
    public class TabHost
    {
        public StackPanel Panel { get; private set; }
        public Frame Frame { get; private set; }
        public Dictionary<TabButton, Page> Tabs { get; private set; }

        public TabHost(StackPanel panel, Frame frame, TabButton tabPattern, Page[] pages)
        {
            Panel = panel;
            Frame = frame;
            Tabs = new Dictionary<TabButton, Page>();
            //
            panel.Children.Clear();
            foreach (var page in pages)
            {
                var tab = CloneControl(tabPattern);
                tab.Content = page.Title;
                tab.Click += OnClick;
                panel.Children.Add(tab);
                Tabs.Add(tab, page);

                void OnClick(object sender, RoutedEventArgs args)
                {
                    Navigate(page);
                }
            }

            if (pages.Length > 0) Navigate(pages[0]);
        }

        public void Navigate(Page page)
        {
            if (Tabs.ContainsValue(page))
            {
                var pair = Tabs.First(p => p.Value == page);
                var tab = pair.Key;

                Frame.Navigate(page);

                foreach (var item in Tabs.Keys)
                {
                    item.Select(tab == item);
                }

                foreach (var item in Tabs.Values)
                {
                    try
                    {
                        if (page == item)
                        {
                            ((ITabPage)item).OnShown();
                        }
                        else
                        {
                            ((ITabPage)item).OnHidden();
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), e.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public T CloneControl<T>(T obj) where T : FrameworkElement
        {
            return (T)XamlReader.Parse(XamlWriter.Save(obj));
        }
    }

    public class TabButton : Button
    {
        public Style StyleDefault { get; set; } = null!;
        public Style StyleSelected { get; set; } = null!;

        public void Select(bool state)
        {
            Style = state ? StyleSelected : StyleDefault;
        }
    }

    public interface ITabPage
    {
        void OnShown();
        void OnHidden();
    }
}

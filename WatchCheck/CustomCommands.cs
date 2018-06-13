using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WatchCheck
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand SaveFile = new RoutedUICommand(
            "Save file",
            "SaveFile",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand CloseFile = new RoutedUICommand(
            "Close file",
            "CloseFile",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.C, ModifierKeys.Control)
            }
            );


    }
}

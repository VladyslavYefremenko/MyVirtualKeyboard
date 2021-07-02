using MyVirtualKeyboardControl.Enums;
using MyVirtualKeyboardControl.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace MyVirtualKeyboardControl
{
    [TemplatePart(Name = "PART_fullKeyboardItemsControl", Type = typeof(ItemsControl))]
    [TemplatePart(Name = "PART_mainKeyboardItemsControl", Type = typeof(ItemsControl))]
    [TemplatePart(Name = "PART_numpadItemsControl", Type = typeof(ItemsControl))]
    public class VirtualKeyboard : Control
    {
        static VirtualKeyboard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VirtualKeyboard), new FrameworkPropertyMetadata(typeof(VirtualKeyboard)));

            KeyboardChoosenTypeProperty = DependencyProperty.Register(nameof(KeyboardChoosenType), typeof(KeyboardType), typeof(VirtualKeyboard),
                new PropertyMetadata(KeyboardType.Close));
        }

        public VirtualKeyboard()
        {
            EventManager.RegisterClassHandler(typeof(UIElement), GotKeyboardFocusEvent, (KeyboardFocusChangedEventHandler)OnUIElementGotFocus);
            //EventManager.RegisterClassHandler(typeof(UIElement), MouseLeftButtonUpEvent, new RoutedEventHandler(LostFocus));
        }

        //public void LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (sender is )
        //    {
        //        Keyboard.Focus(sender as IInputElement);

        //        KeyboardChoosenType = KeyboardType.Close;
        //    }
        //}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CreateKeyboard();
        }

        public static readonly DependencyProperty KeyBackgroundProperty;
        public static readonly DependencyProperty KeyMarginProperty;
        public static readonly DependencyProperty KeyForegroundProperty;
        public static readonly DependencyProperty KeyboardChoosenTypeProperty;

        public ComboBoxItem LayoutList { get; set; }
        public List<ComboBoxItem> ComboBoxList { get; set; }
        public static bool ShiftIsActive = false;

        public static readonly DependencyProperty KeyMetadataProperty
            = DependencyProperty.RegisterAttached("SetKeyMetadataProperty", typeof(KeyMetadata), typeof(VirtualKeyboard),
                new PropertyMetadata());

        public static void SetKeyMetadataProperty(DependencyObject obj, KeyMetadata value)
        {
            obj.SetValue(KeyMetadataProperty, value);
        }

        public static KeyMetadata GetKeyMetadataProperty(DependencyObject obj)
        {
            return (KeyMetadata)obj.GetValue(KeyMetadataProperty);
        }

        public static readonly DependencyProperty KeyboardTypeProperty
            = DependencyProperty.RegisterAttached("SetKeyboardTypeProperty", typeof(KeyboardType), typeof(UIElement3D),
                new PropertyMetadata());

        public static void SetKeyboardTypeProperty(DependencyObject obj, KeyboardType value)
        {
            obj.SetValue(KeyboardTypeProperty, value);
        }

        public static KeyboardType GetKeyboardTypeProperty(DependencyObject obj)
        {
            return (KeyboardType)obj.GetValue(KeyboardTypeProperty);
        }

        public KeyboardType KeyboardChoosenType
        {
            get => (KeyboardType)base.GetValue(KeyboardChoosenTypeProperty);
            set => SetValue(KeyboardChoosenTypeProperty, value);
        }

        private void CreateKeyboard()
        {
            if (KeyboardChoosenType == KeyboardType.FullKeyboard)
            {
                CreateFullKeyboard();
            }
            else if (KeyboardChoosenType == KeyboardType.MainKeyboard)
            {
                CreateMainKeyboard();
            }
            else
            {
                CreateNumpud();
            }
        }

        private void CreateFullKeyboard()
        {
            ItemsControl itemsControl = GetTemplateChild("PART_fullKeyboardItemsControl") as ItemsControl;

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_Q, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_W, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_E, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_R, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_T, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_Y, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_U, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_I, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_O, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_P, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_4, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_6, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_5, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.BACK, 2, 1));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.TAB, 2, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_A, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_S, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_D, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_F, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_G, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_H, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_J, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_K, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_L, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_1, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_7, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.RETURN, 2, 2));

            itemsControl.Items.Add(SetOnKey(new ToggleButton(), VirtualKeyCode.SHIFT, 3, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_Z, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_X, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_C, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_V, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_B, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_N, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_M, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_COMMA, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_PERIOD, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_2, 1, 3));

            ComboBox comboBox = new ComboBox();
            comboBox = (ComboBox)SetOnKey(comboBox, 0, 2, 3);
            comboBox.HorizontalContentAlignment = HorizontalAlignment.Center;
            comboBox.VerticalContentAlignment = VerticalAlignment.Center;
            comboBox.ItemsSource = GetLayouts();
            comboBox.SelectedIndex = 0;
            comboBox.Focusable = false;
            itemsControl.Items.Add(comboBox);

            itemsControl.Items.Add(SetOnKey(new Button() { Content = "&123" }, 0, 2, 4));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.SPACE, 11, 4));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.LEFT, 1, 4));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.RIGHT, 1, 4));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD7, 1, 5));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD8, 1, 5));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD9, 1, 5));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD4, 1, 6));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD5, 1, 6));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD6, 1, 6));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD1, 1, 7));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD2, 1, 7));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD3, 1, 7));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD0, 1, 8));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.DECIMAL, 1, 8));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.BACK, 1, 8));

            foreach (UIElement key in itemsControl.Items)
            {
                key.SetValue(FontSizeProperty, 25.0);
                key.SetValue(MarginProperty, new Thickness(2));
                key.Focusable = false;
            }
        }

        private void CreateMainKeyboard()
        {
            ItemsControl itemsControl = GetTemplateChild("PART_mainKeyboardItemsControl") as ItemsControl;

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_Q, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_W, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_E, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_R, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_T, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_Y, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_U, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_I, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_O, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_P, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_4, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_6, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_5, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.BACK, 2, 1));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.TAB, 2, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_A, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_S, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_D, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_F, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_G, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_H, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_J, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_K, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_L, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_1, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_7, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.RETURN, 2, 2));

            itemsControl.Items.Add(SetOnKey(new ToggleButton(), VirtualKeyCode.SHIFT, 3, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_Z, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_X, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_C, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_V, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_B, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_N, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.VK_M, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_COMMA, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_PERIOD, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.OEM_2, 1, 3));

            ComboBox comboBox = new ComboBox();
            comboBox = (ComboBox)SetOnKey(comboBox, 0, 2, 3);
            comboBox.HorizontalContentAlignment = HorizontalAlignment.Center;
            comboBox.VerticalContentAlignment = VerticalAlignment.Center;
            comboBox.ItemsSource = GetLayouts();
            comboBox.SelectedIndex = 0;
            comboBox.Focusable = false;
            itemsControl.Items.Add(comboBox);

            itemsControl.Items.Add(SetOnKey(new Button() { Content = "&123" }, 0, 2, 4));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.SPACE, 11, 4));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.LEFT, 1, 4));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.RIGHT, 1, 4));

            foreach (UIElement key in itemsControl.Items)
            {
                key.SetValue(FontSizeProperty, 25.0);
                key.SetValue(MarginProperty, new Thickness(2));
                key.Focusable = false;
            }
        }

        public void CreateNumpud()
        {
            ItemsControl itemsControl = GetTemplateChild("PART_numpadItemsControl") as ItemsControl;

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD7, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD8, 1, 1));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD9, 1, 1));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD4, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD5, 1, 2));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD6, 1, 2));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD1, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD2, 1, 3));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD3, 1, 3));

            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.NUMPAD0, 1, 4));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.DECIMAL, 1, 4));
            itemsControl.Items.Add(SetOnKey(new RepeatButton(), VirtualKeyCode.BACK, 1, 4));

            foreach (UIElement key in itemsControl.Items)
            {
                key.SetValue(FontSizeProperty, 25.0);
                key.SetValue(MarginProperty, new Thickness(2));
                key.Focusable = false;
            }
        }
        private UIElement SetOnKey(UIElement button, VirtualKeyCode virtualKeyCode, double widthCoefficient, int row)
        {
            KeyMetadata keyMetadata = new KeyMetadata();

            keyMetadata.VirtualKeyCode = (ushort)virtualKeyCode;
            keyMetadata.WidthCoefficient = widthCoefficient;
            keyMetadata.Row = row;

            SetKeyMetadataProperty(button, keyMetadata);

            return button;
        }

        private List<ComboBoxItem> GetLayouts()
        {
            var layouts = new List<ComboBoxItem>();

            uint nElements = WinAPI.API.GetKeyboardLayoutList(0, null);
            IntPtr[] layoutsIds = new IntPtr[nElements];
            WinAPI.API.GetKeyboardLayoutList(layoutsIds.Length, layoutsIds);

            foreach (var layoutId in layoutsIds)
            {
                var languageId = (UInt16)((UInt32)layoutId & 0xFFFF);

                CultureInfo languageInfo = new CultureInfo(languageId, false);

                ComboBoxItem comboBoxItem = new ComboBoxItem();

                comboBoxItem.Content = languageInfo.ThreeLetterWindowsLanguageName;
                comboBoxItem.Tag = languageId;

                layouts.Add(comboBoxItem);
            }

            return layouts;
        }

        public virtual void OnUIElementGotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                KeyboardChoosenType = GetKeyboardTypeProperty(sender as DependencyObject) switch
                {
                    KeyboardType.FullKeyboard => KeyboardChoosenType = KeyboardType.FullKeyboard,
                    KeyboardType.MainKeyboard => KeyboardChoosenType = KeyboardType.MainKeyboard,
                    KeyboardType.Numpad => KeyboardChoosenType = KeyboardType.Numpad,
                };
            }
        }
    }
}

using System;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CryptoTools.UI.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CryptoTools"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CryptoTools;assembly=CryptoTools"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CheckedComboBox/>
    ///
    /// </summary>
    /// 
    
    public class CheckedComboBox : ComboBox
    {
        private ListBox _listbox;

        static CheckedComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckedComboBox), new FrameworkPropertyMetadata(typeof(CheckedComboBox)));
        }

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof (String), typeof (CheckedComboBox), new PropertyMetadata(default(String)));

        public String DefaultText
        {
            get { return (String) GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsTextProperty =
            DependencyProperty.Register("SelectedItemsText", typeof(String), typeof(CheckedComboBox), new FrameworkPropertyMetadata()
                                                                                                          {
                                                                                                              BindsTwoWayByDefault = true
                                                                                                          });

        public String SelectedItemsText
        {
            get { return (String)GetValue(SelectedItemsTextProperty); }
            set { SetValue(SelectedItemsTextProperty, value); }
        }

        public static readonly DependencyProperty DelimeterProperty =
            DependencyProperty.Register("Delimeter", typeof (String), typeof (CheckedComboBox), new PropertyMetadata(","));

        public String Delimeter
        {
            get { return (String) GetValue(DelimeterProperty); }
            set { SetValue(DelimeterProperty, value); }
        }

        public IList SelectedItems
        {
            get { return _listbox.SelectedItems; }
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            ToolTip = GetFormattedText(Environment.NewLine);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            if (this.Template != null)
            {
                _listbox = (ListBox)Template.FindName("ListBox", this);
                _listbox.SelectionChanged += (sender, args) =>
                                                 {
                                                     OnSelectionChanged(args);
                                                     UpdateText();
                                                     //Console.WriteLine("========");
                                                     //foreach (var selectedItem in _listbox.SelectedItems)
                                                     //{
                                                     //    Console.WriteLine(selectedItem);
                                                     //}
                                                     //Console.WriteLine("========");
                                                 };
            }
        }

        private void UpdateText()
        {
            if(_listbox == null || _listbox.SelectedItems.Count == 0)
            {
                SelectedItemsText = DefaultText;
                return;
            }
            Console.WriteLine(GetFormattedText(Delimeter));
            SelectedItemsText = GetFormattedText(Delimeter);
        }

        private String GetFormattedText(String delimeter)
        {
            var sb = new StringBuilder();
            foreach (var selectedItem in _listbox.SelectedItems)
            {
                sb.Append(selectedItem).Append(delimeter);
            }
            //chop off the last delimeter
            sb.Remove(sb.Length - delimeter.Length, delimeter.Length);
            return sb.ToString();
        }
    }
}

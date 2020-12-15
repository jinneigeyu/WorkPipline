using BroadcastGlobal.Lower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BroadcastGlobal.Higher;
using BroadcastGlobal;

namespace BroadcastExampleApp
{
    /// <summary>
    /// PageRight.xaml 的交互逻辑
    /// </summary>
    public partial class PageRight : Page
    {
        public PageRight()
        {
            InitializeComponent();
        }

     

        private void RightClicked_Lower(object sender, RoutedEventArgs e)
        {
            BroadcastLower<string>.Publish(Define.Key1, "Lower test from page left");
        }

        private void RightClicked_Higher(object sender, RoutedEventArgs e)
        {
            MyDefineContent content = new MyDefineContent() { IntContent = 8, StrContent = "Higher Content" };
            BroadcastHigher<BaseBroadContent>.Publish(Define.Key3, content);
        }

        private void RightClicked_Event(object sender, RoutedEventArgs e)
        {
            EventController.GetEvent<MyDefineEvnet>().Publish("Event from left page");
        }
    }
}

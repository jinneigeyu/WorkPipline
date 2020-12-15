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
using BroadcastGlobal;
using BroadcastGlobal.Lower;
using BroadcastGlobal.Higher;


namespace BroadcastExampleApp
{

    /// <summary>
    /// PageLeft.xaml 的交互逻辑
    /// </summary>
    public partial class PageLeft : Page
    {
        public PageLeft()
        {
            InitializeComponent();
            //注册
            BroadcastLower<string>.Resgister(Define.Key1, Fixmsg);
            BroadcastHigher<BaseBroadContent>.Resgister(Define.Key2, FixHigher);
            EventController.GetEvent<MyDefineEvnet>().Subscribe(FixeEvent);
        }

        private void FixeEvent(string obj)
        {
            MessageBox.Show(obj);
        }

        private void FixHigher(BaseBroadContent obj)
        {
            var content = obj.GetType() == typeof(MyDefineContent) ? (MyDefineContent)obj : null;
            MessageBox.Show(content.ToString());
        }

        private void Fixmsg(string msg)
        {
            MessageBox.Show(msg);
        }

        private void LeftClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}

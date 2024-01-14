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
using System.Windows.Shapes;
using IDZ_CLIENT.ViewModels;
using IDZ_CLIENT.Models;

namespace IDZ_CLIENT.Views
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public ElementOfArmor ElementOfArmor { get; set; }
        public EditWindow(ElementOfArmor elementOfArmor)
        {
            ElementOfArmor = elementOfArmor;
            DataContext = ElementOfArmor;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(
                ElementOfArmor.DEFENCE >= 0 && ElementOfArmor.NAME.Trim() != "" && 
                ElementOfArmor.PRICE >=0 && ElementOfArmor.WEIGHT >= 0
                )
            {
                this.DialogResult = true;
            }
        }
    }
}

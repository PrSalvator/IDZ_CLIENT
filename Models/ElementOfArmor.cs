using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace IDZ_CLIENT.Models
{
    public class ElementOfArmor : INotifyPropertyChanged
    {
        private double weight;
        private int defence;
        private int price;
        private string name;
        private bool textBoxesState = true;
        public int ID { get; set; }
        public string DEV_ID { get; set; }
        public int PART_ID { get; set; }
        public int ARMOR_ID { get; set; }
        public string NAME { get => name; set => name = value.Trim() == "" ? "No Name" : value; }
        public string IMG_URL { get; set; }
        public double WEIGHT { get => weight; set => weight = value <= 0 ? 0 : value; }
        public int DEFENCE { get => defence; set => defence = value <= 0 ? 0 : value; }
        public int PRICE { get => price; set => price = value <= 0 ? 0 : value; }

        public string PriceVM { get => PRICE.ToString(); set => PRICE = int.TryParse(value, out int num) ? num : PRICE; }
        public string WeightVM { get => WEIGHT.ToString(); set => WEIGHT = double.TryParse(value, out double num) ? num : WEIGHT; }
        public string DefenceVM { get => DEFENCE.ToString(); set => DEFENCE = int.TryParse(value, out int num) ? num : DEFENCE; }
        public bool TextBoxesState 
        {
            get
            {
                return textBoxesState;
            }
            set
            {
                textBoxesState = value;
                OnPropertyChanged("TextBoxesState");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

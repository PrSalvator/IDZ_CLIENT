using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDZ_CLIENT.Models
{
    public class ElementOfArmor
    {
        private double weight;
        private int defence;
        private int price;
        private string name;
        public int ID { get; set; }
        public string DEV_ID { get; set; }
        public int PART_ID { get; set; }
        public int ARMOR_ID { get; set; }
        public string NAME { get => name; set => name = value.Trim() == "" ? "No Name" : value; }
        public string IMG_URL { get; set; }
        public double WEIGHT { get => weight; set => weight = value <=0 ? 0 : value; }
        public int DEFENCE { get => defence; set => defence = value <= 0 ? 0 : value; }
        public int PRICE { get => price; set => price = value <= 0 ? 0 : value; }
    }
}

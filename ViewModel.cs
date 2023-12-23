using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IDZ_CLIENT
{
    public class ViewModel: INotifyPropertyChanged
    {
        public ViewModel()
        {
            
        }
        static string uri = "https://static.wikia.nocookie.net/elderscrolls/images/b/b9/%D0%A1%D1%8B%D1%80%D0%BE%D0%BC%D1%8F%D1%82%D0%BD%D1%8B%D0%B9_%D1%88%D0%BB%D0%B5%D0%BC_%28Skyrim%29.png/revision/latest?cb=20140929151640&path-prefix=ru";
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string imageSource = uri;
        public string ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }
    }
}

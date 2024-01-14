using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDZ_CLIENT.Models;

namespace IDZ_CLIENT.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ServerConnector serverConnector = new ServerConnector();
        private ObservableCollection<ElementOfArmor> collection = new ObservableCollection<ElementOfArmor>();
        private ElementOfArmor selectedItem = null;
        private RelayCommand editCommand;
        private RelayCommand deleteCommand;
        private RelayCommand updateCommand;

        public MainWindowViewModel()
        {
            Task.Run(GetDataFromServer);
        }
        public async Task GetDataFromServer()
        {
            string armors = serverConnector.GetDataFromServer();
            byte[] byteArray = Encoding.UTF8.GetBytes(armors);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
            try
            {
                Collection = await JsonSerializer.DeserializeAsync<ObservableCollection<ElementOfArmor>>(stream);
            }
            catch
            {
                Collection = null;
            }
        }

        public ObservableCollection<ElementOfArmor> Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
                OnPropertyChanged("Collection");
            }
        }
        public ElementOfArmor SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
            }
        }
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand(obj =>
                  {
                      if (SelectedItem != null)
                      {
                          Views.EditWindow editWindow = new Views.EditWindow(SelectedItem);
                          editWindow.ShowDialog();
                          if(editWindow.DialogResult == true)
                          {
                              string data = JsonSerializer.Serialize(SelectedItem);
                              serverConnector.SendEditData(data);
                          }
                      }
                  }));
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                  if (SelectedItem != null)
                      {
                          string data = JsonSerializer.Serialize(SelectedItem);
                          Collection.Remove(SelectedItem);
                          serverConnector.SendDeleteData(data);
                      }
                  }));
            }
        }
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand(obj =>
                  {
                      string armors = serverConnector.UpdateDate();
                      SelectedItem = null;
                      try
                      {
                          Collection = JsonSerializer.Deserialize<ObservableCollection<ElementOfArmor>>(armors);
                      }
                      catch
                      {
                          Collection = null;
                      }
                      
                  }));
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

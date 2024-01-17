using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IDZ_CLIENT.Models;

namespace IDZ_CLIENT.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ServerConnector serverConnector = new ServerConnector();
        private readonly ReportCreator reportCreator = new ReportCreator();
        private ObservableCollection<ElementOfArmor> collection = new ObservableCollection<ElementOfArmor>();
        private ElementOfArmor selectedItem = null;
        private RelayCommand editCommand;
        private RelayCommand deleteCommand;
        private RelayCommand updateCommand;
        private RelayCommand selectionChangedCommand;
        private RelayCommand reportCommand;
        private bool buttonsState = true;
        private bool reportButtonState = true;

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
                OnPropertyChanged("SelectedItem");
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
                          SelectedItem.TextBoxesState = false;
                          ButtonsState = false;
                      }
                  }));
            }
        }
        public RelayCommand SelectionChangedCommand
        {
            get
            {
                return selectionChangedCommand ??
                  (selectionChangedCommand = new RelayCommand(obj =>
                  {
                      if (SelectedItem != null)
                      {
                          if (!SelectedItem.TextBoxesState)
                          {
                              string data = JsonSerializer.Serialize(SelectedItem);
                              serverConnector.SendEditData(data);
                              SelectedItem.TextBoxesState = true;
                              ButtonsState = true;
                          }
                      }
                  }));
            }
        }
        public bool ReportButtonState
        {
            get
            {
                return reportButtonState;
            }
            set
            {
                reportButtonState = value;
                OnPropertyChanged("ReportButtonState");
            }
        }
        public bool ButtonsState { 
            get 
            {
                return buttonsState;
            }
            set
            {
                buttonsState = value;
                OnPropertyChanged("ButtonsState");
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
                  (updateCommand = new RelayCommand(async obj =>
                  {
                      ButtonsState = false;
                      await Task.Run(updateDate);
                      ButtonsState = true;
                  }));
            }
        }
        public RelayCommand ReportCommand
        {
            get
            {
                return reportCommand ??
                  (reportCommand = new RelayCommand(async obj =>
                  {
                      try
                      {
                          ReportButtonState = false;
                          if (reportCreator.ArmorList == null)
                          {
                              await Task.Run(updateReportDate);
                          }
                          using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                          {
                              System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                              reportCreator.path = dialog.SelectedPath;
                          }
                          await Task.Run(reportCreator.CreateReport);
                      }
                      catch
                      {
                          return;
                      }
                      finally
                      {
                          ReportButtonState = true;
                      }
                  }));
            }
        }
        private void updateDate()
        {
            string armors = serverConnector.UpdateDate();
            SelectedItem = null;

            updateReportDate();

            try
            {
                Collection = JsonSerializer.Deserialize<ObservableCollection<ElementOfArmor>>(armors);
            }
            catch
            {
                Collection = null;
            }
        }
        private void updateReportDate()
        {
            string armors = serverConnector.GetDataForReport();
            byte[] byteArray = Encoding.UTF8.GetBytes(armors);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
            try
            {
                reportCreator.ArmorList = JsonSerializer.Deserialize<List<Models.ArmorDefence>>(stream);
            }
            catch
            {
                reportCreator.ArmorList = null;
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

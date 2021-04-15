using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
  class ChessBoardConfiguration: INotifyPropertyChanged
  {
    private ObservableCollection<ObservableCollection<Square>> Configuration;

    public ObservableCollection<ObservableCollection<Square>> ChessBoard
    {
      get { return Configuration; }
      set
      {
        if (Configuration == value) return;
        Configuration = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChessBoard"));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}

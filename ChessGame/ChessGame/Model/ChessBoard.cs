using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
  class ChessBoard: INotifyPropertyChanged
  {
    public List<ChessPiece> ChessPieces;

    private ObservableCollection<ObservableCollection<Square>> configuration;

    public ObservableCollection<ObservableCollection<Square>> Configuration
    {
      get { return configuration; }
      set
      {
        if (configuration == value) return;
        configuration = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Configuration"));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}

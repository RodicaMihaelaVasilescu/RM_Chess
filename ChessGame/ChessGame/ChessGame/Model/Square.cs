using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessGame.Model
{
  class Square: INotifyPropertyChanged
  {
    private SolidColorBrush background;
    private string chessPiece;


    //public Brush Background { get; set; }
    public string Id { get; set; }
    public string ChessPieceName { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public override string ToString()
    {
      return this.Id;
    }

    public SolidColorBrush Background
    {
      get { return background; }
      set
      {
        if (background == value) return;
        background = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Background"));
      }
    }

    public String ChessPiece
    {
      get { return chessPiece; }
      set
      {
        if (chessPiece == value) return;
        chessPiece = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChessPiece"));
      }
    }

    public bool IsWhite { get; set; } = true;
    public double ChessPieceSize { get; set; } = 50;
    public double Width { get; set; } = 60;
    public double Height { get; set; } = 60;
  }
}

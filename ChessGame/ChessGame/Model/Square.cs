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
  class Square : INotifyPropertyChanged
  {
    private SolidColorBrush background;

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

    private ChessPiece piece = new ChessPiece();

    public ChessPiece Piece
    {
      get { return piece; }
      set
      {
        if (piece == value) return;
        piece = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Piece"));
      }
    }

 
    public double SquareSize { get; set; } = 60;

    private String chessPiece;
    public String ChessPieceIcon
    {
      get { return chessPiece; }
      set
      {
        if (chessPiece == value) return;
        chessPiece = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChessPieceIcon"));
      }
    }

  }
}

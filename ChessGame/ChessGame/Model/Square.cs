using System;
using System.ComponentModel;
using System.Windows.Media;

namespace ChessGame.Model
{
  class Square : INotifyPropertyChanged
  {

    private ChessPiece piece = new ChessPiece();
    private SolidColorBrush background;
    public string Id { get; set; }
    public string ChessPieceName { get; set; }
    public double SquareSize { get; set; } = 53;

    private string chessPieceIcon;

    public Square() { }
    public Square(Square square)
    {
      this.background = square.Background;

      var newPiece = Activator.CreateInstance(piece.GetType(), new object[] { square.Piece });
      this.Piece = (ChessPiece)newPiece;
      this.Id = square.Id;
      this.ChessPieceIcon = square.ChessPieceIcon;
      this.ChessPieceName = square.ChessPieceName;
    }

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

    public string ChessPieceIcon
    {
      get { return chessPieceIcon; }
      set
      {
        if (chessPieceIcon == value) return;
        chessPieceIcon = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChessPieceIcon"));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

  }
}

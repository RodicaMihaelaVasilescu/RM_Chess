using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ChessGame.Mapper;
using System.Collections.ObjectModel;
//using ChessGame.Annotations;


namespace ChessGame.Model
{
  class ChessPiece : INotifyPropertyChanged
  {
    private string chessPieceIcon = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/";

    public double Size { get; set; } = 50;
    public string Location { get; set; }
    public Pair Coordinates { get; set; }
    public string ChessPieceName { get; set; }
    public string ChessPieceType { get; set; }
    public bool IsWhite { get; set; }

    public ChessPieceLocation mapper = ChessPieceLocation.Instance;

    public ChessPiece() { }

    public ChessPiece(ChessPiece piece)
    {
      this.chessPieceIcon = piece.chessPieceIcon;
      this.ChessPieceName = piece.ChessPieceName;
      this.Location = piece.Location;
      this.IsWhite = piece.IsWhite;
      this.mapper = piece.mapper;
      this.ChessPieceType = piece.ChessPieceType;
      this.Coordinates = piece.Coordinates;
    }

    public String ChessPieceIcon
    {
      get { return chessPieceIcon; }
      set
      {
        if (chessPieceIcon == value) return;
        chessPieceIcon = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChessPieceIcon"));
      }
    }

    public virtual List<Square> GetAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      return new List<Square>();
    }


    public List<Square> DiagonalAndLinearMove(int offsetX, int offsetY, ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard)
    {
      List<Square> moves = new List<Square>();

      if (piece == null || piece.Location == null)
        return moves;
      for (int k = 1; k <= 8; k++)
      {
        char letter = (char)(piece.Location[0] + offsetX * k);
        char digit = (char)(piece.Location[1] - offsetY * k);
        if (!(letter >= 'A' && letter <= 'H' && digit >= '1' && digit <= '8'))
        {
          continue;
        }
        var location = letter.ToString() + digit.ToString();
        var c = mapper.StringToCoordinates[letter.ToString() + digit.ToString()];
        if (pieces.Any(p => p.IsWhite == piece.IsWhite && p.Location == location))
        {
          break;
        }
        moves.Add(chessBoard[c.i][c.j]);
        if (pieces.Any(p => p.IsWhite != piece.IsWhite && p.Location == location))
        {
          break;
        }
      }

      return moves;
    }


    public event PropertyChangedEventHandler PropertyChanged;

    // [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}

using ChessGame.Mapper;
using ChessGame.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ChessGame.Model
{
  class Bishop : ChessPiece, INotifyPropertyChanged
  {
    public Bishop(bool isWhite, string location)
    {
      IsWhite = isWhite;
      Location = location;
      Coordinates = ChessPieceLocation.Instance.StringToCoordinates[location];

      if (isWhite)
      {
        ChessPieceIcon += Resources.WhiteBishop + ".png";
        ChessPieceName = Resources.WhiteBishop;
      }
      else
      {
        ChessPieceIcon += Resources.BlackBishop + ".png";
        ChessPieceName = Resources.BlackBishop;

      }
    }
    public override List<Square> GetAvailableMoves(Square SelectedSquare, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> bishopMoves = new List<Square>();

      bishopMoves.AddRange(DiagonalAndLinearMove(1, 1, SelectedSquare, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(1, -1, SelectedSquare, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(-1, -1, SelectedSquare, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(-1, 1, SelectedSquare, pieces, chessBoard));

      return bishopMoves;
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}

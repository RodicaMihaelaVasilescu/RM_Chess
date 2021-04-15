using ChessGame.Mapper;
using ChessGame.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
  class Rook : ChessPiece, INotifyPropertyChanged
  {
    public Rook(bool isWhite, string location)
    {
      IsWhite = isWhite;
      Location = location;
      Coordinates = ChessPieceLocation.Instance.StringToCoordinates[location];

      if (isWhite)
      {
        ChessPieceIcon += Resources.WhiteRook + ".png";
        ChessPieceName = Resources.WhiteRook;
      }
      else
      {
        ChessPieceIcon += Resources.BlackRook + ".png";
        ChessPieceName = Resources.BlackRook;

      }
    }

    public override List<Square> GetAvailableMoves(Square SelectedSquare, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> rookMoves = new List<Square>();

      rookMoves.AddRange(DiagonalAndLinearMove(0, 1, SelectedSquare, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(0, -1, SelectedSquare, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(-1, 0, SelectedSquare, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(1, 0, SelectedSquare, pieces, chessBoard));

      return rookMoves;
    }


    public event PropertyChangedEventHandler PropertyChanged;
  }
}

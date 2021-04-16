using ChessGame.Mapper;
using ChessGame.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChessGame.Model
{
  class Bishop : ChessPiece
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
      ChessPieceType = Resources.Bishop;
    }
    public override List<Square> GetAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> bishopMoves = new List<Square>();

      bishopMoves.AddRange(DiagonalAndLinearMove(1, 1, piece, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(1, -1, piece, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(-1, -1, piece, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(-1, 1, piece, pieces, chessBoard));

      return bishopMoves;
    }
  }
}

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
      ChessPieceType = Resources.Bishop;
      ChessPieceIcon += (isWhite ? Resources.WhiteBishop : Resources.BlackBishop) + ".png";
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

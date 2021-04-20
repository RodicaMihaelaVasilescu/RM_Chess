using ChessGame.Mapper;
using ChessGame.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChessGame.Model
{
  class Rook : ChessPiece
  {
    public Rook(bool isWhite, string location)
    {
      IsWhite = isWhite;
      Location = location;
      ChessPieceIcon += (isWhite ? Resources.WhiteRook : Resources.BlackRook) + ".png";
      ChessPieceType = Resources.Rook;
    }

    public override List<Square> GetAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> rookMoves = new List<Square>();

      rookMoves.AddRange(DiagonalAndLinearMove(0, 1, piece, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(0, -1, piece, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(-1, 0, piece, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(1, 0, piece, pieces, chessBoard));

      return rookMoves;
    }
  }
}

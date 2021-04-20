using ChessGame.Mapper;
using ChessGame.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChessGame.Model
{
  class Queen : ChessPiece
  {
    public Queen(bool isWhite, string location)
    {
      IsWhite = isWhite;
      Location = location;
      ChessPieceIcon += (isWhite ? Resources.WhiteQueen : Resources.BlackQueen) + ".png";
      ChessPieceType = Resources.Queen;
    }
    public override List<Square> GetAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> queenpMoves = new List<Square>();

      queenpMoves.AddRange(DiagonalAndLinearMove(1, 1, piece, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(1, -1, piece, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(-1, -1, piece, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(-1, 1, piece, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(0, 1, piece, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(0, -1, piece, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(-1, 0, piece, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(1, 0, piece, pieces, chessBoard));

      return queenpMoves;
    }

  }
}

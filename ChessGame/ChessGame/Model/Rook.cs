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

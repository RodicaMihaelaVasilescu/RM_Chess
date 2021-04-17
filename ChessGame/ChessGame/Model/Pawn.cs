using ChessGame.Mapper;
using ChessGame.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessGame.Model
{
  class Pawn : ChessPiece
  {
    public Pawn(bool isWhite, string location)
    {
      IsWhite = isWhite;
      Location = location;
      Coordinates = ChessPieceLocation.Instance.StringToCoordinates[location];

      if (isWhite)
      {
        ChessPieceIcon += Resources.WhitePawn + ".png";
        ChessPieceName = Resources.WhitePawn;
      }
      else
      {
        ChessPieceIcon += Resources.BlackPawn + ".png";
        ChessPieceName = Resources.BlackPawn;

      }
      ChessPieceType = Resources.Pawn;


    }
    public override List<Square> GetAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      return GetPawnAvailableMoves(piece, pieces, chessBoard, Movements);
    }

  }
}

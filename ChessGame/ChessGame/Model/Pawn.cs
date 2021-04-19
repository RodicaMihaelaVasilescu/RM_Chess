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

      List<Square> pawnMoves = new List<Square>();

      string location = piece.Location;
      int myOffset = piece.IsWhite ? 1 : -1;
      string newLocation = location[0].ToString() + ((char)(location[1] + myOffset)).ToString();

      if (newLocation[1] >= '1' && newLocation[1] <= '8' && !pieces.Any(p => p.Location == newLocation))
      {
        var c = mapper.StringToCoordinates[newLocation];
        pawnMoves.Add(chessBoard[c.i][c.j]);

        newLocation = location[0].ToString() + ((char)(location[1] + 2 * myOffset)).ToString();
        if (newLocation[1] >= '1' && newLocation[1] <= '8' && !pieces.Any(p => p.Location == newLocation) && !Movements.ContainsKey(location) && !Movements.ContainsValue(location))
        {
          c = mapper.StringToCoordinates[newLocation];
          pawnMoves.Add(chessBoard[c.i][c.j]);
        }
      }

      newLocation = ((char)(location[0] - 1)).ToString() + ((char)(location[1] + myOffset)).ToString();

      if (newLocation[0] >= 'A' && newLocation[0] <= 'H' &&
        newLocation[1] >= '0' && newLocation[1] <= '9' &&
       pieces.Any(p => p.Location == newLocation && p.IsWhite != piece.IsWhite))
      {
        var c = mapper.StringToCoordinates[newLocation];
        pawnMoves.Add(chessBoard[c.i][c.j]);
      }

      newLocation = ((char)(location[0] + 1)).ToString() + ((char)(location[1] + myOffset)).ToString();

      if (newLocation[0] >= 'A' && newLocation[0] <= 'H' &&
        newLocation[1] >= '0' && newLocation[1] <= '9' &&
       pieces.Any(p => p.Location == newLocation && p.IsWhite != piece.IsWhite))
      {
        var c = mapper.StringToCoordinates[newLocation];
        pawnMoves.Add(chessBoard[c.i][c.j]);
      }

      return pawnMoves;
    }
  }
}

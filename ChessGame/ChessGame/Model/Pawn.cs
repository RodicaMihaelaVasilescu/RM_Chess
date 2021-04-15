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
  class Pawn : ChessPiece, INotifyPropertyChanged
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


    }
    public override List<Square> GetAvailableMoves( Square SelectedSquare, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> pawnMoves = new List<Square>();

      string location = SelectedSquare.Piece.Location;
      int myOffset = SelectedSquare.Piece.IsWhite ? 1 : -1;
      string newLocation = location[0].ToString() + ((char)(location[1] + myOffset)).ToString();

      if (newLocation[1] >= '1' && newLocation[1] <= '8' && !pieces.Any(p => p.Location == newLocation))
      {
        var c = mapper.StringToCoordinates[newLocation];
        pawnMoves.Add(chessBoard[c.i][c.j]);

        newLocation = location[0].ToString() + ((char)(location[1] + 2 * myOffset)).ToString();
        if (newLocation[1] >= '1' && newLocation[1] <= '8' && !pieces.Any(p => p.Location == newLocation) && !Movements.ContainsValue(location))
        {
          c = mapper.StringToCoordinates[newLocation];
          pawnMoves.Add(chessBoard[c.i][c.j]);
        }
      }

      newLocation = ((char)(location[0] - 1)).ToString() + ((char)(location[1] + myOffset)).ToString();

      if (newLocation[0] >= 'A' && newLocation[0] <= 'H' &&
        newLocation[1] >= '0' && newLocation[1] <= '9' &&
       pieces.Any(p => p.Location == newLocation && p.IsWhite != SelectedSquare.Piece.IsWhite))
      {
        var c = mapper.StringToCoordinates[newLocation];
        pawnMoves.Add(chessBoard[c.i][c.j]);
      }

      newLocation = ((char)(location[0] + 1)).ToString() + ((char)(location[1] + myOffset)).ToString();

      if (newLocation[0] >= 'A' && newLocation[0] <= 'H' &&
        newLocation[1] >= '0' && newLocation[1] <= '9' &&
       pieces.Any(p => p.Location == newLocation && p.IsWhite != SelectedSquare.Piece.IsWhite))
      {
        var c = mapper.StringToCoordinates[newLocation];
        pawnMoves.Add(chessBoard[c.i][c.j]);
      }

      return pawnMoves;
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}

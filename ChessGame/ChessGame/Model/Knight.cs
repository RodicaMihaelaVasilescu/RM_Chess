using ChessGame.Mapper;
using ChessGame.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessGame.Model
{
  class Knight : ChessPiece
  {
    public Knight(bool isWhite, string location)
    {
      IsWhite = isWhite;
      Location = location;
      Coordinates = ChessPieceLocation.Instance.StringToCoordinates[location];

      if (isWhite)
      {
        ChessPieceIcon += Resources.WhiteHorse + ".png";
        ChessPieceName = Resources.WhiteHorse;
      }
      else
      {
        ChessPieceIcon += Resources.BlackHorse + ".png";
        ChessPieceName = Resources.BlackHorse;

      }
      ChessPieceType = Resources.Knight;
    }
    public override List<Square> GetAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> knightMoves = new List<Square>();

      List<int> lin = new List<int>() { 2, -2, 2, -2, 1, -1, 1, -1 };
      List<int> col = new List<int>() { 1, 1, -1, -1, 2, 2, -2, -2 };

      List<string> directions = new List<string>();

      for (int i = 0; i < 8; i++)
      {
        char letter = (char)(piece.Location[0] + lin[i]);
        char digit = (char)(piece.Location[1] + col[i]);
        if (letter >= 'A' && letter <= 'H' && digit >= '1' && digit <= '8')
          directions.Add(letter.ToString() + digit.ToString());

      }

      foreach (var dir in directions)
      {

        //  if there's no piece with the same color
        if (pieces.FirstOrDefault(p => p.Location == dir && p.IsWhite == piece.IsWhite) == null)
        {
          var c = mapper.StringToCoordinates[dir];
          knightMoves.Add(chessBoard[c.i][c.j]);
        }
      }

      return knightMoves;
    }
  }
}

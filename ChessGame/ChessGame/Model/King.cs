using ChessGame.Mapper;
using ChessGame.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessGame.Model
{
  class King : ChessPiece
  {
    public King(bool isWhite, string location)
    {
      IsWhite = isWhite;
      Location = location;
      Coordinates = ChessPieceLocation.Instance.StringToCoordinates[location];

      if (isWhite)
      {
        ChessPieceIcon += Resources.WhiteKing + ".png";
        ChessPieceName = Resources.WhiteKing;
      }
      else
      {
        ChessPieceIcon += Resources.BlackKing + ".png";
        ChessPieceName = Resources.BlackKing;

      }
      ChessPieceType = Resources.King;
    }

    public override List<Square> GetAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<int> lin = new List<int>() { 0, 0, 1, 1, 1, -1, -1, -1 };
      List<int> col = new List<int>() { 1, -1, 0, 1, -1, 0, 1, -1 };

      List<Square> moves = new List<Square>();

      for (int i = 0; i < 8; i++)
      {

        char letter = (char)(piece.Location[0] + lin[i]);
        char digit = (char)(piece.Location[1] + col[i]);
        var location = letter.ToString() + digit.ToString();
        if (!(letter >= 'A' && letter <= 'H' && digit >= '1' && digit <= '8') || pieces.Any(p => p.IsWhite == piece.IsWhite && p.Location == location))
        {
          continue;
        }
        var c = mapper.StringToCoordinates[letter.ToString() + digit.ToString()];
        moves.Add(chessBoard[c.i][c.j]);
      }
      return moves;

    }
  }
}

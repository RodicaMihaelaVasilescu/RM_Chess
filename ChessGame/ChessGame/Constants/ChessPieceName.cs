using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Constants
{
  public class ChessPieceName
  {
    public const string NoChessPiece = "no_chess_piece";
    public const string WhitePawn = "white_pawn";
    public const string BlackPawn = "black_pawn";

    public bool IsPawn(string piece)
    {
      return piece.Contains("pawn") == true;
    }
  }
}

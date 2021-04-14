using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
  class ChessBoardConfiguration
  {
    //  private static ChessBoardConfiguration _instance;
    //  public static ChessBoardConfiguration Instance => _instance ?? (_instance = new ChessBoardConfiguration());

    //  public Collection<Square> ChessBoard;
    //  public ChessBoardConfiguration()
    //  {
    //    Emoticons = new Collection<Square>();
    //    Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/1.gif", Text = ":)" });

    //    ChessBoard = new List<List<Square>>();

    //    for (int i = 0; i < 8; i++)
    //    {
    //      List<Square> squareList = new List<Square>();
    //      for (int j = 0; j < 8; j++)
    //      {
    //        var square = new Square();
    //        if ((i + j) % 2 == 0)
    //        {
    //          square.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f0d9b5")); // white
    //        }
    //        else
    //        {
    //          square.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#b58863")); // black
    //        }
    //        squareList.Add(square);
    //      }
    //      Model.ChessBoardConfiguration.Add(squareList);
    //    }


    //    for (int i = 0; i < 8; i++)
    //    {
    //      ChessBoard[i][1].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_pawn.png";
    //      ChessBoard[i][6].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_pawn.png";
    //    }

    //    ChessBoard[0][0].ChessPiece = ChessBoard[7][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_rook.png";
    //    ChessBoard[0][7].ChessPiece = ChessBoard[7][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_rook.png";

    //    ChessBoard[1][0].ChessPiece = ChessBoard[6][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_horse.png";
    //    ChessBoard[1][7].ChessPiece = ChessBoard[6][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_horse.png";

    //    ChessBoard[2][0].ChessPiece = ChessBoard[5][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_bishop.png";
    //    ChessBoard[2][7].ChessPiece = ChessBoard[5][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_bishop.png";

    //    ChessBoard[3][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_queen.png";
    //    ChessBoard[4][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_queen.png";

    //    ChessBoard[4][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_king.png";
    //    ChessBoard[3][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_king.png";
    //  }
  }
}

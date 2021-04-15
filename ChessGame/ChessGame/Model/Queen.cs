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
  class Queen : ChessPiece, INotifyPropertyChanged
  {
    public Queen(bool isWhite, string location)
    {
      IsWhite = isWhite;
      Location = location;
      Coordinates = ChessPieceLocation.Instance.StringToCoordinates[location];

      if (isWhite)
      {
        ChessPieceIcon += Resources.WhiteQueen + ".png";
        ChessPieceName = Resources.WhiteQueen;
      }
      else
      {
        ChessPieceIcon += Resources.BlackQueen + ".png";
        ChessPieceName = Resources.BlackQueen;

      }

    }
    public override List<Square> GetAvailableMoves(Square SelectedSquare, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> queenpMoves = new List<Square>();

      queenpMoves.AddRange(DiagonalAndLinearMove(1, 1, SelectedSquare, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(1, -1, SelectedSquare, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(-1, -1, SelectedSquare, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(-1, 1, SelectedSquare, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(0, 1, SelectedSquare, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(0, -1, SelectedSquare, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(-1, 0, SelectedSquare, pieces, chessBoard));
      queenpMoves.AddRange(DiagonalAndLinearMove(1, 0, SelectedSquare, pieces, chessBoard));

      return queenpMoves;
    }



    public event PropertyChangedEventHandler PropertyChanged;
  }
}

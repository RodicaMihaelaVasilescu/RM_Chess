using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ChessGame.Mapper;
using System.Collections.ObjectModel;
using ChessGame.Properties;
//using ChessGame.Annotations;


namespace ChessGame.Model
{
  class ChessPiece : INotifyPropertyChanged
  {
    public ChessPiece() { }

    public ChessPiece(ChessPiece piece)
    {
      this.chessPieceIcon = piece.chessPieceIcon;
      this.ChessPieceName = piece.ChessPieceName;
      this.Location = piece.Location;
      this.IsWhite = piece.IsWhite;
      this.mapper = piece.mapper;
      this.ChessPieceType = piece.ChessPieceType;
      this.Coordinates = piece.Coordinates;
    }
    private string chessPieceIcon = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/";

    public double Size { get; set; } = 50;
    public string Location { get; set; }

    public Pair Coordinates { get; set; }
    public string ChessPieceName { get; set; }
    public string ChessPieceType { get; set; }

    public ChessPieceLocation mapper = ChessPieceLocation.Instance;

    public bool IsWhite { get; set; }

    public String ChessPieceIcon
    {
      get { return chessPieceIcon; }
      set
      {
        if (chessPieceIcon == value) return;
        chessPieceIcon = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChessPieceIcon"));
      }
    }

    public virtual List<Square> GetAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)

    {
      if (piece.ChessPieceType.Contains(Resources.Queen))
      {
        return GetQueenAvailableMoves(piece, pieces, chessBoard, Movements);
      }
      else if (piece.ChessPieceType.Contains(Resources.King))
      {
        return GetKingAvailableMoves(piece, pieces, chessBoard, Movements);
      }
      else if (piece.ChessPieceType.Contains(Resources.Pawn))
      {
        return GetPawnAvailableMoves(piece, pieces, chessBoard, Movements);
      }
      else if (piece.ChessPieceType.Contains(Resources.Knight))
      {
        return GetKnightAvailableMoves(piece, pieces, chessBoard, Movements);
      }
      else if (piece.ChessPieceType.Contains(Resources.Rook))
      {
        return GetRookAvailableMoves(piece, pieces, chessBoard, Movements);
      }
      else if (piece.ChessPieceType.Contains(Resources.Bishop))
      {
        return GetBishopAvailableMoves(piece, pieces, chessBoard, Movements);
      }
      return new List<Square>();
    }


    public List<Square> GetPawnAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
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
        if (newLocation[1] >= '1' && newLocation[1] <= '8' && !pieces.Any(p => p.Location == newLocation) && !Movements.ContainsKey(location) &&!Movements.ContainsValue(location))
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
    public List<Square> GetQueenAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
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

    public List<Square> GetBishopAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> bishopMoves = new List<Square>();

      bishopMoves.AddRange(DiagonalAndLinearMove(1, 1, piece, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(1, -1, piece, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(-1, -1, piece, pieces, chessBoard));
      bishopMoves.AddRange(DiagonalAndLinearMove(-1, 1, piece, pieces, chessBoard));

      return bishopMoves;
    }


    public List<Square> GetRookAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
    {
      List<Square> rookMoves = new List<Square>();

      rookMoves.AddRange(DiagonalAndLinearMove(0, 1, piece, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(0, -1, piece, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(-1, 0, piece, pieces, chessBoard));
      rookMoves.AddRange(DiagonalAndLinearMove(1, 0, piece, pieces, chessBoard));

      return rookMoves;
    }

    public List<Square> GetKingAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
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
    public List<Square> DiagonalAndLinearMove(int offsetX, int offsetY, ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard)
    {
      List<Square> moves = new List<Square>();

      if (piece == null || piece.Location == null)
        return moves;
      for (int k = 1; k <= 8; k++)
      {
        char letter = (char)(piece.Location[0] + offsetX * k);
        char digit = (char)(piece.Location[1] - offsetY * k);
        if (!(letter >= 'A' && letter <= 'H' && digit >= '1' && digit <= '8'))
        {
          continue;
        }
        var location = letter.ToString() + digit.ToString();
        var c = mapper.StringToCoordinates[letter.ToString() + digit.ToString()];
        if (pieces.Any(p => p.IsWhite == piece.IsWhite && p.Location == location))
        {
          break;
        }
        moves.Add(chessBoard[c.i][c.j]);
        if (pieces.Any(p => p.IsWhite != piece.IsWhite && p.Location == location))
        {
          break;
        }
      }

      return moves;
    }

    public List<Square> GetKnightAvailableMoves(ChessPiece piece, List<ChessPiece> pieces, ObservableCollection<ObservableCollection<Square>> chessBoard, Dictionary<string, string> Movements)
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


    public event PropertyChangedEventHandler PropertyChanged;

    // [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}

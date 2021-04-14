using ChessGame.Model;
using ChessGame.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChessGame.ViewModel
{
  class MainViewModel : INotifyPropertyChanged
  {
    private ObservableCollection<ObservableCollection<Square>> chessBoard;
    private Square selectedSquare;
    private Square selectedPiece;
    List<Square> currentMoves;
    SolidColorBrush White = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f0d9b5")); // white
    SolidColorBrush Black = (SolidColorBrush)(new BrushConverter().ConvertFrom("#b58863")); // black
    SolidColorBrush FocusedItemBackground = Brushes.Olive;
    SolidColorBrush CapturePieceBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cdd26a"));
    private string availableSquare = @"pack://application:,,,/ChessGame;component/Resources/available_square.png";
    private string availablePiece = @"pack://application:,,,/ChessGame;component/Resources/available_piece.png";

    public ObservableCollection<ObservableCollection<Square>> ChessBoard
    {
      get { return chessBoard; }
      set
      {
        if (chessBoard == value) return;
        chessBoard = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChessBoard"));
      }
    }

    public Square SelectedSquare
    {
      get { return selectedSquare; }
      set
      {
        if (selectedSquare == value) return;
        selectedSquare = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedSquare"));
      }
    }
    public Square SelectedPiece
    {
      get { return selectedPiece; }
      set
      {
        if (selectedPiece == value) return;
        selectedPiece = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedPiece"));
      }
    }
    public MainViewModel()
    {

    }

    public void InitializeChessBoard()
    {
      ChessBoard = new ObservableCollection<ObservableCollection<Square>>();

      char letter = 'A';
      string id;
      for (int i = 0; i < 8; i++)
      {
        char number = '8';
        ObservableCollection<Square> squareList = new ObservableCollection<Square>();
        for (int j = 0; j < 8; j++)
        {
          id = letter.ToString() + number.ToString();
          number--;
          var square = new Square();
          square.Id = id;
          square.ChessPieceName = Resources.NoPiece;
          if ((i + j) % 2 == 0)
          {
            square.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f0d9b5")); // white
          }
          else
          {
            square.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#b58863")); // black
          }
          squareList.Add(square);
        }
        letter++;
        ChessBoard.Add(squareList);
      }


      for (int i = 0; i < 8; i++)
      {
        ChessBoard[i][1].ChessPieceName = Resources.BlackPawn;
        ChessBoard[i][1].IsWhite = false;
        ChessBoard[i][1].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_pawn.png";

        ChessBoard[i][6].ChessPieceName = Resources.WhitePawn;
        ChessBoard[i][6].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_pawn.png";
      }

      ChessBoard[0][0].ChessPieceName = ChessBoard[7][0].ChessPieceName = Resources.BlackRook;
      ChessBoard[0][0].IsWhite = ChessBoard[7][0].IsWhite = false;
      ChessBoard[0][0].ChessPiece = ChessBoard[7][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_rook.png";
      ChessBoard[0][7].ChessPieceName = ChessBoard[7][7].ChessPieceName = Resources.WhiteRook;
      ChessBoard[0][7].ChessPiece = ChessBoard[7][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_rook.png";

      ChessBoard[1][0].ChessPieceName = ChessBoard[6][0].ChessPieceName = Resources.BlackHorse;
      ChessBoard[1][0].IsWhite = ChessBoard[6][0].IsWhite = false;
      ChessBoard[1][0].ChessPiece = ChessBoard[6][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_horse.png";
      ChessBoard[1][7].ChessPieceName = ChessBoard[6][7].ChessPieceName = Resources.WhiteHorse;
      ChessBoard[1][7].ChessPiece = ChessBoard[6][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_horse.png";

      ChessBoard[2][0].ChessPieceName = ChessBoard[5][0].ChessPieceName = Resources.BlackBishop;
      ChessBoard[2][0].IsWhite = ChessBoard[5][0].IsWhite = false;
      ChessBoard[2][0].ChessPiece = ChessBoard[5][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_bishop.png";
      ChessBoard[2][7].ChessPieceName = ChessBoard[5][7].ChessPieceName = Resources.WhiteBishop;
      ChessBoard[2][7].ChessPiece = ChessBoard[5][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_bishop.png";


      ChessBoard[3][0].ChessPieceName = Resources.BlackQueen;
      ChessBoard[3][0].IsWhite = false;
      ChessBoard[3][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_queen.png";
      ChessBoard[3][7].ChessPieceName = Resources.WhiteQueen;
      ChessBoard[3][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_queen.png";

      ChessBoard[4][0].ChessPieceName = Resources.BlackKing;
      ChessBoard[4][0].IsWhite = false;
      ChessBoard[4][0].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/black_king.png";
      ChessBoard[4][7].ChessPieceName = Resources.WhiteKing;
      ChessBoard[4][7].ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_king.png";

      SelectedSquare = ChessBoard.FirstOrDefault().FirstOrDefault();

    }

    public void DisplayAvailableSquares(String squareId)
    {

      // clear background of the selected item
      if (SelectedPiece != null)
      {
        int sum = SelectedPiece.Id[0] + SelectedPiece.Id[1];
        if (sum % 2 != 0)
        {
          SelectedPiece.Background = White;
        }
        else
        {
          SelectedPiece.Background = Black;
        }

        if (SelectedPiece.Id == squareId && SelectedPiece.Background == FocusedItemBackground)
        {
          return;
        }
      }

      var previousSelectedPiece = SelectedPiece;
      SelectedPiece = GetSelectedPiece(squareId);

      bool isPieceMoved = false;
      if (SelectedPiece.ChessPiece == availableSquare || selectedPiece.Background == CapturePieceBackground)
      {
        isPieceMoved = true;
      }

      if (previousSelectedPiece != null)
      {
        // clear background of the selected item
        int sum = previousSelectedPiece.Id[0] + previousSelectedPiece.Id[1];
        if (sum % 2 != 0)
        {
          previousSelectedPiece.Background = White;
        }
        else
        {
          previousSelectedPiece.Background = Black;
        }


        // clear previous moves
        if (currentMoves != null)
        {
          foreach (var move in currentMoves)
          {
            if (move.ChessPieceName == Resources.NoPiece)
              move.ChessPiece = null;
            sum = move.Id[0] + move.Id[1];
            if (sum % 2 != 0)
            {
              move.Background = White;
            }
            else
            {
              move.Background = Black;
            }
          }
        }
      }


      if (isPieceMoved)
      {
        SelectedPiece.ChessPiece = previousSelectedPiece.ChessPiece;
        SelectedPiece.ChessPieceName = previousSelectedPiece.ChessPieceName;
        selectedPiece.IsWhite = previousSelectedPiece.IsWhite;
        previousSelectedPiece.ChessPieceName = Resources.NoPiece;
        previousSelectedPiece.ChessPiece = null;

        if (SelectedPiece.ChessPieceName == Resources.WhitePawn && SelectedPiece.Id[1] == '8')
        {
          SelectedPiece.ChessPieceName = Resources.WhiteQueen;
          selectedPiece.ChessPiece = @"pack://application:,,,/ChessGame;component/Resources/ChessPieces/white_queen.png";

        }
        isPieceMoved = true;
      }



      if (isPieceMoved)
      {
        return;
      }

      if (SelectedPiece.ChessPieceName != Resources.NoPiece)
      {
        SelectedPiece.Background = FocusedItemBackground;
      }


      if (SelectedPiece.ChessPieceName == Resources.WhitePawn || SelectedPiece.ChessPieceName == Resources.BlackPawn)
      {
        currentMoves = MarkAvailablePawnMoves(squareId);
        foreach (var move in currentMoves)
        {
          if (move.ChessPieceName == Resources.NoPiece)
          {
            move.ChessPiece = availableSquare;
          }
          else
          {
            move.Background = CapturePieceBackground;
          }
        }
      }

      if (SelectedPiece.ChessPieceName == Resources.WhiteHorse || SelectedPiece.ChessPieceName == Resources.BlackHorse)
      {
        currentMoves = MarkAvailableHorseMoves(squareId);
        foreach (var move in currentMoves)
        {
          if (move.ChessPieceName == Resources.NoPiece)
          {
            move.ChessPiece = availableSquare;
          }
          else
          {
            move.Background = CapturePieceBackground;
          }
        }
      }

      if (SelectedPiece.ChessPieceName == Resources.WhiteRook || SelectedPiece.ChessPieceName == Resources.BlackRook)
      {
        currentMoves = MarkAvailableRookMoves(squareId);
        foreach (var move in currentMoves)
        {
          if (move.ChessPieceName == Resources.NoPiece)
          {
            move.ChessPiece = availableSquare;
          }
          else
          {
            move.Background = CapturePieceBackground;
          }
        }
      }


      if (SelectedPiece.ChessPieceName == Resources.WhiteBishop || SelectedPiece.ChessPieceName == Resources.BlackBishop)
      {
        currentMoves = MarkAvailableBishopMoves(squareId);
        foreach (var move in currentMoves)
        {
          if (move.ChessPieceName == Resources.NoPiece)
          {
            move.ChessPiece = availableSquare;
          }
          else
          {
            move.Background = CapturePieceBackground;
          }
        }
      }


      if (SelectedPiece.ChessPieceName == Resources.WhiteQueen || SelectedPiece.ChessPieceName == Resources.BlackQueen)
      {
        currentMoves = MarkAvailableBishopMoves(squareId);
        var currentMoves2 = MarkAvailableRookMoves(squareId);
        currentMoves.AddRange(currentMoves2);
        foreach (var move in currentMoves)
        {
          if (move.ChessPieceName == Resources.NoPiece)
          {
            move.ChessPiece = availableSquare;
          }
          else
          {
            move.Background = CapturePieceBackground;
          }
        }
      }

      if (SelectedPiece.ChessPieceName == Resources.WhiteKing || SelectedPiece.ChessPieceName == Resources.BlackKing)
      {
        currentMoves = MarkAvailableKingMoves(squareId);
        foreach (var move in currentMoves)
        {
          if (move.ChessPieceName == Resources.NoPiece)
          {
            move.ChessPiece = availableSquare;
          }
          else
          {
            move.Background = CapturePieceBackground;
          }
        }
      }

    }




    private Square GetSelectedPiece(string squareId)
    {
      Square square = null;
      foreach (var line in ChessBoard)
      {
        square = line.Where(p => p.Id.Contains(squareId)).FirstOrDefault();
        if (square != null)
        {
          return square;
        }
      }
      return square;
    }

    private List<Square> MarkAvailablePawnMoves(string squareId)
    {
      List<Square> pawnMoves = new List<Square>();

      List<string> directions = new List<string>();

      char letter = squareId[0];
      char digit = squareId[1];

      int x = letter - 'A';
      int y = 7 - (digit - '1');

      int offset = 1;
      if (SelectedPiece.IsWhite)
        offset = -1;


      int xOffset = x;
      int yOffset = y + offset;
      if (yOffset >= 0 && yOffset < 8 && chessBoard[xOffset][yOffset].ChessPieceName == Resources.NoPiece)
      {
        chessBoard[xOffset][yOffset].ChessPiece = availableSquare;
        pawnMoves.Add(chessBoard[xOffset][yOffset]);
      }

      if ((selectedPiece.IsWhite && digit == '2' || !selectedPiece.IsWhite && digit == '7') && chessBoard[xOffset][yOffset].ChessPieceName == Resources.NoPiece)
      {
        yOffset = y + 2 * offset;

        if (yOffset >= 0 && yOffset < 8 && chessBoard[xOffset][yOffset].ChessPieceName == Resources.NoPiece)
        {
          chessBoard[xOffset][yOffset].ChessPiece = availableSquare;
          pawnMoves.Add(chessBoard[xOffset][yOffset]);
        }
      }

      xOffset = x - 1;
      yOffset = y + offset;

      if (xOffset >= 0 && xOffset < 8 && yOffset >= 0 && yOffset < 8 && chessBoard[xOffset][yOffset].ChessPieceName != Resources.NoPiece && chessBoard[xOffset][yOffset].IsWhite != SelectedPiece.IsWhite)
      {
        pawnMoves.Add(chessBoard[xOffset][yOffset]);
      }


      xOffset = x + 1;
      yOffset = y + offset;

      if (xOffset >= 0 && xOffset < 8 && yOffset >= 0 && yOffset < 8 && chessBoard[xOffset][yOffset].ChessPieceName != Resources.NoPiece && chessBoard[xOffset][yOffset].IsWhite != SelectedPiece.IsWhite)
      {
        pawnMoves.Add(chessBoard[xOffset][yOffset]);
      }

      return pawnMoves;
    }

    private List<Square> MarkAvailableHorseMoves(string squareId)
    {
      List<Square> horseMoves = new List<Square>();

      List<int> lin = new List<int>() { 2, -2, 2, -2, 1, -1, 1, -1 };
      List<int> col = new List<int>() { 1, 1, -1, -1, 2, 2, -2, -2 };

      List<string> directions = new List<string>();

      for (int i = 0; i < 8; i++)
      {

        char letter = squareId[0];
        letter += (char)lin[i];
        char digit = squareId[1];
        digit += (char)col[i];
        directions.Add(letter.ToString() + digit.ToString());

      }

      foreach (var line in ChessBoard)
      {
        foreach (var square in line)
        {
          if (directions.Contains(square.Id))
          {
            if (square.ChessPieceName == Resources.NoPiece || square.IsWhite != SelectedPiece.IsWhite)
            {
              if (square.ChessPieceName == Resources.NoPiece)
                square.ChessPiece = availableSquare;
              horseMoves.Add(square);
            }
          }
        }

      }
      return horseMoves;
    }

    private List<Square> MarkAvailableRookMoves(string squareId)
    {
      List<Square> rookMoves = new List<Square>();

      List<string> locations = new List<string>();

      char letter = squareId[0];
      char digit = squareId[1]; // 
      int i = letter - 'A'; // A [0]   B[1] etc..    [0][0] A7  [0][1] A6
                            // int j = '8'- digit;

      int j = digit - '1';
      //down
      for (j = 8 - (digit - '1'); j < 8; j++)
      {
        var h = chessBoard[i][j].Id;
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
        {
          break;
        }
        string location = chessBoard[i][j].Id;
        locations.Add(location);
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
        {
          break;
        }

      }

      //up
      for (j = 8 - (digit - '1') - 2; j >= 0; j--)
      {
        var h = chessBoard[i][j].Id;
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
        {
          break;
        }
        string location = chessBoard[i][j].Id;
        locations.Add(location);
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
        {
          break;
        }

      }


      j = 8 - (digit - '1') - 1;
      //left
      for (i = (letter - 'A') - 1; i >= 0; i--)
      {
        var h = chessBoard[i][j].Id;
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
        {
          break;
        }
        string location = chessBoard[i][j].Id;
        locations.Add(location);
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
        {
          break;
        }

      }

      //right
      for (i = (letter - 'A') + 1; i < 8; i++)
      {
        var h = chessBoard[i][j].Id;
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
        {
          break;
        }
        string location = chessBoard[i][j].Id;
        locations.Add(location);
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
        {
          break;
        }

      }


      foreach (var line in ChessBoard)
      {
        foreach (var square in line)
        {
          if (locations.Contains(square.Id))
          {
            if (square.ChessPieceName == Resources.NoPiece || square.IsWhite != SelectedPiece.IsWhite)
            {
              if (square.ChessPieceName == Resources.NoPiece)
                square.ChessPiece = availableSquare;
              rookMoves.Add(square);
            }
          }
        }

      }
      return rookMoves;
    }



    private List<Square> MarkAvailableBishopMoves(string squareId)
    {
      List<Square> bishopMoves = new List<Square>();

      List<string> locations = new List<string>();

      char letter = squareId[0];
      char digit = squareId[1]; // 
      int I = letter - 'A'; // A [0]   B[1] etc..    [0][0] A7  [0][1] A6
                            // int j = '8'- digit;

      int J = 8 - (digit - '1') - 1;
      //right down
      for (int k = 1; I + k < 8 && J + k < 8; k++)
      {
        int i = I + k;
        int j = J + k;
        var h = chessBoard[i][j].Id;
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
        {
          break;
        }
        string location = chessBoard[i][j].Id;
        locations.Add(location);
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
        {
          break;
        }

      }

      //left down
      for (int k = 1; I - k >= 0 && J + k < 8; k++)
      {
        int i = I - k;
        int j = J + k;
        var h = chessBoard[i][j].Id;
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
        {
          break;
        }
        string location = chessBoard[i][j].Id;
        locations.Add(location);
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
        {
          break;
        }

      }


      //right up
      for (int k = 1; I + k < 8 && J - k > 0; k++)
      {
        int i = I + k;
        int j = J - k;
        var h = chessBoard[i][j].Id;
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
        {
          break;
        }
        string location = chessBoard[i][j].Id;
        locations.Add(location);
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
        {
          break;
        }

      }


      //left up
      for (int k = 1; I - k >= 0 && J - k > 0; k++)
      {
        int i = I - k;
        int j = J - k;
        var h = chessBoard[i][j].Id;
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
        {
          break;
        }
        string location = chessBoard[i][j].Id;
        locations.Add(location);
        if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
        {
          break;
        }

      }



      ////up
      //for (j = 8 - (digit - '1') - 2; j >= 0; j--)
      //{
      //  var h = chessBoard[i][j].Id;
      //  if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
      //  {
      //    break;
      //  }
      //  string location = chessBoard[i][j].Id;
      //  locations.Add(location);
      //  if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
      //  {
      //    break;
      //  }

      //}


      //j = 8 - (digit - '1') - 1;
      ////left
      //for (i = (letter - 'A') - 1; i >= 0; i--)
      //{
      //  var h = chessBoard[i][j].Id;
      //  if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
      //  {
      //    break;
      //  }
      //  string location = chessBoard[i][j].Id;
      //  locations.Add(location);
      //  if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
      //  {
      //    break;
      //  }

      //}

      ////right
      //for (i = (letter - 'A') + 1; i < 8; i++)
      //{
      //  var h = chessBoard[i][j].Id;
      //  if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite == SelectedPiece.IsWhite)
      //  {
      //    break;
      //  }
      //  string location = chessBoard[i][j].Id;
      //  locations.Add(location);
      //  if (chessBoard[i][j].ChessPieceName != Resources.NoPiece && chessBoard[i][j].IsWhite != SelectedPiece.IsWhite)
      //  {
      //    break;
      //  }

      //}


      foreach (var line in ChessBoard)
      {
        foreach (var square in line)
        {
          if (locations.Contains(square.Id))
          {
            if (square.ChessPieceName == Resources.NoPiece || square.IsWhite != SelectedPiece.IsWhite)
            {
              if (square.ChessPieceName == Resources.NoPiece)
                square.ChessPiece = availableSquare;
              bishopMoves.Add(square);
            }
          }
        }

      }
      return bishopMoves;
    }


    private List<Square> MarkAvailableKingMoves(string squareId)
    {
      List<Square> horseMoves = new List<Square>();

      List<int> lin = new List<int>() { 0, 0, 1, 1, 1, -1, -1, -1 };
      List<int> col = new List<int>() { 1, -1, 0, 1, -1, 0, 1, -1 };

      List<string> directions = new List<string>();

      for (int i = 0; i < 8; i++)
      {

        char letter = squareId[0];
        letter += (char)lin[i];
        char digit = squareId[1];
        digit += (char)col[i];
        directions.Add(letter.ToString() + digit.ToString());
      }

      foreach (var line in ChessBoard)
      {
        foreach (var square in line)
        {
          if (directions.Contains(square.Id))
          {
            if (square.ChessPieceName == Resources.NoPiece || square.IsWhite != SelectedPiece.IsWhite)
            {
              if (square.ChessPieceName == Resources.NoPiece)
                square.ChessPiece = availableSquare;
              horseMoves.Add(square);
            }
          }
        }

      }
      return horseMoves;
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}

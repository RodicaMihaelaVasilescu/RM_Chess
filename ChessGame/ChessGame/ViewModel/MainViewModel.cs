using ChessGame.Mapper;
using ChessGame.Model;
using ChessGame.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
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
    List<Square> markedAsAvailableSquares = new List<Square>();
    SolidColorBrush WhiteBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f0d9b5")); // white
    SolidColorBrush BlackBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#b58863")); // black
    SolidColorBrush FocusedItemBackground = Brushes.Olive;
    SolidColorBrush CapturePieceBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cdd26a"));
    private string availableSquareIcon = @"pack://application:,,,/ChessGame;component/Resources/available_square.png";
    //private string availablePieceIcon = @"pack://application:,,,/ChessGame;component/Resources/available_piece.png";
    private ObservableCollection<string> listOfMvements = new ObservableCollection<string>();
    public Dictionary<string, string> Movements = new Dictionary<string, string>();

    ObservableCollection<ObservableCollection<ObservableCollection<Square>>> listOfChessBoards = new ObservableCollection<ObservableCollection<ObservableCollection<Square>>>();



    public ObservableCollection<string> ListOfMovements
    {
      get { return listOfMvements; }
      set
      {
        if (listOfMvements == value) return;
        listOfMvements = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ListOfMovements"));
      }
    }

    public String SelectedMovement
    {
      get { return selectedMovement; }
      set
      {
        if (selectedMovement == value) return;
        selectedMovement = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMovement"));
      }
    }


    public int SelectedMovementIndex
    {
      get { return selectedMovementIndex; }
      set
      {
        if (selectedMovementIndex == value) return;
        selectedMovementIndex = value;
        //  ChessBoard = new ObservableCollection<ObservableCollection<Square>>();
        //ChessBoard = listOfChessBoards[value];
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMovementIndex"));
      }
    }



    ChessPieceLocation mapper = new ChessPieceLocation();

    public Boolean IsWhiteTurn = true;
    public Boolean isFirstPlayerWhite = true;

    private string message;

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

    //public Square SelectedSquare
    //{
    //  get { return selectedSquare; }
    //  set
    //  {
    //    if (selectedSquare == value) return;
    //    selectedSquare = value;
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedSquare"));
    //  }
    //}

    public String Message
    {
      get { return message; }
      set
      {
        if (message == value) return;
        message = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
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
    public MainViewModel()
    {

    }

    List<ChessBoardModel> ChessBoardConfigurations = new List<ChessBoardModel>();

    public void InitializeChessBoard()
    {
      ChessBoard = GetEmptyChessBoard();
      PlaceChessPieces();
    }


    ObservableCollection<ObservableCollection<Square>> GetEmptyChessBoard()
    {
      var ClearChessBoard = new ObservableCollection<ObservableCollection<Square>>();


      for (int i = 0; i < 8; i++)
      {
        var lineOfSquares = new ObservableCollection<Square>();
        for (int j = 0; j < 8; j++)
        {
          Square square = new Square();
          var coordinates = new Pair(i, j);
          square.Id = mapper.CoordinatesToString[coordinates];
          square.ChessPieceName = Resources.NoPiece;
          lineOfSquares.Add(square);
          if ((coordinates.i + coordinates.j) % 2 == 0)
          {
            square.Background = WhiteBackground;
          }
          else
          {
            square.Background = BlackBackground; // black
          }
        }
        ClearChessBoard.Add(lineOfSquares);
      }

      listOfChessBoards.Add(ClearChessBoard);
      return ClearChessBoard;
    }


    List<ChessPiece> pieces = new List<ChessPiece>();
    private string selectedMovement;
    private int selectedMovementIndex;

    private void PlaceChessPieces()
    {
      pieces = new List<ChessPiece>();
      // add pawns
      for (int i = 0; i < 8; i++)
      {
        pieces.Add(new Pawn(isFirstPlayerWhite, ((char)('A' + i)).ToString() + "2"));
        pieces.Add(new Pawn(!isFirstPlayerWhite, ((char)('A' + i)).ToString() + "7"));
      }

      //add rooks
      pieces.Add(new Rook(isFirstPlayerWhite, "A1"));
      pieces.Add(new Rook(isFirstPlayerWhite, "H1"));
      pieces.Add(new Rook(!isFirstPlayerWhite, "A8"));
      pieces.Add(new Rook(!isFirstPlayerWhite, "H8"));

      //add knights
      pieces.Add(new Knight(isFirstPlayerWhite, "B1"));
      pieces.Add(new Knight(isFirstPlayerWhite, "G1"));
      pieces.Add(new Knight(!isFirstPlayerWhite, "B8"));
      pieces.Add(new Knight(!isFirstPlayerWhite, "G8"));

      //add bishops
      pieces.Add(new Bishop(isFirstPlayerWhite, "C1"));
      pieces.Add(new Bishop(isFirstPlayerWhite, "F1"));
      pieces.Add(new Bishop(!isFirstPlayerWhite, "C8"));
      pieces.Add(new Bishop(!isFirstPlayerWhite, "F8"));


      //add queens
      pieces.Add(new Queen(isFirstPlayerWhite, isFirstPlayerWhite ? "D1" : "E1"));
      pieces.Add(new Queen(!isFirstPlayerWhite, isFirstPlayerWhite ? "D8" : "E8"));

      //add kings
      pieces.Add(new King(isFirstPlayerWhite, isFirstPlayerWhite ? "E1" : "D1"));
      pieces.Add(new King(!isFirstPlayerWhite, isFirstPlayerWhite ? "E8" : "D8"));

      // place pieces on chessboard
      foreach (var piece in pieces)
      {
        chessBoard[piece.Coordinates.i][piece.Coordinates.j].ChessPieceName = piece.ChessPieceName;
        chessBoard[piece.Coordinates.i][piece.Coordinates.j].Piece = piece;
        //chessBoard[piece.Coordinates.i][piece.Coordinates.j].ChessPieceName = piece.ChessPieceName;
        //chessBoard[piece.Coordinates.i][piece.Coordinates.j].Piece.IsWhite = piece.IsWhite;
        //chessBoard[piece.Coordinates.i][piece.Coordinates.j].Piece.Location = piece.Location;
        //chessBoard[piece.Coordinates.i][piece.Coordinates.j].Piece.ChessPieceIcon = piece.ChessPieceIcon;
      }

      //SelectedSquare = ChessBoard.FirstOrDefault().FirstOrDefault();
    }


    public void DisplayAvailableSquares(String squareId)
    {
      var previousSelectedPiece = SelectedSquare;
      SelectedSquare = GetSelectedPiece(squareId);

      if (SelectedSquare == null)
      {
        return;
      }
      if (previousSelectedPiece != null && previousSelectedPiece.Background != null)
      {
        var c = mapper.StringToCoordinates[previousSelectedPiece.Id];
        chessBoard[c.i][c.j].Background = (c.i + c.j) % 2 == 0 ? WhiteBackground : BlackBackground;
      }


      if (previousSelectedPiece != null && SelectedSquare != null)
      {
        if (previousSelectedPiece.Piece.GetType().ToString().Contains("King") && SelectedSquare.Piece.GetType().ToString().Contains("Rook") && SelectedSquare.Piece.IsWhite == previousSelectedPiece.Piece.IsWhite)
        {
          if (!Movements.ContainsValue(previousSelectedPiece.Id) && !Movements.ContainsValue(previousSelectedPiece.Id))
          {
            int offset = 1;
            if (SelectedSquare.Piece.Location[0] > previousSelectedPiece.Id[0])
            {
              offset = -1;
            }
            bool canMakeRocada = true;
            for (int i = SelectedSquare.Piece.Location[0] + offset; i != previousSelectedPiece.Id[0]; i += offset)
            {
              string s = ((char)i).ToString() + previousSelectedPiece.Id[1].ToString();
              if (pieces.Any(p => p.Location == s))
              {
                canMakeRocada = false;
              }
            }
            if (canMakeRocada)
            {
              ClearAvailableSquares();

              var c = mapper.StringToCoordinates[SelectedSquare.Piece.Location];
              var rookTemp = chessBoard[c.i][c.j].Piece;
              var rookIdTemp = rookTemp.Location;
              chessBoard[c.i][c.j].Piece = new ChessPiece();
              string rook = ((char)(previousSelectedPiece.Piece.Location[0] - offset)).ToString() + previousSelectedPiece.Piece.Location[1].ToString();


              rookTemp.Location = rook;
              var piece = pieces.FirstOrDefault(p => p.Location == null && p.IsWhite == rookTemp.IsWhite);
              piece = rookTemp;
              c = mapper.StringToCoordinates[rook];
              chessBoard[c.i][c.j].Piece = rookTemp;


              c = mapper.StringToCoordinates[previousSelectedPiece.Piece.Location];
              var kingTemp = chessBoard[c.i][c.j].Piece;
              var kingIdTemp = kingTemp.Location;
              chessBoard[c.i][c.j].Piece = new ChessPiece();

              string king = ((char)(kingIdTemp[0] - 2 * offset)).ToString() + kingIdTemp[1].ToString();


              kingTemp.Location = king;
              piece = pieces.FirstOrDefault(p => p.GetType().ToString().Contains("King") && p.IsWhite == kingTemp.IsWhite);
              piece = kingTemp;
              c = mapper.StringToCoordinates[king];
              chessBoard[c.i][c.j].Piece = kingTemp;

              IsWhiteTurn = !IsWhiteTurn;
              return;

            }
          }
        }
      }


      // if is moved
      if (markedAsAvailableSquares.Contains(SelectedSquare))
      {
        MovePiece(previousSelectedPiece);
        return;
      }

      //clear
      ClearAvailableSquares();

      // if the same piece is selected
      if (previousSelectedPiece == SelectedSquare)
      {
        SelectedSquare = new Square();
        return;
      }

      var selected = pieces.FirstOrDefault(p => p.Location == SelectedSquare.Id);
      if (selected != null && SelectedSquare.Piece.IsWhite == IsWhiteTurn)
      {

        markedAsAvailableSquares = selected.GetAvailableMoves(SelectedSquare, pieces, chessBoard, Movements);
        if (markedAsAvailableSquares.Any())
        {
          SelectedSquare.Background = FocusedItemBackground;
        }
        // mark available squares
        foreach (var currentSquare in markedAsAvailableSquares)
        {
          if (currentSquare.Piece.ChessPieceName == null)
          {
            var c = mapper.StringToCoordinates[currentSquare.Id];
            chessBoard[c.i][c.j].Piece.ChessPieceIcon = availableSquareIcon;
          }
          else
          {
            var c = mapper.StringToCoordinates[currentSquare.Id];
            chessBoard[c.i][c.j].Background = CapturePieceBackground;
          }
        }
        return;
      }




    }

    private void MovePiece(Square previousSelectedPiece)
    {
      ClearAvailableSquares();

      //Movements[previousSelectedPiece.Id] = SelectedSquare.Id;
      if (SelectedSquare.Piece.ChessPieceName != null)
      {
        pieces.RemoveAll(p => p.Location == SelectedSquare.Id);
      }
      SelectedSquare.Piece = previousSelectedPiece.Piece;

      var piece = pieces.FirstOrDefault(p => p.Location == previousSelectedPiece.Id);
      piece.Location = SelectedSquare.Id;

      previousSelectedPiece.Piece = new ChessPiece();
      var c = mapper.StringToCoordinates[SelectedSquare.Id];
      chessBoard[c.i][c.j].Background = (c.i + c.j) % 2 == 0 ? WhiteBackground : BlackBackground;

      IsWhiteTurn = !IsWhiteTurn;
    }

    void ClearAvailableSquares()
    {

      foreach (var currentSquare in markedAsAvailableSquares)
      {
        if (currentSquare.Piece.ChessPieceName == null)
        {
          var c = mapper.StringToCoordinates[currentSquare.Id];
          chessBoard[c.i][c.j].Piece = new ChessPiece();
        }
        else
        {
          var c = mapper.StringToCoordinates[currentSquare.Id];
          chessBoard[c.i][c.j].Background = (c.i + c.j) % 2 == 0 ? WhiteBackground : BlackBackground;
        }
      }
      markedAsAvailableSquares.Clear();
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




    public event PropertyChangedEventHandler PropertyChanged;
  }
}

using ChessGame.Command;
using ChessGame.Mapper;
using ChessGame.Model;
using ChessGame.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace ChessGame.ViewModel
{
  class MainViewModel : INotifyPropertyChanged
  {
    private ObservableCollection<ObservableCollection<Square>> chessBoard;
    private string availableSquareIcon = @"pack://application:,,,/ChessGame;component/Resources/available_square.png";
    List<Square> markedAsAvailableSquares = new List<Square>();

    SolidColorBrush WhiteBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f0d9b5")); // white
    SolidColorBrush BlackBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#b58863")); // black
    SolidColorBrush FocusedItemBackground = Brushes.Olive;
    SolidColorBrush ChessBackground = Brushes.DarkRed;
    SolidColorBrush CapturePieceBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cdd26a"));
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

    public string SelectedMovement
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
    public Square SelectedSquare;

    public ICommand ResetCommand { get; set; }
    public MainViewModel()
    {
      ResetCommand = new RelayCommand(ResetCommandExecute);

    }

    private void ResetCommandExecute()
    {
      Movements.Clear();
      IsWhiteTurn = true;
      isFirstPlayerWhite = !isFirstPlayerWhite;
      pieces.Clear();
      ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);
      InitializeChessBoard();

    }

    public void InitializeChessBoard()
    {
      ChessBoard = GetEmptyChessBoard();
      pieces = PlaceChessPieces(chessBoard);
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
    private List<ChessPiece> PlaceChessPieces(ObservableCollection<ObservableCollection<Square>> chessboard, List<ChessPiece> pieces = null)
    {
      var piecesList = new List<ChessPiece>();
      if (pieces == null)
      {


        // add pawns
        for (int i = 0; i < 8; i++)
        {
          piecesList.Add(new Pawn(isFirstPlayerWhite, ((char)('A' + i)).ToString() + "2"));
          piecesList.Add(new Pawn(!isFirstPlayerWhite, ((char)('A' + i)).ToString() + "7"));
        }

        //add rooks
        piecesList.Add(new Rook(isFirstPlayerWhite, "A1"));
        piecesList.Add(new Rook(isFirstPlayerWhite, "H1"));
        piecesList.Add(new Rook(!isFirstPlayerWhite, "A8"));
        piecesList.Add(new Rook(!isFirstPlayerWhite, "H8"));

        //add knights
        piecesList.Add(new Knight(isFirstPlayerWhite, "B1"));
        piecesList.Add(new Knight(isFirstPlayerWhite, "G1"));
        piecesList.Add(new Knight(!isFirstPlayerWhite, "B8"));
        piecesList.Add(new Knight(!isFirstPlayerWhite, "G8"));

        //add bishops
        piecesList.Add(new Bishop(isFirstPlayerWhite, "C1"));
        piecesList.Add(new Bishop(isFirstPlayerWhite, "F1"));
        piecesList.Add(new Bishop(!isFirstPlayerWhite, "C8"));
        piecesList.Add(new Bishop(!isFirstPlayerWhite, "F8"));


        //add queens
        piecesList.Add(new Queen(isFirstPlayerWhite, isFirstPlayerWhite ? "D1" : "E1"));
        piecesList.Add(new Queen(!isFirstPlayerWhite, isFirstPlayerWhite ? "D8" : "E8"));

        //add kings
        piecesList.Add(new King(isFirstPlayerWhite, isFirstPlayerWhite ? "E1" : "D1"));
        piecesList.Add(new King(!isFirstPlayerWhite, isFirstPlayerWhite ? "E8" : "D8"));
      }
      else
      {
        piecesList = pieces;
      }

      // place pieces on chessboard
      foreach (var piece in piecesList)
      {
        var c = mapper.StringToCoordinates[piece.Location];
        chessboard[c.i][c.j].ChessPieceName = piece.ChessPieceName;
        chessboard[c.i][c.j].Piece = piece;
      }
      return piecesList;
    }

    Square ChessState = null;
    public void DisplayAvailableSquares(String squareId)
    {
      var previousSelectedPiece = SelectedSquare;
      SelectedSquare = GetSelectedPiece(squareId, ChessBoard);

      if (SelectedSquare == null)
      {
        return;
      }

      if (previousSelectedPiece != null && previousSelectedPiece.Background != null)
      {
        var c = mapper.StringToCoordinates[previousSelectedPiece.Id];
        chessBoard[c.i][c.j].Background = (c.i + c.j) % 2 == 0 ? WhiteBackground : BlackBackground;

      }


      Castle(previousSelectedPiece);


      // if is moved
      if (markedAsAvailableSquares.Contains(SelectedSquare))
      {
        if (MovePiece2(SelectedSquare, previousSelectedPiece, ChessBoard, pieces, markedAsAvailableSquares))
        {
          Movements[SelectedSquare.Id] = previousSelectedPiece.Id;
          IsWhiteTurn = !IsWhiteTurn;
        }
        else
        {
          // is in chess state
          if (ChessState != null && ChessState.Piece != null)
          {
            IsWhiteTurn = ChessState.Piece.IsWhite;
            ChessState.Background = ChessBackground;
          }

        }
        //IsInChessState();
        return;
      }

      //clear
      ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);

      // if the same piece is selected
      if (previousSelectedPiece == SelectedSquare)
      {
        SelectedSquare = new Square();
        return;
      }

      var selected = pieces.FirstOrDefault(p => p.Location == SelectedSquare.Id);
      if (selected != null && SelectedSquare.Piece.IsWhite == IsWhiteTurn)
      {

        markedAsAvailableSquares = selected.GetAvailableMoves(SelectedSquare.Piece, pieces, chessBoard, Movements);

        var pieces2 = GetCopyOfPieces(pieces);
        var ChessBoard2 = GetEmptyChessBoard();
        pieces2 = PlaceChessPieces(ChessBoard2, pieces2);
        var SelectedSquare2 = GetSelectedPiece(SelectedSquare.Id, ChessBoard2);

        var availableSquares = selected.GetAvailableMoves(SelectedSquare2.Piece, pieces2, ChessBoard2, Movements);

        var toRemove = new List<Square>();
        foreach (var move in availableSquares)
        {
          var nextChessboardConfiguration = GetDuplicateOfChessBoard(ChessBoard);
          var copyPieces = GetCopyOfPieces(pieces);
          var copyAvailableSquares = GetCopyOfListOfSquare(availableSquares);
          var currentSquare = GetSelectedPiece(SelectedSquare.Id, nextChessboardConfiguration);
          var nextMove = copyAvailableSquares.Where(p => p.Id == move.Id).FirstOrDefault();

          if (!MovePiece2(nextMove, currentSquare, nextChessboardConfiguration, copyPieces, copyAvailableSquares))
          {
            if (ChessState != null && ChessState.Piece != null && SelectedSquare.Piece.IsWhite == ChessState.Piece.IsWhite)
              toRemove.Add(move);
          }
        }
        markedAsAvailableSquares.RemoveAll(p => toRemove.FirstOrDefault(t => t.Id == p.Id) != null);

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
      }
    }

    private List<Square> GetCopyOfListOfSquare(List<Square> markedAsAvailableSquares)
    {
      List<Square> pieces2 = new List<Square>();
      foreach (var square in markedAsAvailableSquares)
      {
        var newPiece = new Square(square);
        pieces2.Add(newPiece);
      }
      return pieces2;
    }

    private List<ChessPiece> GetCopyOfPieces(List<ChessPiece> pieces)
    {
      List<ChessPiece> pieces2 = new List<ChessPiece>();
      foreach (var piece in pieces)
      {
        var newPiece = new ChessPiece(piece);
        pieces2.Add(newPiece);
      }
      return pieces2;
    }

    private ObservableCollection<ObservableCollection<Square>> GetDuplicateOfChessBoard(ObservableCollection<ObservableCollection<Square>> chessBoard)
    {
      var duplicate = new ObservableCollection<ObservableCollection<Square>>();
      for (int i = 0; i < 8; i++)
      {
        var line = new ObservableCollection<Square>();
        for (int j = 0; j < 8; j++)
        {
          Square sq = new Square(chessBoard[i][j]);
          line.Add(sq);
        }
        duplicate.Add(line);
      }
      return duplicate;
    }

    private void Castle(Square previousSelectedPiece)
    {

      if (previousSelectedPiece != null && previousSelectedPiece.Piece.ChessPieceType != null && SelectedSquare != null && SelectedSquare.Piece.ChessPieceType != null)
      {
        if (previousSelectedPiece.Piece.ChessPieceType.Contains(Resources.King) && SelectedSquare.Piece.ChessPieceType.Contains(Resources.Rook) && SelectedSquare.Piece.IsWhite == previousSelectedPiece.Piece.IsWhite)
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
              ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);

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
              piece = pieces.FirstOrDefault(p => p.ChessPieceType.Contains(Resources.King) && p.IsWhite == kingTemp.IsWhite);
              piece = kingTemp;
              c = mapper.StringToCoordinates[king];
              chessBoard[c.i][c.j].Piece = kingTemp;

              IsWhiteTurn = !IsWhiteTurn;
              return;

            }
          }
        }
      }

    }

    private bool MovePiece1(Square currentSquare, Square previousSquare, ObservableCollection<ObservableCollection<Square>> ChessBoard, List<ChessPiece> pieces, List<Square> markedAsAvailableSquares)
    {
      ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);

      if (currentSquare != null)
      {
        //remove current piece
        pieces.RemoveAll(p => p.Location == currentSquare.Id);
      }

      //switch pieces and update location
      var piece = pieces.FirstOrDefault(p => p.Location == previousSquare.Id);
      piece.Location = currentSquare.Id;
      currentSquare.Piece = piece;
      previousSquare.Piece = new ChessPiece();
      var c = mapper.StringToCoordinates[currentSquare.Id];
      ChessBoard[c.i][c.j].Background = (c.i + c.j) % 2 == 0 ? WhiteBackground : BlackBackground;


      if (currentSquare.Piece.ChessPieceType.Contains(Resources.Pawn))
      {
        if (currentSquare.Piece.Location[1] == '1' || currentSquare.Piece.Location[1] == '8')
        {
          var temp = new ChessPiece();
          temp = piece;
          piece = new Queen(temp.IsWhite, temp.Location);

          pieces.RemoveAll(p => p.Location == currentSquare.Id);
          c = mapper.StringToCoordinates[piece.Location];
          ChessBoard[c.i][c.j].Piece = piece;
        }
      }

      if (IsInChessState1(ChessBoard, pieces))
      {
        return false;
      }
      return true;
    }

    private bool IsInChessState1(ObservableCollection<ObservableCollection<Square>> chessBoard, List<ChessPiece> pieces)
    {
      var movements = new List<Square>();

      foreach (var piece in pieces)
      {
        if (piece != null)
        {
          var x = piece.GetAvailableMoves(piece, pieces, chessBoard, Movements);
          movements.AddRange(piece.GetAvailableMoves(piece, pieces, chessBoard, Movements));
        }
      }



      // all pieces
      var kings = movements.Where(p => p.Piece != null && p.Piece.ChessPieceType != null && p.Piece.ChessPieceType.Contains(Resources.King)).ToList().Distinct();

      foreach (var king in kings)
      {
        ChessState = king;
        return true;
      }
      ChessState = null;
      return false;
    }

    private bool MovePiece2(Square currentSquare, Square previousSquare,
      ObservableCollection<ObservableCollection<Square>> ChessBoard, List<ChessPiece> pieces,
      List<Square> markedAsAvailableSquares)
    {
      ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);

      if (currentSquare != null)
      {
        //remove current piece
        pieces.RemoveAll(p => p.Location == currentSquare.Id);
      }

      //switch pieces and update location
      var piece = pieces.FirstOrDefault(p => p.Location == previousSquare.Id);
      piece.Location = currentSquare.Id;
      currentSquare.Piece = piece;
      previousSquare.Piece = new ChessPiece();
      var c = mapper.StringToCoordinates[currentSquare.Id];
      ChessBoard[c.i][c.j].Background = (c.i + c.j) % 2 == 0 ? WhiteBackground : BlackBackground;
      ChessBoard[c.i][c.j].Piece = piece;

      if (currentSquare.Piece.ChessPieceType.Contains(Resources.Pawn))
      {
        if (currentSquare.Piece.Location[1] == '1' || currentSquare.Piece.Location[1] == '8')
        {
          var temp = new ChessPiece();
          temp = piece;
          piece = new Queen(temp.IsWhite, temp.Location);

          pieces.RemoveAll(p => p.Location == currentSquare.Id);
          c = mapper.StringToCoordinates[piece.Location];
          ChessBoard[c.i][c.j].Piece = piece;
        }
      }

      if (IsInChessState2(ChessBoard, pieces, currentSquare.Piece))
      {
        return false;
      }
      return true;
    }

    private bool IsInChessState2(ObservableCollection<ObservableCollection<Square>> chessBoard, List<ChessPiece> pieces, ChessPiece currentPiece)
    {
      var movements = new List<Square>();

      foreach (var piece in pieces)
      {
        if (piece != null)
        {
          movements.AddRange(piece.GetAvailableMoves(piece, pieces, chessBoard, Movements));
        }
      }

      // all pieces
      var kings = movements.Where(p => p.Piece != null && p.Piece.ChessPieceType != null && p.Piece.ChessPieceType.Contains(Resources.King)).ToList().Distinct();

      foreach (var king in kings)
      {
        ChessState = king;
        return true;
      }
      ChessState = null;
      return false;

    }

    ObservableCollection<ObservableCollection<Square>> ClearAvailableSquares(ObservableCollection<ObservableCollection<Square>> chessBoard, List<Square> markedAsAvailableSquares)
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
      return chessBoard;
    }

    private Square GetSelectedPiece(string squareId, ObservableCollection<ObservableCollection<Square>> ChessBoard)
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

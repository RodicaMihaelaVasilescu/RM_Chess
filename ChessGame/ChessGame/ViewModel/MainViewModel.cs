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
    #region Private Properties

    private int selectedMovementIndex;
    private string message;
    private int noOfMovement = 0;
    private string selectedMovement;
    private string availableSquareIcon = @"pack://application:,,,/ChessGame;component/Resources/available_square.png";
    private Square CheckState = null;
    private ChessPieceLocation mapper = ChessPieceLocation.Instance;
    private ObservableCollection<ObservableCollection<Square>> chessBoard;
    private ObservableCollection<Movement> listOfMvements = new ObservableCollection<Movement>();
    private ObservableCollection<ObservableCollection<ObservableCollection<Square>>> listOfChessBoards = new ObservableCollection<ObservableCollection<ObservableCollection<Square>>>();
    private List<Square> markedAsAvailableSquares = new List<Square>();
    private List<ChessPiece> pieces = new List<ChessPiece>();
    private SolidColorBrush WhiteBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f0d9b5")); // white
    private SolidColorBrush BlackBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#b58863")); // black
    private SolidColorBrush FocusedItemBackground = Brushes.Olive;
    private SolidColorBrush CheckBackground = Brushes.DarkRed;
    private SolidColorBrush CapturePieceBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#cdd26a"));
    #endregion

    #region Public Properties

    public bool IsCheckmate { get; private set; } = false;
    public bool AddMoveSound { get; set; } = false;
    public bool IsPieceMoved { get; set; }
    public bool IsWhiteTurn = true;
    public bool IsPieceCaptured = false;
    public bool isFirstPlayerWhite = true;
    public Square SelectedSquare;
    public Dictionary<string, string> Movements = new Dictionary<string, string>();
    public ICommand ResetCommand { get; set; }
    public ObservableCollection<ObservableCollection<Square>> EmptyChessboard { get; }

    public event PropertyChangedEventHandler PropertyChanged;


    public ObservableCollection<Movement> ListOfMovements
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
        //IsWhiteTurn = value % 2 == 0 ? true : false;
        if (ChessBoard != null && ChessBoard.Count() >= value)
        {
          ChessBoard = listOfChessBoards[value];

        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMovementIndex"));
      }
    }

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


    #endregion

    #region Constructor
    public MainViewModel()
    {
      ResetCommand = new RelayCommand(ResetCommandExecute);
      EmptyChessboard = GetEmptyChessBoard();
    }

    #endregion

    #region Private Methods
    private void ResetCommandExecute()
    {
      Execute();
      Execute();
    }
    void Execute()
    {
      ChessBoard = EmptyChessboard;
      IsCheckmate = false;
      Movements.Clear();
      ListOfMovements.Clear();
      listOfChessBoards.Clear();
      IsWhiteTurn = true;
      isFirstPlayerWhite = !isFirstPlayerWhite;
      pieces.Clear();
      ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);
      InitializeChessBoard();
    }

    private ObservableCollection<ObservableCollection<Square>> GetEmptyChessBoard()
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
      return ClearChessBoard;
    }

    #endregion


    #region Public Methods
    public void InitializeChessBoard()
    {
      ChessBoard = EmptyChessboard;
      pieces = PlaceChessPieces(chessBoard);
      ListOfMovements.Add(new Movement("Chessboard",null, @"pack://application:,,,/ChessGame;component/Resources/chessboard.png"));

      listOfChessBoards.Add(GetCopyOfChessboard(ChessBoard));
    }


    public void DisplayAvailableSquares(String squareId)
    {
      IsPieceMoved = false;
      IsPieceCaptured = false;
      AddMoveSound = false;
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

      if (CheckState != null && CheckState.Piece != null)
      {
        IsWhiteTurn = CheckState.Piece.IsWhite;
      }
      else
      {
        var king = pieces.FirstOrDefault(k => k.ChessPieceType.Contains(Resources.King) && k.IsWhite == k.IsWhite);
        if (king != null)
        {
          var kingSquare = GetSelectedPiece(king.Location, ChessBoard);
          if (kingSquare != null)
          {
            var c = mapper.StringToCoordinates[kingSquare.Id];
            chessBoard[c.i][c.j].Background = (c.i + c.j) % 2 == 0 ? WhiteBackground : BlackBackground;
          }
        }
      }

      MakeCastle(previousSelectedPiece);

      if (markedAsAvailableSquares.Contains(SelectedSquare))
      {
        //an available move was selected
        AddMoveSound = true;
        // check if the move can be made
        if (CanPieceBeMoved(SelectedSquare, previousSelectedPiece, ChessBoard, pieces, markedAsAvailableSquares, true))
        {
          IsPieceMoved = true;
          AddMoveSound = true;
          Movements[SelectedSquare.Id] = previousSelectedPiece.Id;
          listOfChessBoards.Add(GetCopyOfChessboard(ChessBoard));
          noOfMovement++;
          ListOfMovements.Add(new Movement(string.Format("{0}: {1} {2}", noOfMovement.ToString(), SelectedSquare.Piece.IsWhite ? "White" : "Black",
            SelectedSquare.Piece.ChessPieceType), previousSelectedPiece.Id + "-"+SelectedSquare.Id, SelectedSquare.Piece.ChessPieceIcon));

          IsWhiteTurn = !IsWhiteTurn;
          if (CheckState != null)
          {
            CheckState.Background = CheckBackground;
          }
          CheckIfCheckmate(ChessBoard);
        }
        else
        {
          //is in chess state
          if (CheckState != null && CheckState.Piece != null)
          {
            IsWhiteTurn = CheckState.Piece.IsWhite;
            CheckState.Background = CheckBackground;
          }

        }
        return;
      }

      //clear
      ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);

      MarkAvailableSquares(previousSelectedPiece);
    }

    private void CheckIfCheckmate(ObservableCollection<ObservableCollection<Square>> chessBoard)
    {
      bool isCheckmate = true;
      foreach (var currentPiece in pieces)
      {
        if (currentPiece.IsWhite != SelectedSquare.Piece.IsWhite)
        {
          if (!IsThereAnyMoveAvailable(currentPiece))
          {
            isCheckmate = false;
          }
        }
      }
      if (isCheckmate)
      {
        var king = pieces.FirstOrDefault(k => k.ChessPieceType.Contains(Resources.King) && k.IsWhite != SelectedSquare.Piece.IsWhite);
        GetSelectedPiece(king.Location, chessBoard).Background = CheckBackground;
        ListOfMovements.Add(new Movement( string.Format("{0}: Checkmate! {1} wins!", ++noOfMovement, king.IsWhite ? "Black" : "White"), null, SelectedSquare.Piece.ChessPieceIcon));
        IsCheckmate = true;
      }
    }

    #endregion

    #region Private Methods

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
        chessboard[c.i][c.j].Piece = piece;
      }
      return piecesList;
    }

    private void MarkAvailableSquares(Square previousSelectedPiece)
    {
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
          var nextChessboardConfiguration = GetCopyOfChessboard(ChessBoard);
          var copyPieces = GetCopyOfPieces(pieces);
          var copyAvailableSquares = GetCopyOfListOfSquare(availableSquares);
          var currentSquare = GetSelectedPiece(SelectedSquare.Id, nextChessboardConfiguration);
          var nextMove = copyAvailableSquares.Where(p => p.Id == move.Id).FirstOrDefault();

          if (!CanPieceBeMoved(nextMove, currentSquare, nextChessboardConfiguration, copyPieces, copyAvailableSquares, false))
          {
            IsPieceMoved = false;
            if (CheckState != null && CheckState.Piece != null && SelectedSquare.Piece.IsWhite == CheckState.Piece.IsWhite)
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
          if (currentSquare.Piece.Location == null)
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

    private bool IsThereAnyMoveAvailable(ChessPiece selectedChessPiece)
    {
      var selected = pieces.FirstOrDefault(p => p.Location == selectedChessPiece.Location);
      if (selected != null && SelectedSquare.Piece.IsWhite != IsWhiteTurn)
      {

        var markedAsAvailableSquares = selected.GetAvailableMoves(selected, pieces, chessBoard, Movements);

        var pieces2 = GetCopyOfPieces(pieces);
        var ChessBoard2 = GetEmptyChessBoard();
        pieces2 = PlaceChessPieces(ChessBoard2, pieces2);
        var SelectedSquare2 = GetSelectedPiece(selected.Location, ChessBoard2);

        var availableSquares = selected.GetAvailableMoves(selected, pieces2, ChessBoard2, Movements);

        var toRemove = new List<Square>();
        foreach (var move in availableSquares)
        {
          var nextChessboardConfiguration = GetCopyOfChessboard(ChessBoard);
          var copyPieces = GetCopyOfPieces(pieces);
          var copyAvailableSquares = GetCopyOfListOfSquare(availableSquares);
          var currentSquare = GetSelectedPiece(selected.Location, nextChessboardConfiguration);
          var nextMove = copyAvailableSquares.Where(p => p.Id == move.Id).FirstOrDefault();

          if (!CanPieceBeMoved(nextMove, currentSquare, nextChessboardConfiguration, copyPieces, copyAvailableSquares, false))
          {
            IsPieceMoved = false;
            if (CheckState != null && CheckState.Piece != null && SelectedSquare.Piece.IsWhite != CheckState.Piece.IsWhite)
              toRemove.Add(move);
          }
        }
        markedAsAvailableSquares.RemoveAll(p => toRemove.FirstOrDefault(t => t.Id == p.Id) != null);

        if (markedAsAvailableSquares.Any())
        {
          return false;
        }
      }

      return true;
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
        var newPiece = Activator.CreateInstance(piece.GetType(), new object[] { piece.IsWhite, piece.Location });
        pieces2.Add((ChessPiece)newPiece);
      }
      return pieces2;
    }

    private ObservableCollection<ObservableCollection<Square>> GetCopyOfChessboard(ObservableCollection<ObservableCollection<Square>> chessBoard)
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

    private void MakeCastle(Square previousSelectedPiece)
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
            bool canMakeCastle = true;
            for (int i = SelectedSquare.Piece.Location[0] + offset; i != previousSelectedPiece.Id[0]; i += offset)
            {
              string s = ((char)i).ToString() + previousSelectedPiece.Id[1].ToString();
              if (pieces.Any(p => p.Location == s))
              {
                canMakeCastle = false;
              }
            }
            if (canMakeCastle)
            {
              noOfMovement++;
              ListOfMovements.Add(new Movement(string.Format("{0}: {1} Castle", noOfMovement, IsWhiteTurn ? "White: " : "Black: "), null, null));
              ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);
              IsPieceMoved = true;
              AddMoveSound = true;

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

    private bool CanPieceBeMoved(Square currentSquare, Square previousSquare,
      ObservableCollection<ObservableCollection<Square>> ChessBoard, List<ChessPiece> pieces,
      List<Square> markedAsAvailableSquares, bool isPieceMoved)
    {
      // clear green dots of available moves
      ClearAvailableSquares(ChessBoard, markedAsAvailableSquares);

      if (currentSquare != null)
      {
        // remove the pieces on the current square, in order to place the current piece
        pieces.RemoveAll(p => p.Location == currentSquare.Id);
      }

      //update the properties of the current piece
      var currentPiece = pieces.FirstOrDefault(p => p.Location == previousSquare.Id);
      currentPiece.Location = currentSquare.Id;

      // if the piece is moved and the current square location is not empty, the previous piece is capture
      if (currentSquare.Piece.Location != null && isPieceMoved)
      {
        IsPieceCaptured = true;
      }

      // update the current square properties
      currentSquare.Piece = currentPiece;
      previousSquare.Piece = new ChessPiece();
      var c = mapper.StringToCoordinates[currentSquare.Id];
      ChessBoard[c.i][c.j].Background = (c.i + c.j) % 2 == 0 ? WhiteBackground : BlackBackground;
      ChessBoard[c.i][c.j].Piece = currentPiece;

      // check if the current piece is a pawn promoted to queen
      if (currentSquare.Piece.ChessPieceType.Contains(Resources.Pawn) && isPieceMoved)
      {
        if (currentSquare.Piece.Location[1] == '1' || currentSquare.Piece.Location[1] == '8')
        {
          PromotePawnToQueen(currentPiece, currentSquare.Id);
        }
      }

      // check if the king is threatened
      if (IsInCheckState(ChessBoard, pieces, currentSquare.Piece.IsWhite, isPieceMoved))
      {
        return false;
      }
      return true;
    }

    private void PromotePawnToQueen(ChessPiece piece, string id)
    {
      var newQueen = new Queen(piece.IsWhite, piece.Location);

      pieces.RemoveAll(p => p.Location == id);
      pieces.Add(newQueen);
      var c = mapper.StringToCoordinates[newQueen.Location];
      ChessBoard[c.i][c.j].Piece = newQueen;
    }

    private bool IsInCheckState(ObservableCollection<ObservableCollection<Square>> chessBoard, List<ChessPiece> pieces, bool isWhite, bool isPieceMoved)
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
      var kingCurrentColor = movements.Where(p => p.Piece != null && p.Piece.ChessPieceType != null && p.Piece.ChessPieceType.Contains(Resources.King) && p.Piece.IsWhite == IsWhiteTurn).ToList().FirstOrDefault();
      var kingOppositeColor = movements.Where(p => p.Piece != null && p.Piece.ChessPieceType != null && p.Piece.ChessPieceType.Contains(Resources.King) && p.Piece.IsWhite == !IsWhiteTurn).ToList().FirstOrDefault();

      if (isPieceMoved && kingOppositeColor != null)
      {
        CheckState = kingOppositeColor;
        CheckState.Background = CheckBackground;
      }
      if (kingCurrentColor != null)
      {
        CheckState = kingCurrentColor;
        if (CheckState != null)
          CheckState.Background = CheckBackground;
        return true;
      }
      CheckState = null;

      return false;

    }

    private ObservableCollection<ObservableCollection<Square>> ClearAvailableSquares
      (ObservableCollection<ObservableCollection<Square>> chessBoard, List<Square> markedAsAvailableSquares)
    {

      foreach (var currentSquare in markedAsAvailableSquares)
      {
        if (currentSquare.Piece.Location == null)
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

    #endregion
  }
}

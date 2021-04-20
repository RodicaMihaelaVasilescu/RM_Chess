using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChessGame.Model
{
  class Movement
  {
    public ObservableCollection<ObservableCollection<Square>> Configuration { get; set; }
    public List<ChessPiece> Pieces { get; set; }
    public String MovementPiece { get; set; }
    public String MovementLocation { get; set; }
    public String MovementIcon { get; set; }
    public double IconSize { get; set; } = 25;
    public Movement(string name, string location, string ico)
    {
      MovementPiece = name;
      MovementLocation = location;
      MovementIcon = ico;
    }
  }
}

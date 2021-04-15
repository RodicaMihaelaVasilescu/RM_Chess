using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Mapper
{
  class ChessPieceLocation
  {
    public Dictionary<string, Pair> StringToCoordinates = new Dictionary<string, Pair>();

    public Dictionary<Pair, String> CoordinatesToString = new Dictionary<Pair, String>();

    private static ChessPieceLocation _instance;
    public static ChessPieceLocation Instance => _instance ?? (_instance = new ChessPieceLocation());

    public ChessPieceLocation()
    {
      char letter = 'A';
      string id;
      for (int i = 0; i < 8; i++)
      {
        char number = '8';
        for (int j = 0; j < 8; j++)
        {
          id = letter.ToString() + number.ToString();
          number--;

          Pair coordinates = new Pair(i, j);
          StringToCoordinates.Add(id, coordinates);
          CoordinatesToString.Add(coordinates, id);
        }
        letter++;
      }
    }


  }
}

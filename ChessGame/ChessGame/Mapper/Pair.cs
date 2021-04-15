using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Mapper
{
  public class Pair
  {
    public int i = 0;
    public int j = 0;
    public Pair(int x, int y)
    {
      i = x;
      j = y;
    }
    public override bool Equals(Object obj)
    {
      //Check for null and compare run-time types.
      if ((obj == null) || !this.GetType().Equals(obj.GetType()))
      {
        return false;
      }
      else
      {
        Pair p = (Pair)obj;
        return (i == p.i) && (j == p.j);
      }
    }

    public override int GetHashCode()
    {
      return (i << 2) ^ j;
    }

    public override string ToString()
    {
      return String.Format("Point({0}, {1})", i, j);
    }
  }
}

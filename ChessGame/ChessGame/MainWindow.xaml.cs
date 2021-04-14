using ChessGame.Model;
using ChessGame.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessGame
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    MainViewModel viewModel;

    public MainWindow()
    {
      InitializeComponent();
      viewModel = new MainViewModel();
      DataContext = viewModel;
      viewModel.InitializeChessBoard();
      
    }

    private void getSelectedItem(object sender, MouseButtonEventArgs e)
    {
      //var book = (Square)Chessboard.SelectedItems.;
      //System.Windows.MessageBox.Show(book.Id);
    }
    //public override bool Equals(object obj)
    //{
    //  if (this.name_to_show_menu == (obj as patient).name_to_show_menu)
    //    return true;
    //  else
    //    return false;
    //}
    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var listViewItem = sender as ListViewItem;
      //ListView listView = sender as ListView;
      List<Square> l = new List<Square>();
      foreach (ObservableCollection<Square> sq in viewModel.ChessBoard)
      {
        foreach (Square s in sq)
        {
          l.Add(s);
        }
      }

      //if (listViewItem != null && listViewItem.IsSelected)
      //{
      //  var sq = Chessboard.SelectedItem;
      //  //var selected = e.AddedItems[0] as StackPanel;
      //  int breakpoint = 0;

      //  //Do your stuff
      //}

      viewModel.DisplayAvailableSquares(listViewItem.Content.ToString());


      //  var square2 = l.Where(c => c.Id == listViewItem.Tag.ToString()).FirstOrDefault();

      //var emoticon = EmoticonCollection.Instance.Emoticons.Where(em => em.Icon == listViewItem.Content.ToString()).FirstOrDefault();
      //if (emoticon != null)
      //{
      //  TextEmoticon = emoticon.Text;
      //  EmoticonsViewModel viewModel = DataContext as EmoticonsViewModel;
      //  viewModel.TextEmoticon = emoticon.Text;
      //  viewModel.CloseAction();
      //}
      int x = 0;
    }




  }
}

using ChessGame.Model;
using ChessGame.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
    }

    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var listViewItem = sender as ListViewItem;
      List<Square> l = new List<Square>();
      foreach (ObservableCollection<Square> sq in viewModel.ChessBoard)
      {
        foreach (Square s in sq)
        {
          l.Add(s);
        }
      }

      viewModel.DisplayAvailableSquares(listViewItem.Content.ToString());

      if (viewModel.AddMoveSound)
      {
        if (viewModel.IsPieceCaptured)
        {
          SoundPlayer simpleSound = new SoundPlayer(@"D:\GitHub\RM_Chess\ChessGame\ChessGame\Resources\click2.wav");
          simpleSound.Play();
        }
        else
        {
          SoundPlayer simpleSound = new SoundPlayer(@"D:\GitHub\RM_Chess\ChessGame\ChessGame\Resources\click.wav");
          simpleSound.Play();
        }
      }

      if(viewModel.Checkmate)
      {
        SoundPlayer simpleSound = new SoundPlayer(@"D:\GitHub\RM_Chess\ChessGame\ChessGame\Resources\checkmate.wav");
        simpleSound.Play();
      }
    }

    private void ChessboardItem_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key >= Key.A && e.Key <= Key.Z /*&& MyTextBox.IsFocused == false*/)
      {
        return;
      }
      switch (e.Key)
      {
        case Key.Down:
          if (viewModel.SelectedMovementIndex > 0)
            viewModel.SelectedMovementIndex--;
          e.Handled = true;
          break;
        case Key.Right:
          if (viewModel.SelectedMovementIndex < viewModel.ListOfMovements.Count - 1)
            viewModel.SelectedMovementIndex++;
          e.Handled = true;
          break;
        case Key.Left:
          if (viewModel.SelectedMovementIndex > 0)
            viewModel.SelectedMovementIndex--;
          e.Handled = true;
          break;
        case Key.Up:
          if (viewModel.SelectedMovementIndex < viewModel.ListOfMovements.Count - 1)
            viewModel.SelectedMovementIndex++;
          e.Handled = true;
          break;
        //case Key.Enter:
        //  if (MyTextBox.IsFocused == false)
        //  {
        //    viewModel.FindCommandExecute();
        //    e.Handled = true;
        //  }
        //  break;
        default: break;
      }


    }
  }
}

using ChessGame.Model;
using ChessGame.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

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

      string fullPathToSound = (Assembly.GetEntryAssembly().Location + "");
      if (viewModel.AddMoveSound)
      {
        if (viewModel.IsPieceCaptured)
        {
          fullPathToSound = fullPathToSound.Replace("ChessGame.exe", "Resources\\capture_sound.wav");
          SoundPlayer simpleSound = new SoundPlayer(fullPathToSound);
          simpleSound.Play();
        }
        else
        {
          fullPathToSound = fullPathToSound.Replace("ChessGame.exe", "Resources\\move_sound.wav");
          SoundPlayer simpleSound = new SoundPlayer(fullPathToSound);
          simpleSound.Play();
        }
      }

      if (viewModel.IsCheckmate)
      {
        fullPathToSound = fullPathToSound.Replace("ChessGame.exe", "Resources\\checkmate_sound.wav");
        SoundPlayer simpleSound = new SoundPlayer(fullPathToSound);
        simpleSound.Play();
      }
    }

    private void ChessboardItem_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key >= Key.A && e.Key <= Key.Z)
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
        default: break;
      }


    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      viewModel = new MainViewModel();
      DataContext = viewModel;
      viewModel.InitializeChessBoard();
      ListOfMovements.Width = SystemParameters.WorkArea.Width / 4;
      Chat.Width = SystemParameters.WorkArea.Width / 4;
      ListOfMovements.MaxHeight = SystemParameters.WorkArea.Height/1.4;
    }
  }
}

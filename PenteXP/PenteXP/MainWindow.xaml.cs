using Microsoft.Win32;
using PenteXP.Models;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using swf = System.Windows.Forms;

namespace PenteXP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int turnOrder = 1;
        private Player[] players = new Player[2];
        private swf.Timer timer = new swf.Timer();
        private const int turnTimer = 20;
        private int ticks = turnTimer;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Start();
            timer.Tick += TickTest;
        }

        private void TickTest(object sender, EventArgs e)
        {
            ticks--;
            if (ticks == 0)
            {
                timer.Stop();
                ticks = turnTimer;
                MessageBox.Show("You're turn has been skipped", "Turn Skipped!", MessageBoxButton.OK);
                turnOrder++;
                timer.Start();
            }
            test.Content = ticks;
        }

        public void InitializeBoard()
        {
            GameBoard.Children.Clear();
            int boardSize = (int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value;
            PlayerTurnOrder.Visibility = Visibility.Visible;
            for (int i = 0; i < boardSize; i++)
            {
                Label spot = new Label();
                spot.MouseLeftButtonDown += Label_MouseLeftButtonDown;
                if (i == boardSize / 2)
                {
                    spot.Name = "StartingTile";
                    Uri resourceUri = new Uri("Images/StartingTile.png", UriKind.Relative);
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush
                    {
                        ImageSource = temp,
                        Stretch = Stretch.Uniform
                    };
                    spot.Background = brush;
                }
                else
                {
                    spot.Name = "RegularTile";
                    Uri resourceUri = new Uri("Images/BlankBoard.png", UriKind.Relative);
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush
                    {
                        ImageSource = temp,
                        Stretch = Stretch.Uniform
                    };
                    spot.Background = brush;
                }
                GameBoard.Children.Add(spot);
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label test = (Label)sender;
            if (test.Name == "RegularTile" || test.Name == "StartingTile")
            {
                if (turnOrder == 1)
                {
                    Label label = (Label)sender;
                    if (label.Name == "StartingTile")
                    {
                        Label spot = (Label)sender;
                        spot.Name = "BlackPiece";
                        Uri resourceUri = new Uri("Images/BlackPiece.png", UriKind.Relative);
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush
                        {
                            ImageSource = temp
                        };
                        spot.Background = brush;
                        sender = spot;
                        turnOrder++;
                    }
                    else
                    {
                        MessageBox.Show("You must place the first peice in the center as indicated");
                    }
                }
                else
                {
                    if (turnOrder % 2 == 0)
                    {
                        Label label = (Label)sender;
                        label.Name = "WhitePiece";
                        Uri resourceUri = new Uri("Images/WhitePiece.png", UriKind.Relative);
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush
                        {
                            ImageSource = temp,
                            Stretch = Stretch.Fill
                        };
                        label.Background = brush;
                        sender = label;
                    }
                    else
                    {
                        Label label = (Label)sender;
                        label.Name = "BlackPiece";
                        Uri resourceUri = new Uri("Images/BlackPiece.png", UriKind.Relative);
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush
                        {
                            ImageSource = temp,
                            Stretch = Stretch.Fill
                        };
                        label.Background = brush;
                        sender = label;
                    }
                    turnOrder++;
                }
            }
            else
            {
                MessageBox.Show("There is a piece there already");
            }
            if (WinCheck(sender, e) == true)
            {

            }
            ticks = turnTimer;
        }

        private bool WinCheck(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            int blackCounter = 0;
            int whiteCounter = 0;



            return false;
        }

        private void Refresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            turnOrder = 1;
            GameBoard.Children.Clear();
            Player.id = 1;
            PlayerTurnOrder.Visibility = Visibility.Hidden;
            PlayerDetails.Visibility = Visibility.Visible;
            BoardCover.Visibility = Visibility.Visible;
            players = new Player[2];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BoardCover.Visibility = Visibility.Hidden;
            players[0] = new HumanPlayer(Player1Name.Text);
            players[1] = new HumanPlayer(Player2Name.Text);
            PlayerDetails.Visibility = Visibility.Hidden;
            PlayerTurnOrder.Visibility = Visibility.Visible;
            Player1Info.Content = players[0].ToString();
            Player2Info.Content = players[1].ToString();
            InitializeBoard();
        }

        private void Instructions_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("\t\tPente\nPlace pieces 5 in a row or get 5 captures to win");
        }

        private void SaveGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            gameModel model = new gameModel();
            foreach (Label name in GameBoard.Children)
            {
                model.boardpieces.Add(name.Name);
            }
            foreach (Player player in players)
            {
                model.players.Add(player);
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".pente";
            sfd.FileName = "pente.pente";
            sfd.Filter = "Pente Saves (*.pente)|*.pente";
            if (sfd.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(sfd.FileName, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(stream, model);
                }
            }
        }

        private void LoadGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".pente";
            ofd.Filter = "Pente Saves (*.pente)|*.pente";
            gameModel newGame = null;
            if (ofd.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(ofd.FileName, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    newGame = (gameModel)bf.Deserialize(stream);
                }
                
            }
        }

        private void LoadGame(gameModel game)
        {

        }
    }
}
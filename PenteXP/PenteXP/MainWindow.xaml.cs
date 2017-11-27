using Microsoft.Win32;
using PenteXP.Models;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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
        private int turnOrder = 2;
        public Player[] players = new Player[2];
        private swf.Timer timer = new swf.Timer();
        private const int turnTimer = 20;
        private int ticks = turnTimer;

        public MainWindow()
        {
            InitializeComponent();
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
                    spot.Name = "BlackPiece";
                    Uri resourceUri = new Uri("Images/BlackPiece.png", UriKind.Relative);
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
            Label piecePlaced = (Label)sender;
            if ()
            {

            }
            if (piecePlaced.Name == "RegularTile" || piecePlaced.Name == "StartingTile")
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
                CaptureCheck(sender);
                turnOrder++;
                UpdatePlayerTurn();
            }
            else
            {
                MessageBox.Show("There is a piece there already");
            }
            ticks = turnTimer;
            if (WinCheck(sender) == true)
            {
                timer.Stop();
                MessageBox.Show($"{players[turnOrder % 2].Name} wins!");
                Refresh_Executed(sender, null);
            }
        }

        private bool WinCheck(object sender)
        {
            bool playerWin = false;
            if (players[turnOrder % 2].Captures >= 5)
            {
                playerWin = true;
                return playerWin;
            }
            Label label = (Label)sender;
            int blackCounter = 0;
            int whiteCounter = 0;


            return false;
        }

        private void CaptureCheck(object sender)
        {
            Label label = (Label)sender;
            int startPosition = GameBoard.Children.IndexOf(label);
            HorizontalLeftCheckCapture(startPosition, label);
            HorizontalRightCheckCapture(startPosition, label);
            VerticalUpCheckCapture(startPosition, label);
            VerticalDownCheckCapture(startPosition, label);
            DiagonalUpLeftCheckCapture(startPosition, label);
            DiagonalUpRightCheckCapture(startPosition, label);
            DiagonalDownLeftCheckCapture(startPosition, label);
            DiagonalDownRightCheckCapture(startPosition, label);
        }

        private bool HorizontalLeftCheckCapture(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition - 1;
            if (currentPosition % (int)BoardSizeSlider.Value >= 2 && currentPosition >= 0)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[currentPosition - 1];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[currentPosition - 2];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition - 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
            else
            {
                return isCapture;
            }
            return isCapture;
        }

        private bool HorizontalRightCheckCapture(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition + 1;
            if (currentPosition % (int)BoardSizeSlider.Value < (int)BoardSizeSlider.Value - 2 && currentPosition < (int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[currentPosition + 1];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[currentPosition + 2];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition + 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
            else
            {
                return isCapture;
            }
            return isCapture;
        }

        private bool VerticalDownCheckCapture(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition + (int)BoardSizeSlider.Value;
            if (currentPosition / (int)BoardSizeSlider.Value < (int)BoardSizeSlider.Value - 2)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[currentPosition + (int)BoardSizeSlider.Value];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[currentPosition + ((int)BoardSizeSlider.Value * 2)];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition + (int)BoardSizeSlider.Value);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
            else
            {
                return isCapture;
            }
            return isCapture;
        }

        private bool VerticalUpCheckCapture(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition - (int)BoardSizeSlider.Value;
            if (currentPosition / (int)BoardSizeSlider.Value >= 2)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[currentPosition - (int)BoardSizeSlider.Value];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[currentPosition - ((int)BoardSizeSlider.Value * 2)];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition - (int)BoardSizeSlider.Value);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
            else
            {
                return isCapture;
            }
            return isCapture;
        }
        
        private bool DiagonalUpLeftCheckCapture(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition - (int)BoardSizeSlider.Value - 1;
            if (currentPosition / (int)BoardSizeSlider.Value >= 2 && currentPosition % (int)BoardSizeSlider.Value >= 2)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[(currentPosition - (int)BoardSizeSlider.Value) - 1];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[(currentPosition - ((int)BoardSizeSlider.Value * 2)) - 2];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition - (int)BoardSizeSlider.Value - 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
            else
            {
                return isCapture;
            }
            return isCapture;
        }

        private bool DiagonalUpRightCheckCapture(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition - (int)BoardSizeSlider.Value + 1;
            if (currentPosition / (int)BoardSizeSlider.Value >= 2 && currentPosition % (int)BoardSizeSlider.Value <= (int)BoardSizeSlider.Value - 3)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[(currentPosition - (int)BoardSizeSlider.Value) + 1];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[(currentPosition - ((int)BoardSizeSlider.Value * 2)) + 2];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition - (int)BoardSizeSlider.Value + 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
            else
            {
                return isCapture;
            }
            return isCapture;
        }

        private bool DiagonalDownLeftCheckCapture(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition + (int)BoardSizeSlider.Value - 1;
            if (currentPosition / (int)BoardSizeSlider.Value < (int)BoardSizeSlider.Value - 2 && currentPosition % (int)BoardSizeSlider.Value >= 2 && currentPosition % (int)BoardSizeSlider.Value >= 2)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[(currentPosition + (int)BoardSizeSlider.Value) - 1];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[(currentPosition + ((int)BoardSizeSlider.Value * 2)) - 2];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition + (int)BoardSizeSlider.Value - 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
            else
            {
                return isCapture;
            }
            return isCapture;
        }

        private bool DiagonalDownRightCheckCapture(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition + (int)BoardSizeSlider.Value + 1;
            if (currentPosition / (int)BoardSizeSlider.Value < (int)BoardSizeSlider.Value - 2 && currentPosition % (int)BoardSizeSlider.Value <= (int)BoardSizeSlider.Value - 3)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[(currentPosition + (int)BoardSizeSlider.Value) + 1];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[(currentPosition + ((int)BoardSizeSlider.Value * 2)) + 2];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition + (int)BoardSizeSlider.Value + 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
            else
            {
                return isCapture;
            }
            return isCapture;
        }

        private void UpdatePlayerTurn()
        {
            Player1Info.Content = players[0].ToString();
            Player2Info.Content = players[1].ToString();
        }

        private void CaptureRemove(int label1, int label2)
        {
            Label replaceLabel = new Label();
            Uri resourceUri = new Uri("Images/BlankBoard.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            var brush = new ImageBrush
            {
                ImageSource = temp,
                Stretch = Stretch.Uniform
            };
            replaceLabel.Background = brush;
            GameBoard.Children.Cast<Label>().ElementAt(label1).Background = brush;
            GameBoard.Children.Cast<Label>().ElementAt(label2).Background = brush;
            GameBoard.Children.Cast<Label>().ElementAt(label1).Name = "RegularTile";
            GameBoard.Children.Cast<Label>().ElementAt(label2).Name = "RegularTile";
        }

        private void Refresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.Stop();
            turnOrder = 2;
            GameBoard.Children.Clear();
            Player.id = 1;
            PlayerTurnOrder.Visibility = Visibility.Hidden;
            PlayerDetails.Visibility = Visibility.Visible;
            BoardCover.Visibility = Visibility.Visible;
            players = new Player[2];
            ticks = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BoardCover.Visibility = Visibility.Hidden;
            players[0] = new HumanPlayer(Player1Name.Text);
            players[1] = new HumanPlayer(Player2Name.Text);
            PlayerDetails.Visibility = Visibility.Hidden;
            PlayerTurnOrder.Visibility = Visibility.Visible;
            InitializeBoard();
            timer.Interval = 1000;
            timer.Start();
            UpdatePlayerTurn();
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
            GameBoard.Children.Clear();
        }
    }
}
using Microsoft.Win32;
using PenteXP.Models;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
                    CaptureCheck(sender);
                    if (WinCheck(sender) == true)
                    {

                    }
                    turnOrder++;
                }
            }
            else
            {
                MessageBox.Show("There is a piece there already");
            }
            ticks = turnTimer;
        }

        private bool WinCheck(object sender)
        {
            Label label = (Label)sender;
            int blackCounter = 0;
            int whiteCounter = 0;

            

            return false;
        }

        private void CaptureCheck(object sender)
        {
            Label label = (Label)sender;
            int startPosition = GameBoard.Children.IndexOf(label);
            HorizontalLeftCheck(startPosition, label);
            HorizontalRightCheck(startPosition, label);
            VerticalUpCheck(startPosition, label);
            VerticalDownCheck(startPosition, label);
            DiagonalUpLeftCheck(startPosition, label);
            DiagonalUpRightCheck(startPosition, label);
            DiagonalDownLeftCheck(startPosition, label);
            DiagonalDownRightCheck(startPosition, label);
        }

        private bool HorizontalLeftCheck(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition - 1;
            if (currentPosition % (int)BoardSizeSlider.Value > 3)
            {
                Label l = (Label)GameBoard.Children[currentPosition];
                if (l.Name != sender.Name && (l.Name != "RegularTile" && l.Name != "StartingTile"))
                {
                    Label secondLabel = (Label)GameBoard.Children[currentPosition - 1];
                    if (secondLabel.Name != sender.Name && (secondLabel.Name != "RegularTile" && secondLabel.Name != "StartingTile"))
                    {
                        Label finalLabel = (Label)GameBoard.Children[currentPosition - 2];
                        if(finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition-1);
                            players[turnOrder % 2].Captures++;
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

        private bool HorizontalRightCheck(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition + 1;
            if (currentPosition % (int)BoardSizeSlider.Value < (int)BoardSizeSlider.Value-3)
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
                            players[turnOrder % 2].Captures++;
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

        private bool VerticalDownCheck(int startPosition, Label sender)
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
                        Label finalLabel = (Label)GameBoard.Children[currentPosition + ((int)BoardSizeSlider.Value*2)];
                        if (finalLabel.Name == sender.Name)
                        {
                            isCapture = true;
                            CaptureRemove(currentPosition, currentPosition + (int)BoardSizeSlider.Value);
                            players[turnOrder % 2].Captures++;
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

        private bool VerticalUpCheck(int startPosition, Label sender)
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
                            players[turnOrder % 2].Captures++;
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

        private bool DiagonalUpLeftCheck(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition - (int)BoardSizeSlider.Value - 1;
            if (currentPosition / (int)BoardSizeSlider.Value >= 2 && currentPosition % (int)BoardSizeSlider.Value > 3)
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
                            players[turnOrder % 2].Captures++;
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

        private bool DiagonalUpRightCheck(int startPosition, Label sender)
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
                            players[turnOrder % 2].Captures++;
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

        private bool DiagonalDownLeftCheck(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition - (int)BoardSizeSlider.Value - 1;
            if (currentPosition / (int)BoardSizeSlider.Value < (int)BoardSizeSlider.Value - 2 && currentPosition % (int)BoardSizeSlider.Value > 3 && currentPosition % (int)BoardSizeSlider.Value > 3)
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
                            players[turnOrder % 2].Captures++;
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

        private bool DiagonalDownRightCheck(int startPosition, Label sender)
        {
            bool isCapture = false;
            int currentPosition = startPosition - (int)BoardSizeSlider.Value + 1;
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
                            players[turnOrder % 2].Captures++;
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
            timer.Interval = 1000;
            timer.Start();
            timer.Tick += TickTest;
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
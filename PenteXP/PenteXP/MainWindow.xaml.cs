using PenteXP.Models;
using System;
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
            InitializeBoard();
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
                timer.Start();
            }
            test.Content = ticks;
        }

        public void InitializeBoard()
        {
            PlayerTurnOrder.Visibility = Visibility.Hidden;
            BoardCover.Visibility = Visibility.Visible;
            for (int i = 0; i < 361; i++)
            {
                Label spot = new Label();
                spot.MouseLeftButtonDown += Label_MouseLeftButtonDown;
                if (i == 180)
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
                        spot.Name = "Piece";
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
                        label.Name = "Piece";
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
                        label.Name = "Piece";
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
        }

        private bool WinCheck(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            return false;
        }

        private void Refresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            turnOrder = 1;
            GameBoard.Children.Clear();
            InitializeBoard();
            Player.id = 1;
            PlayerDetails.Visibility = Visibility.Visible;
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
        }

        private void Instructions_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("\t\tPente\nPlace pieces 5 in a row or get 5 captures to win");
        }

        ////https://stackoverflow.com/questions/40979793/how-to-invert-an-image
        ////Courtesy of Patrick from StackOverflow
        //public static BitmapSource Invert(BitmapSource source)
        //{
        //    int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

        //    int length = stride * source.PixelHeight;
        //    byte[] data = new byte[length];

        //    source.CopyPixels(data, stride, 0);

        //    for (int i = 0; i < length; i += 4)
        //    {
        //        data[i] = (byte)(255 - data[i]);
        //        data[i + 1] = (byte)(255 - data[i + 1]);
        //        data[i + 2] = (byte)(255 - data[i + 2]);
        //    }

        //    return BitmapSource.Create(
        //        source.PixelWidth, source.PixelHeight,
        //        source.DpiX, source.DpiY, source.Format,
        //        null, data, stride);
        //}
    }
}
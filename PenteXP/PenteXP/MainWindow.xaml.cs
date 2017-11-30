using Microsoft.Win32;
using PenteXP.Models;
using System;
using System.Collections.Generic;
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
        private Label player1LastPiece = new Label();
        private Label player1SecondToLastPiece = new Label();
        private List<Label> player2LastPiece = new List<Label>();
        private bool tria = false;
        private bool tessera = false;

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += TickTest;
            timer.Interval = 1000;
        }

        private void TickTest(object sender, EventArgs e)
        {
            ticks--;
            if (ticks == 0)
            {
                timer.Stop();
                ticks = turnTimer;
                MessageBox.Show("You're turn has been skipped", "Turn Skipped!", MessageBoxButton.OK);
                if (players[1].GetType() == typeof(HumanPlayer))
                {
                    turnOrder++;
                }
                else
                {
                    turnOrder--;
                    AITakeTurn();
                }
                UpdatePlayerTurn();
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
                    player1LastPiece = spot;
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
            if (turnOrder == 3)
            {
                int playerPiece = GameBoard.Children.IndexOf((Label)sender);
                int row3Center = ((int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value) / 2;
                int row1Center = row3Center - ((int)BoardSizeSlider.Value * 2);
                int row2Center = row3Center - ((int)BoardSizeSlider.Value);
                int row4Center = row3Center + ((int)BoardSizeSlider.Value);
                int row5Center = row3Center + ((int)BoardSizeSlider.Value * 2);
                if ((playerPiece >= row1Center - 2 && playerPiece <= row1Center + 2) || (playerPiece >= row2Center - 2 && playerPiece <= row2Center + 2) || (playerPiece >= row3Center - 2 && playerPiece <= row3Center + 2) || (playerPiece >= row4Center - 2 && playerPiece <= row4Center + 2) || (playerPiece >= row5Center - 2 && playerPiece <= row5Center + 2))
                {
                    timer.Stop();
                    MessageBox.Show("Invalid Move: Tournament rules state that you must place your second piece at least 3 spaces away from the center.");
                    timer.Start();
                }
                else if (piecePlaced.Name == "RegularTile" || piecePlaced.Name == "StartingTile")
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
                    turnOrder++;
                    player1SecondToLastPiece = player1LastPiece;
                    player1LastPiece = label;
                    UpdatePlayerTurn();
                    if (players[1].GetType() == typeof(AIPlayer))
                    {
                        AITakeTurn();
                    }
                }
                else
                {
                    MessageBox.Show("There is a piece there already");
                }
            }
            else
            {
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
                        player2LastPiece.Add(label);
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
                        player1SecondToLastPiece = player1LastPiece;
                        player1LastPiece = label;
                    }
                    CaptureCheck(sender);
                    turnOrder++;
                    UpdatePlayerTurn();
                    tria = false;
                    tessera = false;
                    if (WinCheck(sender))
                    {
                        timer.Stop();
                        MessageBox.Show($"{players[turnOrder % 2].Name} wins!");
                        ticks = turnTimer;
                        Refresh_Executed(null, null);
                    }
                    else if (players[1].GetType() == typeof(AIPlayer))
                    {
                        AITakeTurn();
                    }
                }
                else
                {
                    MessageBox.Show("There is a piece there already");
                }
            }
            ticks = turnTimer;
        }

        public void AITakeTurn()
        {
            int aiPlayerIndex = 0;
            Label aiPlayerPiece = null;
            #region Random AI Placement
            //Random rand = new Random();
            //aiPlayerIndex = rand.Next(((int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value) - 1);
            //do
            //{
            //    randPiece = rand.Next(((int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value) - 1);
            //    aiPlayerPiece = (Label)GameBoard.Children[randPiece];
            //} while (aiPlayerPiece.Name == "BlackPiece" || aiPlayerPiece.Name == "WhitePiece");
            #endregion

            if (!tria && !tessera)
            {
                bool validPlacement = false;
                do
                {
                    try
                    {
                        aiPlayerIndex = GameBoard.Children.IndexOf(player2LastPiece.Last()) + 1;
                        aiPlayerPiece = (Label)GameBoard.Children[aiPlayerIndex];
                        if (aiPlayerPiece.Name != "RegularTile")
                        {
                            aiPlayerIndex = GameBoard.Children.IndexOf(player2LastPiece.Last()) - 1;
                            aiPlayerPiece = (Label)GameBoard.Children[aiPlayerIndex];
                            if (aiPlayerPiece.Name != "RegularTile")
                            {
                                aiPlayerIndex = GameBoard.Children.IndexOf(player2LastPiece.Last()) + (int)BoardSizeSlider.Value;
                                aiPlayerPiece = (Label)GameBoard.Children[aiPlayerIndex];
                                if (aiPlayerPiece.Name != "RegularTile")
                                {
                                    aiPlayerIndex = GameBoard.Children.IndexOf(player2LastPiece.Last()) - (int)BoardSizeSlider.Value;
                                    aiPlayerPiece = (Label)GameBoard.Children[aiPlayerIndex];
                                    if (aiPlayerPiece.Name != "RegularTile")
                                    {
                                        aiPlayerIndex = GameBoard.Children.IndexOf(player2LastPiece.Last()) - (int)BoardSizeSlider.Value + 1;
                                        aiPlayerPiece = (Label)GameBoard.Children[aiPlayerIndex];
                                        if (aiPlayerPiece.Name != "RegularTile")
                                        {
                                            aiPlayerIndex = GameBoard.Children.IndexOf(player2LastPiece.Last()) - (int)BoardSizeSlider.Value - 1;
                                            aiPlayerPiece = (Label)GameBoard.Children[aiPlayerIndex];
                                            if (aiPlayerPiece.Name != "RegularTile")
                                            {
                                                aiPlayerIndex = GameBoard.Children.IndexOf(player2LastPiece.Last()) + (int)BoardSizeSlider.Value - 1;
                                                aiPlayerPiece = (Label)GameBoard.Children[aiPlayerIndex];
                                                if (aiPlayerPiece.Name != "RegularTile")
                                                {
                                                    aiPlayerIndex = GameBoard.Children.IndexOf(player2LastPiece.Last()) + (int)BoardSizeSlider.Value + 1;
                                                    aiPlayerPiece = (Label)GameBoard.Children[aiPlayerIndex];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        validPlacement = true;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        player2LastPiece.Remove(player2LastPiece.Last());
                    }
                    catch (ArgumentOutOfRangeException )
                    {
                        player2LastPiece.Remove(player2LastPiece.Last());
                    }
                    

                } while (!validPlacement);
            }
            else
            {
                int firstIndex = GameBoard.Children.IndexOf(player1LastPiece);
                int secondIndex = GameBoard.Children.IndexOf(player1SecondToLastPiece);
                int rateOfChange = firstIndex - secondIndex;
                if (Math.Abs(rateOfChange) > BoardSizeSlider.Value + 1)
                {
                    if (rateOfChange % (int)BoardSizeSlider.Value == 0)
                    {
                        if(rateOfChange > 0)
                        {
                            rateOfChange = (int)BoardSizeSlider.Value;
                        }
                        else
                        {
                            rateOfChange = -(int)BoardSizeSlider.Value;
                        }
                    }
                    else if (rateOfChange % ((int)BoardSizeSlider.Value + 1) == 0)
                    {
                        if (rateOfChange > 0)
                        {
                            rateOfChange = (int)BoardSizeSlider.Value + 1;
                        }
                        else
                        {
                            rateOfChange = ((int)BoardSizeSlider.Value + 1) * -1;
                        }
                    }
                    else if (rateOfChange % ((int)BoardSizeSlider.Value - 1) == 0)
                    {
                        if (rateOfChange > 0)
                        {
                            rateOfChange = (int)BoardSizeSlider.Value - 1;
                        }
                        else
                        {
                            rateOfChange = ((int)BoardSizeSlider.Value - 1) * -1;
                        }
                    }
                    else
                    {
                        rateOfChange = 1;
                    }
                }
                else if (Math.Abs(rateOfChange) == 3 || Math.Abs(rateOfChange) == 4)
                {
                    if(rateOfChange > 0)
                    {
                        rateOfChange = 1;
                    }
                    else
                    {
                        rateOfChange = -1;
                    }
                }
                if (Math.Abs(rateOfChange) == 1 && firstIndex % (int)BoardSizeSlider.Value == (int)BoardSizeSlider.Value-1)
                {
                    if (tessera)
                    {
                        rateOfChange = -4;
                    }
                    else
                    {
                        rateOfChange = -3;
                    }
                }
                else if (Math.Abs(rateOfChange) == 1 && firstIndex % (int)BoardSizeSlider.Value == 0)
                {
                    if (tessera)
                    {
                        rateOfChange = 4;
                    }
                    else
                    {
                        rateOfChange = 3;
                    }
                }
                aiPlayerPiece = (Label)GameBoard.Children[firstIndex + rateOfChange];
                
                while (aiPlayerPiece.Name != "RegularTile")
                {
                    aiPlayerPiece = (Label)GameBoard.Children[GameBoard.Children.IndexOf(aiPlayerPiece) + rateOfChange];
                }
            }

            Label label = new Label();
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
            GameBoard.Children.Cast<Label>().ElementAt(GameBoard.Children.IndexOf(aiPlayerPiece)).Background = brush;
            GameBoard.Children.Cast<Label>().ElementAt(GameBoard.Children.IndexOf(aiPlayerPiece)).Name = "WhitePiece";
            turnOrder++;
            if (WinCheck(aiPlayerPiece))
            {
                timer.Stop();
                MessageBox.Show($"{players[turnOrder % 2].Name} wins!");
                Refresh_Executed(null, null);
            }
            player2LastPiece.Add(aiPlayerPiece);
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
            if (!playerWin)
            {
                playerWin = HorizontalWinCheck(GameBoard.Children.IndexOf(label), label);
            }
            if (!playerWin)
            {
                playerWin = VerticalWinCheck(GameBoard.Children.IndexOf(label), label);
            }
            if (!playerWin)
            {
                playerWin = DiagonalRightWinCheck(GameBoard.Children.IndexOf(label), label);
            }
            if (!playerWin)
            {
                playerWin = DiagonalLeftWinCheck(GameBoard.Children.IndexOf(label), label);
            }


            return playerWin;
        }

        private bool HorizontalWinCheck(int startPosition, Label sender)
        {
            bool hasWon = false;
            int totalRowCount = 1;
            int startCap = (((startPosition / (int)BoardSizeSlider.Value)) * (int)BoardSizeSlider.Value);
            int endCap = (((startPosition / (int)BoardSizeSlider.Value)+1) * (int)BoardSizeSlider.Value) - 1;
            bool keepGoing = true;
            int emptyEndCaps = 0;
            
            for (int i = startPosition-1; i >= startCap && keepGoing; i--)
            {
                Label l = (Label)GameBoard.Children[i];
                if (l.Name == sender.Name)
                {
                    totalRowCount++;
                }
                else
                {
                    if (l.Name == "RegularTile" || l.Name == "StartingTile")
                    {
                        emptyEndCaps++;
                    }
                    keepGoing = false;
                }

            }
            keepGoing = true;
            for (int i = startPosition+1; i <= endCap && keepGoing; i++)
            {
                Label l = (Label)GameBoard.Children[i];
                if (l.Name == sender.Name)
                {
                    totalRowCount++;
                }
                else
                {
                    if (l.Name == "RegularTile" || l.Name == "StartingTile")
                    {
                        emptyEndCaps++;
                    }
                    keepGoing = false;
                }
            }
            hasWon = totalRowCount >= 5;
            if (!hasWon)
            {
                if (totalRowCount == 3 && emptyEndCaps == 2)
                {
                    MessageBox.Show($"{players[turnOrder%2].Name} got a TRIA!");
                    if (players[0].GetType() == typeof(HumanPlayer))
                    {
                        tria = true;
                    }
                }
                else if(totalRowCount == 4 && emptyEndCaps >= 1)
                {
                    MessageBox.Show($"{players[turnOrder % 2].Name} got a TESSERA!");
                    if (players[0].GetType() == typeof(HumanPlayer))
                    {
                        tessera = true;
                    }
                }
            }
            return hasWon;
        }

        private bool VerticalWinCheck(int startPosition, Label sender)
        {
            bool hasWon = false;
            int totalRowCount = 1;
            int startCap = startPosition % (int)BoardSizeSlider.Value;
            int endCap = (int)BoardSizeSlider.Value* (int)BoardSizeSlider.Value - ((int)BoardSizeSlider.Value - startCap);
            bool keepGoing = true;
            int emptyEndCaps = 0;

            for (int i = startPosition - (int)BoardSizeSlider.Value; i >= startCap && keepGoing; i -= (int)BoardSizeSlider.Value)
            {
                Label l = (Label)GameBoard.Children[i];
                if (l.Name == sender.Name)
                {
                    totalRowCount++;
                }
                else
                {
                    if (l.Name == "RegularTile" || l.Name == "StartingTile")
                    {
                        emptyEndCaps++;
                    }
                    keepGoing = false;
                }

            }
            keepGoing = true;
            for (int i = startPosition + (int)BoardSizeSlider.Value; i <= endCap && keepGoing; i += (int)BoardSizeSlider.Value)
            {
                Label l = (Label)GameBoard.Children[i];
                if (l.Name == sender.Name)
                {
                    totalRowCount++;
                }
                else
                {
                    if (l.Name == "RegularTile" || l.Name == "StartingTile")
                    {
                        emptyEndCaps++;
                    }
                    keepGoing = false;
                }
            }
            hasWon = totalRowCount >= 5;
            if (!hasWon)
            {
                if (totalRowCount == 3 && emptyEndCaps == 2)
                {
                    MessageBox.Show($"{players[turnOrder % 2].Name} got a TRIA!");
                    if (players[0].GetType() == typeof(HumanPlayer))
                    {
                        tria = true;
                    }
                }
                else if (totalRowCount == 4 && emptyEndCaps >= 1)
                {
                    MessageBox.Show($"{players[turnOrder % 2].Name} got a TESSERA!");
                    if (players[0].GetType() == typeof(HumanPlayer))
                    {
                        tessera = true;
                    }
                }
            }
            return hasWon;
        }

        private bool DiagonalRightWinCheck(int startPosition, Label sender)
        {
            bool hasWon = false;
            int totalRowCount = 1;
            bool keepGoing = true;
            int emptyEndCaps = 0;

            for (int i = startPosition - (int)BoardSizeSlider.Value +1 ; keepGoing && i > 0; i -= ((int)BoardSizeSlider.Value - 1))
            {
                Label l = (Label)GameBoard.Children[i];
                if (l.Name == sender.Name)
                {
                    totalRowCount++;
                }
                else
                {
                    if (l.Name == "RegularTile" || l.Name == "StartingTile")
                    {
                        emptyEndCaps++;
                    }
                    keepGoing = false;
                }

            }
            keepGoing = true;
            for (int i = startPosition + (int)BoardSizeSlider.Value - 1; keepGoing && i < (int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value; i += (int)BoardSizeSlider.Value - 1)
            {
                Label l = (Label)GameBoard.Children[i];
                if (l.Name == sender.Name)
                {
                    totalRowCount++;
                }
                else
                {
                    if (l.Name == "RegularTile" || l.Name == "StartingTile")
                    {
                        emptyEndCaps++;
                    }
                    keepGoing = false;
                }
            }
            hasWon = totalRowCount >= 5;
            if (!hasWon)
            {
                if (totalRowCount == 3 && emptyEndCaps == 2)
                {
                    MessageBox.Show($"{players[turnOrder % 2].Name} got a TRIA!");
                    if (players[0].GetType() == typeof(HumanPlayer))
                    {
                        tria = true;
                    }
                }
                else if (totalRowCount == 4 && emptyEndCaps >= 1)
                {
                    MessageBox.Show($"{players[turnOrder % 2].Name} got a TESSERA!");
                    if (players[0].GetType() == typeof(HumanPlayer))
                    {
                        tessera = true;
                    }
                }
            }
            return hasWon;
        }

        private bool DiagonalLeftWinCheck(int startPosition, Label sender)
        {
            bool hasWon = false;
            int totalRowCount = 1;
            bool keepGoing = true;
            int emptyEndCaps = 0;

            for (int i = startPosition - (int)BoardSizeSlider.Value - 1; keepGoing && i >= 0; i -= ((int)BoardSizeSlider.Value + 1))
            {
                Label l = (Label)GameBoard.Children[i];
                if (l.Name == sender.Name)
                {
                    totalRowCount++;
                }
                else
                {
                    if (l.Name == "RegularTile" || l.Name == "StartingTile")
                    {
                        emptyEndCaps++;
                    }
                    keepGoing = false;
                }

            }
            keepGoing = true;
            for (int i = startPosition + (int)BoardSizeSlider.Value + 1; keepGoing && i < (int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value; i += (int)BoardSizeSlider.Value + 1)
            {
                Label l = (Label)GameBoard.Children[i];
                if (l.Name == sender.Name)
                {
                    totalRowCount++;
                }
                else
                {
                    if (l.Name == "RegularTile" || l.Name == "StartingTile")
                    {
                        emptyEndCaps++;
                    }
                    keepGoing = false;
                }
            }
            hasWon = totalRowCount >= 5;
            if (!hasWon)
            {
                if (totalRowCount == 3 && emptyEndCaps == 2)
                {
                    MessageBox.Show($"{players[turnOrder % 2].Name} got a TRIA!");
                    if (players[0].GetType() == typeof(HumanPlayer))
                    {
                        tria = true;
                    }
                }
                else if (totalRowCount == 4 && emptyEndCaps >= 1)
                {
                    MessageBox.Show($"{players[turnOrder % 2].Name} got a TESSERA!");
                    if (players[0].GetType() == typeof(HumanPlayer))
                    {
                        tessera = true;
                    }
                }
            }
            return hasWon;
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

        private void HorizontalLeftCheckCapture(int startPosition, Label sender)
        {
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
                            CaptureRemove(currentPosition, currentPosition - 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
           
        }

        private void HorizontalRightCheckCapture(int startPosition, Label sender)
        {
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
                            CaptureRemove(currentPosition, currentPosition + 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
        }

        private void VerticalDownCheckCapture(int startPosition, Label sender)
        {
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
                            CaptureRemove(currentPosition, currentPosition + (int)BoardSizeSlider.Value);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
        }

        private void VerticalUpCheckCapture(int startPosition, Label sender)
        {
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
                            CaptureRemove(currentPosition, currentPosition - (int)BoardSizeSlider.Value);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
        }
        
        private void DiagonalUpLeftCheckCapture(int startPosition, Label sender)
        {
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
                            CaptureRemove(currentPosition, currentPosition - (int)BoardSizeSlider.Value - 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
        }

        private void DiagonalUpRightCheckCapture(int startPosition, Label sender)
        {
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
                            CaptureRemove(currentPosition, currentPosition - (int)BoardSizeSlider.Value + 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
        }

        private void DiagonalDownLeftCheckCapture(int startPosition, Label sender)
        {
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
                            CaptureRemove(currentPosition, currentPosition + (int)BoardSizeSlider.Value - 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
        }

        private void DiagonalDownRightCheckCapture(int startPosition, Label sender)
        {
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
                            CaptureRemove(currentPosition, currentPosition + (int)BoardSizeSlider.Value + 1);
                            players[(turnOrder-1) % 2].Captures = 1;
                        }
                    }
                }
            }
        }

        private void UpdatePlayerTurn()
        {
            Player1Info.Content = players[0].ToString();
            Player2Info.Content = players[1].ToString();
            if (turnOrder % 2 == 0)
            {
                Player2Info.Background = new SolidColorBrush(Color.FromRgb(63, 140, 208));
                Player1Info.Background = new SolidColorBrush(Color.FromRgb(108, 38, 38));
            }
            else
            {
                Player2Info.Background = new SolidColorBrush(Color.FromRgb(108, 38, 38));
                Player1Info.Background = new SolidColorBrush(Color.FromRgb(63, 140, 208));
            }
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
            player1LastPiece = new Label();
            player1SecondToLastPiece = new Label();
            player2LastPiece = new List<Label>();
            tria = false;
            tessera = false;
            ticks = turnTimer;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BoardCover.Visibility = Visibility.Hidden;
            players[0] = new HumanPlayer(Player1Name.Text);
            if ((bool)AICheckBox.IsChecked)
            {
                players[1] = new AIPlayer();
            }
            else
            {
                players[1] = new HumanPlayer(Player2Name.Text);
            }
            PlayerDetails.Visibility = Visibility.Hidden;
            PlayerTurnOrder.Visibility = Visibility.Visible;
            InitializeBoard();
            timer.Start();
            UpdatePlayerTurn();
            if (players[1].GetType() == typeof(AIPlayer))
            {
                Random rand = new Random();
                Label randLabel = null;
                int randPiece = rand.Next(((int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value) - 1);
                do
                {
                    randPiece = rand.Next(((int)BoardSizeSlider.Value * (int)BoardSizeSlider.Value) - 1);
                    randLabel = (Label)GameBoard.Children[randPiece];
                } while (randLabel.Name == "BlackPiece");

                Label label = new Label();
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
                GameBoard.Children.Cast<Label>().ElementAt(randPiece).Background = brush;
                GameBoard.Children.Cast<Label>().ElementAt(randPiece).Name = "WhitePiece";
                player2LastPiece.Add(randLabel);
                turnOrder++;
            }
        }

        private void Instructions_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.Stop();
            MessageBox.Show("\t\t\t   Pente Rules: \n\nHow to win: \nPlace five or more of your stones in a row, this can horizontally, vertically, or diagonally with no empty points in between the stones. \nYou may also win by capturing five or more pairs of your opponent's stones \n\nHow to make your turn: \nWhen it is your turn you will have twenty seconds to choose a place on the board to place your stone. \nTo place your stone click on a part of the board where the board’s grid intersects as those are the places on the board that represent the spaces. \n\nConditions on placing your stone: \nIf you are player one(the black stones), your first move is made for you. The first stone is placed in the center of the board. \nThen during player one’s second turn they cannot place their second black stone within three intersections of the first black stone. \n\nCaptures: \nTo capture your opponent’s pieces you must find a spot where your opponent has placed two of their pieces next to each other,  no more and no less than two, and place two of your pieces to bracket them in at the ends. The captured pieces are then removed from the board. \nCaptures can occur vertically, horizontally, or diagonally. Multiple captures can also occur with a single turn.");
            timer.Start();
        }

        private void SaveGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.Stop();
            GameModel model = new GameModel();
            foreach (Label name in GameBoard.Children)
            {
                model.boardpieces.Add(name.Name);
            }
            foreach (Player player in players)
            {
                model.players.Add(player);
            }
            model.Player1LastPiece = GameBoard.Children.IndexOf(player1LastPiece);
            model.Player1SecondToLastPiece = GameBoard.Children.IndexOf(player1SecondToLastPiece);
            model.Player2LastPiece = new List<int>();
            foreach (Label name in player2LastPiece)
            {
                model.Player2LastPiece.Add(GameBoard.Children.IndexOf(name));
            }

            model.lastTurn = turnOrder;

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
            timer.Start();
        }

        private void LoadGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer.Stop();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".pente";
            ofd.Filter = "Pente Saves (*.pente)|*.pente";
            GameModel newGame = null;
            if (ofd.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(ofd.FileName, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    newGame = (GameModel)bf.Deserialize(stream);
                }
                LoadGame(newGame);
            }
            timer.Start();
        }

        private void LoadGame(GameModel save)
        {
            Refresh_Executed(null, null);
            turnOrder = save.lastTurn;
            timer.Stop();
            BoardCover.Visibility = Visibility.Hidden;
            PlayerDetails.Visibility = Visibility.Hidden;
            PlayerTurnOrder.Visibility = Visibility.Visible;
            GameBoard.Children.Clear();
            players = save.players.ToArray();
            UpdatePlayerTurn();
            foreach (string name in save.boardpieces)
            {
                Label label = new Label();
                switch (name)
                {
                    case "BlackPiece":
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
                        break;
                    case "WhitePiece":
                        label.Name = "WhitePiece";
                        Uri resourceUri2 = new Uri("Images/WhitePiece.png", UriKind.Relative);
                        StreamResourceInfo streamInfo2 = Application.GetResourceStream(resourceUri2);
                        BitmapFrame temp2 = BitmapFrame.Create(streamInfo2.Stream);
                        var brush2 = new ImageBrush
                        {
                            ImageSource = temp2,
                            Stretch = Stretch.Fill
                        };
                        label.Background = brush2;
                        break;
                    case "RegularTile":
                        label.Name = "RegularTile";
                        Uri resourceUri3 = new Uri("Images/BlankBoard.png", UriKind.Relative);
                        StreamResourceInfo streamInfo3 = Application.GetResourceStream(resourceUri3);
                        BitmapFrame temp3 = BitmapFrame.Create(streamInfo3.Stream);
                        var brush3 = new ImageBrush
                        {
                            ImageSource = temp3,
                            Stretch = Stretch.Fill
                        };
                        label.Background = brush3;
                        break;
                }
                label.MouseLeftButtonDown += Label_MouseLeftButtonDown;
                GameBoard.Children.Add(label);
            }
            player1LastPiece = (Label)GameBoard.Children[save.Player1LastPiece];
            player1SecondToLastPiece = (Label)GameBoard.Children[save.Player1SecondToLastPiece];
            foreach (int index in save.Player2LastPiece)
            {
                if (index >= 0 )
                {
                    player2LastPiece.Add((Label)GameBoard.Children[index]);

                }
            }
            ticks = turnTimer;
            MessageBox.Show("Your game has loaded, the timer will now start!");
            timer.Start();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Player2Name.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Player2Name.Visibility = Visibility.Visible;
        }
    }
}
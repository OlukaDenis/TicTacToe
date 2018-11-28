using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Shapes;

namespace Wpf_Tic_Tac_Toe
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// The client game window
    public partial class Window1 : Window
    {
        private MainWindow Owner;
        private bool isServer;
        private bool isMyTurn = false;
        private int[][] board = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };// 0=neutral, 1=server, 2=client
        private SocketManagement con;
        private string[] mapping = { "", "X", "O" };// 0=netral, 1=server, 2=client
        private bool isFinished = false;
        private bool isWinner = false;
        int counter = 0;
        //part of the network protocol on the client:
        //static TcpClient client;
        //static NetworkStream stream;
        //--------------------------------------------
        public Window1(MainWindow owner, bool isServer, SocketManagement con)
        {
            Loaded += Window1_MyLoaded;
            this.isMyTurn = isServer;
            this.Owner = owner;
            this.isServer = isServer;
            //new ResizeForBorderlessForm(this) { AllowResizeAll = false, AllowMove = true };
            InitializeComponent();
            this.con = con;
        }
        private void ReSetBoard()
        {
            p00.Content = mapping[board[0][0]];
            p01.Content = mapping[board[0][1]];
            p02.Content = mapping[board[0][2]];
            p10.Content = mapping[board[1][0]];
            p11.Content = mapping[board[1][1]];
            p12.Content = mapping[board[1][2]];
            p20.Content = mapping[board[2][0]];
            p21.Content = mapping[board[2][1]];
            p22.Content = mapping[board[2][2]];
        }

        //checking for the winner
        private void CheckBoard()
        {
            
            if (this.Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                // Horizontal win Check
                if (board[0][0] != 0 && board[0][1] != 0 && board[0][2] != 0 && board[0][0] == board[0][1] && board[0][1] == board[0][2] && board[0][0] == board[0][2])
                {
                    
                    // horizontal 0
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) 
                        isWinner = true;
                        WinnerStatus(isWinner);
                }
                else if (board[1][0] != 0 && board[1][1] != 0 && board[1][2] != 0 && board[1][0] == board[1][1] && board[1][1] == board[1][2] && board[1][0] == board[1][2])
                {
                    // horizontal 1
                    isFinished = true;
                    if ((isServer && board[1][0] == 1) || (!isServer && board[1][0] == 2)) 
                        isWinner = true;
                        WinnerStatus(isWinner);
                }
                else if (board[2][0] != 0 && board[2][1] != 0 && board[2][2] != 0 && board[2][0] == board[2][1] && board[2][1] == board[2][2] && board[2][0] == board[2][2])
                {
                    // horizontal 2
                    isFinished = true;
                    if ((isServer && board[2][0] == 1) || (!isServer && board[2][0] == 2)) 
                        isWinner = true;
                        WinnerStatus(isWinner);
                }
                // Vertical win Check
                else if (board[0][0] != 0 && board[1][0] != 0 && board[2][0] != 0 && board[0][0] == board[1][0] && board[1][0] == board[2][0] && board[0][0] == board[2][0])
                {
                    //vertical 0
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) 
                        isWinner = true;
                        WinnerStatus(isWinner);
                }
                else if (board[0][1] != 0 && board[1][1] != 0 && board[2][1] != 0 && board[0][1] == board[1][1] && board[1][1] == board[2][1] && board[0][1] == board[2][1])
                {
                    // vertical 1
                    isFinished = true;
                    if ((isServer && board[0][1] == 1) || (!isServer && board[0][1] == 2)) 
                        isWinner = true;
                        WinnerStatus(isWinner);
                }
                else if (board[0][2] != 0 && board[1][2] != 0 && board[2][2] != 0 && board[0][2] == board[1][2] && board[1][2] == board[2][2] && board[0][2] == board[2][2])
                {
                    // vertical 2
                    isFinished = true;
                    if ((isServer && board[0][2] == 1) || (!isServer && board[0][2] == 2))
                        isWinner = true;
                        WinnerStatus(isWinner);
                }
                // Diagonal Check
                else if (board[0][0] != 0 && board[1][1] != 0 && board[2][2] != 0 && board[0][0] == board[1][1] && board[1][1] == board[2][2] && board[0][0] == board[2][2])
                {
                    // Diogonal left to right
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) 
                        isWinner = true;
                        WinnerStatus(isWinner);
                }
                else if (board[0][2] != 0 && board[1][1] != 0 && board[2][0] != 0 && board[2][0] == board[1][1] && board[1][1] == board[0][2] && board[2][0] == board[0][2])
                {
                    // Diagonal right to left
                    isFinished = true;
                    if ((isServer && board[1][1] == 1) || (!isServer && board[1][1] == 2)) 
                        isWinner = true;
                        WinnerStatus(isWinner);
                }
                if (isFinished)
                {
                    SetEnabled(true);
                    isMyTurn = false;
                    
                    //check draw
                    if (counter == 9)
                    {
                        MessageBox.Show("No winner!!", "Draw", MessageBoxButton.OK, MessageBoxImage.Information);
                        Environment.Exit(0);
                    }

                    if (isWinner)
                    {
                        MessageBox.Show("You Win!!", "Winner", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                    }
                    else
                    {
                        MessageBox.Show("You Lose!!", "Loser", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //int close_game = Int32.Parse (MessageBox.Show("You Lose!!", "Loser", MessageBoxButton.YesNo, MessageBoxImage.Error);)
                    Environment.Exit(0);
                }
               
            }
            //else this.Invoke((MethodInvoker)delegate { CheckBoard(); });
           else this.Dispatcher.BeginInvoke(new Action(delegate
            {
                // Do your work
                CheckBoard();
            }));
        }

        //assigns a color on row, column or diagonal after a win
        private void WinnerStatus(bool status)
        {
            if (board[0][0] == board[0][1] && board[0][1] == board[0][2] && board[0][0] == board[0][2])
            {
                p00.Background = Brushes.Green;
                p01.Background = Brushes.Green;
                p02.Background = Brushes.Green;
                status = true;
            }

            if (board[1][0] == board[1][1] && board[1][1] == board[1][2] && board[1][0] == board[1][2])
            {
                p10.Background = Brushes.Green;
                p11.Background = Brushes.Green;
                p12.Background = Brushes.Green;
                status = true;
            }
            if (board[2][0] == board[2][1] && board[2][1] == board[2][2] && board[2][0] == board[2][2])
            {
                p20.Background = Brushes.Green;
                p21.Background = Brushes.Green;
                p22.Background = Brushes.Green;
                status = true;
            }
            if (board[0][0] == board[1][0] && board[1][0] == board[2][0] && board[0][0] == board[2][0])
            {
                p00.Background = Brushes.Green;
                p10.Background = Brushes.Green;
                p20.Background = Brushes.Green;
                status = true;
            }
            if (board[0][1] == board[1][1] && board[1][1] == board[2][1] && board[0][1] == board[2][1])
            {
                p01.Background = Brushes.Green;
                p11.Background = Brushes.Green;
                p21.Background = Brushes.Green;
                status = true;
            }
            if (board[0][2] == board[1][2] && board[1][2] == board[2][2] && board[0][2] == board[2][2])
            {
                p02.Background = Brushes.Green;
                p12.Background = Brushes.Green;
                p22.Background = Brushes.Green;
                status = true;
            }
            if (board[0][0] == board[1][1] && board[1][1] == board[2][2] && board[0][0] == board[2][2])
            {
                p00.Background = Brushes.Green;
                p11.Background = Brushes.Green;
                p22.Background = Brushes.Green;
                status = true;
            }
            if (board[2][0] == board[1][1] && board[1][1] == board[0][2] && board[2][0] == board[0][2])
            {
                p20.Background = Brushes.Green;
                p11.Background = Brushes.Green;
                p02.Background = Brushes.Green;
                status = true;
            }
        }
        //checking for the player's turn
        private void CheckTurn()
        {
            //if (!this.InvokeRequired)
            if (this.Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                if (isMyTurn && isFinished == false)
                {
                    SetEnabled(true);
                }
                else
                {
                    SetEnabled(false);
                    GetDataFromOthers();
                }
                ReSetBoard();
            }
            //else this.Invoke((MethodInvoker)delegate { CheckTurn(); });
            else 
                this.Dispatcher.BeginInvoke(new Action(delegate
                 {
                     // Do your work
                     CheckTurn();
                 }));
        }

        //enable and disable the board
        private void SetEnabled(bool value)
        {
            p00.IsEnabled = value;
            p01.IsEnabled = value;
            p02.IsEnabled = value;
            p10.IsEnabled = value;
            p11.IsEnabled = value;
            p12.IsEnabled = value;
            p20.IsEnabled = value;
            p21.IsEnabled = value;
            p22.IsEnabled = value;
        }

        private void Window1_MyLoaded(object sender, RoutedEventArgs e)
        {
            //ENTRY INTO APPLICATION:
            // launch a new stream for data acquisition
            Thread receiveThread = new Thread(new ThreadStart(CheckTurn));
            receiveThread.Start();
            
        }

        //get data from the other client
        private void GetDataFromOthers()
        {

            //ClientObject CO = new ClientObject(SocketManagement._CLIENT, null/*SocketManagement.server*/, false, 0);//isMain=false, 
            //and currentCLientID=0, because In this case, we do not need these parameters
            Task.Factory.StartNew(() =>{//your FLOW, there may be a conflict with the stream ABOVE
                try
                {
                    AllData allData = con.getBoard(SocketManagement._CLIENT);
                    board = allData.obj;
                }
                catch (Exception) {
                    isServer = true;
                    CheckTurn();//return to to the begining
                }

                isMyTurn = true;
                CheckBoard();
                CheckTurn();
            });
        }

        private void SetBoardBasedOnButtonName(string code)
        {
            // 0=netral, 1=server, 2=client
            char[] realCodeInChar = code.Substring(1).ToCharArray();
            if (isServer)
            {
                //if server, set X
                board[Int32.Parse("" + realCodeInChar[0])][Int32.Parse("" + realCodeInChar[1])] = 1;
            }
            else
            {
                //if client, set O
                board[Int32.Parse("" + realCodeInChar[0])][Int32.Parse("" + realCodeInChar[1])] = 2;
            }
            //board[Int32.Parse("" + realCodeInChar[0])][Int32.Parse("" + realCodeInChar[1])] = isServer ? 1 : 2;
        }

        private void p_click(object sender, EventArgs e)
        {
            if (isMyTurn && isFinished == false)
            {
                //cast sender to a button
                var button = (Button)sender;
                if ((String)(button).Content == "")
                {                   
                    SetBoardBasedOnButtonName((button).Name);
                    ClientObject CO = new ClientObject(SocketManagement._CLIENT, SocketManagement.server, false, 0);//isMain=false, 
                    //and currentCLientID=0, because In this case, we do not need these parameters
                    con.sendBoard(board, CO);
                    isMyTurn = false;
                    CheckBoard();
                    CheckTurn();
                    counter++;
                    if (!isMyTurn )
                    {
                        button.Foreground = Brushes.Blue;
                    }

                }
                else
                                   
                    MessageBox.Show("No winner", "This is a draw");
            }
        }
        void Window1_Closing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            Environment.Exit(0);

        }
    }
}

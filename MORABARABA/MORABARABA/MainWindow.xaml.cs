using System;
using System.Collections.Generic;
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

namespace MORABARABA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int p1, p2; string mark;
        Point Cows;
        private bool gameover;
        public List<Button> ButtonList;
        Gameplay gameplay;
        private PlayerMark[] positionValue;
        private ClickType click;
        private int PrevIndex;
        private bool PlayerOne;
        private bool LockClick;

        public MainWindow()
        {
            InitializeComponent();
            click = new ClickType();
            gameplay = new Gameplay();
            NewGame();
        }

        private void NewGame()
        {
            gameover = false;
            gameplay.DrawBoard(Board);
            ListButtons();
            AddContent();
            click = ClickType.First;
            Cows = new Point(12, 12);


        }

        private void AddContent()
        {
            positionValue = new PlayerMark[32];

            for (int index = 0; index < 32; index++)
            {
                positionValue[index] = PlayerMark.Empty;
            }
        }

        private void ListButtons()
        {
            List<Button> lstBtns = new List<Button>
            {
                a1, a2, a3,
                b1, b2, b3,
                c1, c2, c3,
                d1, d2, d3,
                e1, e2, e3,
                f1, f2, f3,
                g1, g2, g3,
                h1, h2, h3,
            };

            ButtonList = lstBtns;
            foreach (Button BT in ButtonList)
            {
                BT.Background = Brushes.Silver;
                BT.Content = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var content = (e.Source as Button).Content.ToString();

            gameplay.ValueBind(positionValue);

            double token;

            if (PlayerOne)
            {
                token = Cows.X;
            }
            else
            {
                token = Cows.Y;
            }

            string s = token.ToString();
            switch (click)
            {
                
                case ClickType.First:
                    First(button, content,token);
                    break;
                case ClickType.Second:
                    Second(button, content);
                    break;
                case ClickType.Capture:
                    DoCapture(button, content);
                    CheckWinner();
                    break;
                case ClickType.None:
                    break;
                default:
                    break;

            }

        }

        private void CheckWinner()
        {
            if (Cows.Y == 0)
            {
                int blue = 0, red = 0;
                foreach (PlayerMark pos in positionValue)
                {
                    if (pos == PlayerMark.Blue)
                    {
                        blue = blue + 1;
                    }
                    else if (pos == PlayerMark.Red)
                    {
                        red = red + 1;
                    }
                }

                if (blue < 3 )
                {
                    MessageBox.Show("Player 2 won");
                    NewGame();
                }
                else if (red < 3)
                {
                    MessageBox.Show("Player 1 won");
                    NewGame();
                }
            }
            
        }

        private void Second(Button button, string content)
        {
            int index = ButtonList.IndexOf(button);
            if (gameplay.ValidSecondClick(PrevIndex, index))
            {
                if (PlayerOne)
                {
                    ReversePrev(PrevIndex);
                    button.Background = Brushes.Red;
                    button.Content = "X";
                    Cows.X = Cows.X - 1;
                    positionValue[index] = PlayerMark.Red;

                    gameplay.ValueBind(positionValue);
                    if (gameplay.IsItCapture(index))
                    {
                        click = ClickType.Capture;
                    }
                    else
                    {
                        click = ClickType.First;
                        PlayerOne = false;
                        mark = "O";
                    }
                }
                else
                {
                    ReversePrev(PrevIndex);
                    button.Background = Brushes.Blue;
                    button.Content = "O";
                    Cows.Y = Cows.Y - 1;
                    positionValue[index] = PlayerMark.Blue;

                    gameplay.ValueBind(positionValue);
                    if (gameplay.IsItCapture(index))
                    {
                        click = ClickType.Capture;
                    }
                    else
                    {
                        click = ClickType.First;
                        PlayerOne = true;
                        mark = "X"; 
                    }
                }
            }
        }

        private void ReversePrev(int prevIndex)
        {
            ButtonList[prevIndex].Background = Brushes.Silver;
            ButtonList[prevIndex].Content = "";
            positionValue[prevIndex] = PlayerMark.Empty;

        }

        private void First(Button button,string content, double tokens)
        {
            int index = ButtonList.IndexOf(button);
            if (tokens > 0)
            {
                if (content != "")
                {
                    MessageBox.Show("Not allowed to move pieces whie you still have cows");
                }
                else
                {
                    if (PlayerOne)
                    {
                        button.Background = Brushes.Red;
                        button.Content = "X";
                        Cows.X = Cows.X - 1;
                        positionValue[index] = PlayerMark.Red;

                        gameplay.ValueBind(positionValue);
                        if (gameplay.IsItCapture(index))
                        {
                            click = ClickType.Capture;
                        }
                        else
                        {
                            click = ClickType.First;
                            PlayerOne = false;
                            mark = "O";
                        }                                                
                    }
                    else
                    {
                        button.Background = Brushes.Blue;
                        button.Content = "O";
                        Cows.Y = Cows.Y - 1;                        
                        positionValue[index] = PlayerMark.Blue;

                        gameplay.ValueBind(positionValue);
                        if (gameplay.IsItCapture(index))
                        {
                            click = ClickType.Capture;
                        }
                        else
                        {
                            click = ClickType.First;
                            PlayerOne = true;
                            mark = "X";
                        }
                    }

                }
            }
            else
            {
                if (content != mark)
                {
                    MessageBox.Show("Not player's piece");
                }
                else
                {
                    gameplay.ValueBind(positionValue);
                    if (gameplay.CheckAgainst(index))
                    {
                        button.Background = Brushes.LimeGreen;
                        click = ClickType.Second;
                        PrevIndex = index;
                    }
                    
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void DoCapture(Button button, string content)
        {
            int index = ButtonList.IndexOf(button);

            if (content == "" || content == mark)
            {
                MessageBox.Show("Pick an opponents piece to capture");
            }
            else
            {
                button.Background = Brushes.Silver;
                button.Content = "";
                positionValue[index] = PlayerMark.Empty;

                click = ClickType.First;

                if (PlayerOne)
                {
                    PlayerOne = false;
                    mark = "O";
                }
                else
                {
                    PlayerOne = true;
                    mark = "X";
                }
            }
        }
    }
}

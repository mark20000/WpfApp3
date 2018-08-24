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
using System.Windows.Threading;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string aplphabet = "SPACEabcdefghijklmnopqrstuvwxyz*#";
        List<string> cacheKey = new List<string>();
        Dictionary<string, string> keypairs = new Dictionary<string, string>();
        DispatcherTimer disp = new DispatcherTimer();
        int buttonClick = 0;
        bool timeElapsed;
        string key = null;
        string previousKey = null;
        string content = null;
        int intervalCount = 0;
        

        public MainWindow()
        {
            InitializeComponent();
            load();
        }

        private void checkPrevious(string content)
        {
            key = keypairs.Keys.Where(x => x == content[content.Length - 1].ToString()).FirstOrDefault();

            // previousKey=keypairs.FirstOrDefault(x => x.Value[buttonClick].ToString() == cacheKey[cacheKey.Count - 1]).Key;
            foreach (var values in keypairs.Values)
            {
                if (values.Length > 1)
                {
                    if (values[buttonClick].ToString() == cacheKey[cacheKey.Count - 1])
                    {
                        previousKey = keypairs.Where((x => x.Value == values)).FirstOrDefault().Key;
                    }
                }
            }

        }
        private void load()
        {
            disp.Tick += Disp_Tick;
            string[] buttonText = new string[12];
            KeyPadGrid = new Grid();
            KeyPadGrid.RowDefinitions.Add(new RowDefinition());
            KeyPadGrid.Children.Add(new TextBox());
            DockPanel dockPanel = null;
            int Position = 5;

            for (int i = 0; i < 12; i++)
            {
                if (i < 1)
                {                    
                    buttonText[i] = aplphabet.Substring(0, 5) + "\n" + i;
                    keypairs.Add(i.ToString(), buttonText[i]);
                }
                else if (i == 1)
                {                 
                    buttonText[i] = i.ToString();
                    keypairs.Add(i.ToString(), buttonText[i]);
                }
                else if (i == 7 || i == 9)
                {
                    Position = Position + buttonText[i - 1].Length;
                    Position = Position - 2;
                    
                    buttonText[i] = aplphabet.Substring(Position, 4) + "\n" + i;
                    keypairs.Add(i.ToString(), buttonText[i]);
                }
                else if (i == 2)
                {
                    buttonText[i] = aplphabet.Substring(i + 3, 3) + "\n" + i;
                    keypairs.Add(i.ToString(), buttonText[i]);
                }
                else if (i == 10)
                {
                    buttonText[i] = aplphabet.Substring(aplphabet.Length - 2, 1);
                    keypairs.Add(i.ToString(), buttonText[i]);
                }
                else if (i == 11)
                {
                    buttonText[i] = aplphabet.Substring(aplphabet.Length - 1, 1);
                    keypairs.Add(i.ToString(), buttonText[i]);
                }
                else if (i < 12)
                {
                    Position = Position + buttonText[i - 1].Length;
                    Position = Position - 2;
                    buttonText[i] = aplphabet.Substring(Position, 3) + "\n" + i;
                    keypairs.Add(i.ToString(), buttonText[i]);
                } 
            }
            
            for (int i = 0; i < buttonText.Length; i++)
            {
                string spaceButton = buttonText[0];
                string previousItem = null;

                if (i == 10)
                {
                    break;
                }
                previousItem = buttonText[i + 1];

                if (i < 1)
                {
                    buttonText[i + 1] = spaceButton;
                    buttonText[i] = previousItem;
                }
                else
                {
                    buttonText[i + 1] = buttonText[i];
                    buttonText[i] = previousItem;
                }
            }

           // for (int i = 0; i < keypairs.Count; i++)
           // {
               // keypairs[i.ToString()]= buttonText[i];
           // }

          for (int i = 0; i < 13; i++)
            {
                if (i % 3 == 0)
                {
                    KeyPadGrid.RowDefinitions.Add(new RowDefinition());
                    dockPanel = new DockPanel();
                    KeyPadGrid.Children.Add(dockPanel);
                }

                if (i < 12)
                {
                    dockPanel.Children.Add(new Button() { Height = 100, Width = 200, HorizontalAlignment = HorizontalAlignment.Left, Content = buttonText[i] });
                    foreach (var dock in dockPanel.Children)
                    {
                        DockPanel.SetDock((UIElement)dock, Dock.Left);

                        if (typeof(Button) == dock.GetType())
                        { 
                            ((Button)dock).PreviewMouseDown += keypad_Click;
                            ((Button)dock).PreviewMouseUp += keypadUp_Click;
                            ((Button)dock).Background = Brushes.Black;
                            ((Button)dock).Foreground = Brushes.White;
                        }
                    }
                }
            }

            int counter = 1;
            foreach (var child in KeyPadGrid.Children)
            {
                Grid.SetRow((UIElement)child, counter);
                counter++;
                if ((typeof(TextBox) == child.GetType()))
                {
                    ((TextBox)child).HorizontalAlignment = HorizontalAlignment.Left;
                    ((TextBox)child).FontSize = 30;
                    ((TextBox)child).Width = 600;
                    ((TextBox)child).Height = 150;
                    ((TextBox)child).IsEnabled = false;
                    ((TextBox)child).Foreground = Brushes.Black;
                    ((TextBox)child).Background = Brushes.LightGray;
                }
            }
            Window win = new Window();
            win.Content = KeyPadGrid;
            this.Hide();
            win.Show();
        }

        private void keypadUp_Click(object sender, MouseButtonEventArgs e)
       {
            e.Handled = true;
            string content = ((Button)sender).Content.ToString();

            if (cacheKey.Count > 0) { 
                
                checkPrevious(content);

                    if (key!=previousKey)
                    {
                       // cacheKey.Add(content[0].ToString());
                    }
                    else
                    {                       
                        if (intervalCount <2)
                        {
                            intervalCount = 0;
                            cacheKey[cacheKey.Count - 1] = "";
                        }
                            buttonClick++;

                        if (buttonClick > 2)
                        {
                            buttonClick = 0;
                        }                   
                    }
                }
        }

        private void keypad_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            content = ((Button)sender).Content.ToString();
            disp.Stop();
            timeElapsed = false;

            if (cacheKey.Count == 0)
            {
                disp.Interval = new TimeSpan(0, 0, 0, 0, 100); 
            }
            else
            {
                checkPrevious(content);
                if (key == previousKey)
                {
                    disp.Interval = new TimeSpan(0, 0, 0, 0, 500);
                }               
            }
            disp.Start();
        }

        private void Disp_Tick(object sender, EventArgs e)
        {
            if (intervalCount > 1)
            {
                intervalCount = 0;
            }             
                intervalCount++;
                timeElapsed = true;

            if (content != null)
            {
                if (content.Length > 6 )
                {
                    content = " ";
                    cacheKey.Add(content);
                }
                else if(content.Length == 1)
                {
                    cacheKey.Add(content);
                }
                else
                {
                    cacheKey.Add(content[buttonClick].ToString());
                }
           

                foreach (var child in KeyPadGrid.Children)
                {
                    if ((typeof(TextBox) == child.GetType()))
                    {
                        ((TextBox)child).Text = "";
                        foreach (var letter in cacheKey)
                        {

                            ((TextBox)child).Text = ((TextBox)child).Text + letter;
                        }
                    }
                }
               
                content = null;
            }                        
        }
    }
}

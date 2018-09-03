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
using AutoItX3Lib;
using System.ComponentModel;
using System.Threading;

namespace azur_lane_farming
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AutoItX3 Au3 = new AutoItX3();
        string noxname = "NoxPlayer1";
        bool isStart = true;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        
        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_Dowork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private bool checkValidColor(int t, int l, int r, int b, int[] colors)
        {
            foreach (int color in colors)
            {
                var test1 = Au3.PixelSearch(t, l, r, b, color, 4);
                if (Au3.error != 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void click(string message,int x,int y)
        {
            Console.WriteLine(message);
            Au3.ControlClick(noxname, "", "", "LEFT", 1, x, y);
            Thread.Sleep(1000);
        }

        private void worker_Dowork(object sender, DoWorkEventArgs e)
        {
            while (isStart)
            {
                string currentTime = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
                Console.Write(value: $"{currentTime} ");
                
                //lbl_status.Content = test1.ToString();
     
                if (startGame()) { continue; }
                if (pressToStart()) { continue; }
                if (closeNotice()) { continue; }
                if (pressAttackButton()) { continue; }
                if (enterstage5_1()) { continue; }
                Thread.Sleep(1000);

            }
        }

        

        private bool startGame()
        {
            int t = 131, l = 345, r = 188, b = 394;


            if (checkValidColor(t, l, r, b, new int[] { 0xB6ADBE, 0x171413 , 0xA89EC8 }))
            {
                click("Enter The Game", 162, 369);
                return true;
            }
            return false;
        }

        private bool pressToStart()
        {

            int t = 39, l = 68, r = 248, b = 150;


            if (checkValidColor(t, l, r, b, new int[] { 0x3942BF, 0x3940BD , 0x080842 }))
            {
                click("pressToStart", 496, 245);
                return true;
            }
            return false;


        }

        private bool closeNotice()
        {
            int t = 839, l = 62, r = 867, b = 84;

            
            if (checkValidColor(t, l, r, b, new int[] { 0xFFD747, 0xF7D755})) {
                click("Check notice Screen", 853, 74);
                return true;
            }
            return false;
        }

        private bool pressAttackButton()
        {
            int t = 713, l = 295, r = 882, b = 345;

            if (checkValidColor(t, l, r, b, new int[] { 0xEFB810, 0xD0942C, 0xC05D00, 0xD68200 }))
            {
                click("pressAttackButton", 802, 324);
                return true;
            }
            return false;
        }

        private bool enterstage5_1()
        {

            //if (checkValidColor(155, 271, 201, 313, new int[] { 0x295159, 0xFFD742, 0xF79A00, 0xBAA976 }))
            if (true)
            {
                bool inBattle = true;
                //click("enterstage5_1", 177, 298);
                while (inBattle)
                {
                    if (checkValidColor(737, 359, 752, 369, new int[] { 0x963B03, 0xAA5215, 0x943800, 0x704B1B }))
                    {
                        click("go", 673, 380);                            
                    }
                    if (checkValidColor(796, 377, 807, 388, new int[] { 0x943800, 0xCE9B34, 0x974B02, 0x923700 }))
                    {
                        click("Select Fleet", 739, 400);
                        
                    }
                    Console.WriteLine("Before Battle");
                    int battle_count = 0;
                    while (battle_count < 3)
                    {
                        var coord = Au3.PixelSearch(149, 115, 924, 523, 0xD4B010, 4);
                        if (Au3.error == 0)
                        {
                            Console.WriteLine("found small fleet");

                            var coord2 = Au3.PixelSearch(coord[0], coord[1], coord[0] + 30, coord[1]+30, 0xD6C310, 4);
                            if (Au3.error == 0)
                            {
                                click("Enter The Battle Round " + (battle_count+1), coord2[0], coord2[1]);
                                bool waitForClickBattle = true;
                                while(waitForClickBattle)
                                {
                                    Console.WriteLine("waitForClickBattle");
                                    if (checkValidColor(687, 467, 752, 512, new int[] { 0xE77D6B, 0x824646, 0xAC7C59, 0xFEE4AE }))
                                    {
                                        click("Click Battle", 777, 488);
                                        waitForClickBattle = false;
                                    }
                                    Thread.Sleep(1000);
                                }
                                bool isFinish = false;
                                while (!isFinish)
                                {
                                    Console.WriteLine("waitForFinishBattle");
                                    if (checkValidColor(25, 267, 117, 299, new int[] { 0xA56910 }) && checkValidColor(831, 267, 920, 300, new int[] { 0xA56910 }))
                                    {
                                        click("Finish Battle", 476, 320);
                                        isFinish = true;
                                    }
                                        
                                }

                                bool isConfirm = false;
                                while (!isConfirm)
                                {
                                    if (checkValidColor(829, 465, 859, 484, new int[] { 0xE7DFDE , 0xACAAAB, 0x555655 , 0xDEDCDE }))
                                    {
                                        click("Finish Battle", 806, 468);
                                        isConfirm = true;
                                        battle_count += 1;
                                    }

                                }

                            }
                            
                        }
                        else
                        {
                            Console.WriteLine(Au3.error);
                        }
                        Thread.Sleep(1000);
                    }


                    inBattle = false;
                }
                Console.WriteLine("End Battle");
                return true;

            }
            return false;
        }







        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            Au3.AutoItSetOption("CaretCoordMode", 2);
            Au3.AutoItSetOption("MouseCoordMode", 2);
            Au3.AutoItSetOption("PixelCoordMode", 2);

            Au3.WinGetHandle(noxname);
            Au3.WinActivate(noxname);

            if (Au3.WinActive(noxname) == 1)
            {
                lbl_status.Content = "nox is active";

                if (!worker.IsBusy)
                {
                    isStart = true;
                    worker.RunWorkerAsync();
                }
                
            }
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            isStart = false;
        }
    }
}

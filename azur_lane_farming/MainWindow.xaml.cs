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
            battle();
            return true;
            if (checkValidColor(155, 271, 201, 313, new int[] { 0x295159, 0xFFD742, 0xF79A00, 0xBAA976 }))
            //if (true)
            {
                bool inBattle = true;
                click("enterstage5_1", 177, 298);
                while (inBattle)
                {
                    if (checkValidColor(737, 359, 752, 369, new int[] { 0x963B03, 0xAA5215, 0x943800, 0x704B1B }))
                    {
                        click("go", 673, 380);
                    }
                    if (checkValidColor(796, 377, 807, 388, new int[] { 0x943800, 0xCE9B34, 0x974B02, 0x923700 }))
                    {
                        click("Select Fleet", 739, 400);
                        battle();
                        return true;
                    }



                }
            }
            return false;
        }

        void battle()
        {
                Console.WriteLine("Before Battle");
                int battle_count = 0;
                while (true)
                {
                    //boss node
                    if (findNode(new int[] { 0x312831 , 0xFF4D4A }))
                    {
                        Console.WriteLine("End Battle");
                        return;
                    }
                    //small fleet
                    findNode(new int[] { 0xD6C518, 0xD6C518 });
                    //medium fleet
                    findNode(new int[] { 0xDEAA00, 0xE7A600 });
                    //large fleet
                    findNode(new int[] { 0xBD3000, 0xB63300 });

                }

            
            
        }
        

        bool findNode(int[] colors)
        {
            int startCoordX = 88;
            int startCoordY = 99;
            while(!checkValidColor(687, 467, 752, 512, new int[] { 0xE77D6B, 0x824646, 0xAC7C59, 0xFEE4AE }))
            {
                
                Console.WriteLine("Find Node");
                if (startCoordY > 523)
                {
                    return false;
                }
                var nodeCoord = Au3.PixelSearch(startCoordX, startCoordY, 924, 523, colors[0], 4);
                if (Au3.error == 0)
                {
                    Console.WriteLine("Founded First Color" + nodeCoord[0] + ":" + nodeCoord[1]);
                    var coord = Au3.PixelSearch(nodeCoord[0] - 30, nodeCoord[1] - 30, nodeCoord[0] + 30, nodeCoord[1] + 30, colors[1], 4);
                    if (Au3.error == 0)
                    {
                        Console.WriteLine("found node " + nodeCoord[0] + ":" + nodeCoord[1]);
                        click("Enter The Battle", coord[0] + 50, coord[1] + 50);
                        Thread.Sleep(5000);
                        startCoordX = 88;
                        startCoordY = 99;
                        if (checkValidColor(594, 290, 732, 328, new int[] { 0xC3C1C3, 0xFFFFFF, 0xE4E2E4, 0xC2BEBD }))
                        {
                            click("Evade", 656, 306) ;
                        }

                    }
                    else
                    {
                        

                        if (nodeCoord[1] == startCoordY)
                        {
                            startCoordX = nodeCoord[0] + 30;
                        }
                        else
                        {
                            startCoordX = 88;
                            startCoordY = nodeCoord[1] + 30;
                        }
                        
                        Console.WriteLine("set start x to " + startCoordX + ":" + startCoordY);
                    }



                }
                else
                {
                    return false;
                }
            }
            Console.WriteLine("waitForClickBattle");
            click("Click Battle", 777, 488);
            Thread.Sleep(1000);
            isStart = true;

            bool isFinish = false;
            while (!isFinish)
            {
                Console.WriteLine("waitForClickBattle");
                if (checkValidColor(125, 363, 848, 397, new int[] { 0x4BCE32}))
                {
                    click("Finish Battle", 467, 422);
                    Thread.Sleep(3000);
                    click("Finish Battle", 467, 422);
                    Thread.Sleep(3000);
                    click("Finish Battle", 467, 422);
                    isFinish = true;
                }

            }

            bool isConfirm = false;
            while (!isConfirm)
            {
                Console.WriteLine("waitForConfirm");
                if (checkValidColor(829, 465, 859, 484, new int[] { 0xE7DFDE, 0xACAAAB, 0x555655, 0xDEDCDE }))
                {
                    click("Finish Battle", 806, 468);
                    isConfirm = true;
                    return true;
                }

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

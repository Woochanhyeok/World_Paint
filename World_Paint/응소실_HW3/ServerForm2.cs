using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using ClassLibrary1;
using System.Collections;
using System.IO;

namespace 응소실_HW3
{
    public partial class ServerForm2 : Form
    {
        ServerForm1 serverform1;
        int PORT = 0;
        string IP;
        int count = 0;                  //client 수
        public ServerForm2()
        {
            InitializeComponent();
            
            
        }
        string[] saved = new string[1024 * 10];
        public ServerForm2(ServerForm1 form, string ip, int port)
        {
            InitializeComponent();
            serverform1 = form;
            IP = ip;
            PORT = port;
            SetupVar();

            int index_A = 0, index_R = 0, index_G = 0, index_B = 0;
            int result_A = 0, result_R = 0, result_G = 0, result_B = 0;
            int length_A = 0, length_R = 0, length_G = 0, length_B = 0;
            //==================서버 정보에서 불러오기======================
            try
            {
                saved = File.ReadAllLines(@"./server_shapes.txt");          //현재 경로 응소실_HW3/응소실_HW3/bin/debug
                for (int i = 0; i < (saved.Length/8); i++)
                {
                    
                    string[] a = saved[8 * i].Split(',');
                    string[] b = a[0].Split('=');
                    string[] c = a[1].Split('=');
                    
                    string[] q = saved[(8 * i) + 1].Split(',');
                    string[] w = q[0].Split('=');
                    string[] e = q[1].Split('=');

                    Start_Finish start_finishClass = new Start_Finish();

                    start_finishClass.Type= (int)PacketType.그림;
                    //좌표는 {X=?,Y=?} 형태이므로 ,로 반반 쪼개고 =으로 쪼갠다 Y는 }도 빼줘야하므로 Substring으로 길이 -1 만큼 잘라줌
                    start_finishClass.start.X = Convert.ToInt32(b[1]);
                    start_finishClass.start.Y = Convert.ToInt32(c[1].Substring(0, c[1].Length - 1));// here
                    start_finishClass.finish.X = Convert.ToInt32(w[1]);
                    start_finishClass.finish.Y= Convert.ToInt32(e[1].Substring(0, e[1].Length - 1));// here

                    start_finishClass.thick = Convert.ToInt32(saved[8 * i + 2]);
                    start_finishClass.isFilled = Convert.ToBoolean(saved[8 * i + 3]);
                    //색상은 Color [Black]   OR   Color [A=?,R=?,G=?,B=?] 이 형태이므로
                    //A,R,G,B의 인덱스 값을 저장하고 각각의 길이를 인덱스 뺄셈을 이용하여 구하고
                    //Substring으로 색상값들만 잘라준다.
                    if (saved[(8 * i) + 4].IndexOf('=') == -1)      //색상 값이 이름일 때
                    {
                        string[] z = saved[(8 * i) + 4].Split('[');
                        string result = z[1].Substring(0, z[1].Length-1);
                        start_finishClass.penColor = Color.FromName(result);
                    }
                    else                                            //색상 값이 ARGB일 때
                    {
                        index_A = saved[(8 * i) + 4].IndexOf('A');
                        index_R = saved[(8 * i) + 4].IndexOf('R');
                        index_G = saved[(8 * i) + 4].IndexOf('G');
                        index_B = saved[(8 * i) + 4].IndexOf('B');
                        length_A = index_R - index_A - 4;
                        length_R = index_G - index_R - 4;
                        length_G = index_B - index_G - 4;
                        length_B = saved[(8 * i) + 4].Length - index_B - 3;
                        result_A = Convert.ToInt32(saved[(8 * i) + 4].Substring(index_A + 2, length_A));
                        result_R = Convert.ToInt32(saved[(8 * i) + 4].Substring(index_R + 2, length_R));
                        result_G = Convert.ToInt32(saved[(8 * i) + 4].Substring(index_G + 2, length_G));
                        result_B = Convert.ToInt32(saved[(8 * i) + 4].Substring(index_B + 2, length_B));
                        //MessageBox.Show(result_A.ToString() + " " + result_R.ToString() + " " + result_G.ToString() + " " + result_B.ToString());
                        start_finishClass.penColor = Color.FromArgb(result_A,result_R,result_G,result_B);
                    }
                    
                    if (saved[(8 * i) + 5].IndexOf('=') == -1)      //색상 값이 이름일 때
                    {
                        string[] z = saved[(8 * i) + 5].Split('[');
                        string result = z[1].Substring(0, z[1].Length - 1);             //두번째 도형에서 이부분을 못 들어옴
                        start_finishClass.insideColor = Color.FromName(result);
                    }
                    else                                            //색상 값이 ARGB일 때
                    {
                        index_A = saved[(8 * i) + 5].IndexOf('A');      //A의 인덱스
                        index_R = saved[(8 * i) + 5].IndexOf('R');      //R의 인덱스
                        index_G = saved[(8 * i) + 5].IndexOf('G');      //G의 인덱스
                        index_B = saved[(8 * i) + 5].IndexOf('B');
                        length_A = index_R - index_A - 4;
                        length_R = index_G - index_R - 4;
                        length_G = index_B - index_G - 4;
                        length_B = saved[(8 * i) + 5].Length - index_B - 3;
                        
                        result_A = Convert.ToInt32(saved[(8 * i) + 5].Substring(index_A + 2, length_A));
                        result_R = Convert.ToInt32(saved[(8 * i) + 5].Substring(index_R + 2, length_R));
                        result_G = Convert.ToInt32(saved[(8 * i) + 5].Substring(index_G + 2, length_G));
                        result_B = Convert.ToInt32(saved[(8 * i) + 5].Substring(index_B + 2, length_B));
                        //MessageBox.Show(result_A.ToString() + " " + result_R.ToString() + " " + result_G.ToString() + " " + result_B.ToString());

                        start_finishClass.insideColor = Color.FromArgb(result_A, result_R, result_G, result_B);
                    }

                    start_finishClass.drawType = Convert.ToInt32(saved[8 * i + 6]);
                    start_finishClass.num = Convert.ToInt32(saved[8 * i + 7]);
                    //저장한 그림들 drawType에 따라 그리기
                    if (start_finishClass.drawType == 1)
                    {
                        mylines[nline].setPoint(start_finishClass.start, start_finishClass.finish, start_finishClass.thick, start_finishClass.penColor, start_finishClass.num);
                        nline++;
                        drawTypes[num] = 1;
                        num++;
                    }
                    if (start_finishClass.drawType == 2)
                    {
                        myrect[nrect].setRect(start_finishClass.start, start_finishClass.finish, start_finishClass.thick, start_finishClass.isFilled, start_finishClass.penColor, start_finishClass.insideColor, start_finishClass.num);
                        nrect++;
                        drawTypes[num] = 2;
                        num++;
                    }
                    if (start_finishClass.drawType == 3)
                    {
                        mycircle[ncircle].setRectC(start_finishClass.start, start_finishClass.finish, start_finishClass.thick, start_finishClass.isFilled, start_finishClass.penColor, start_finishClass.insideColor, start_finishClass.num);
                        ncircle++;                      //start, finish, thick, isFilled, pencolor, insidecolor
                        drawTypes[num] = 3;
                        num++;
                    }
                    if (start_finishClass.drawType == 4)
                    {
                        mypencil[npencil].setPoint(start_finishClass.start, start_finishClass.finish, start_finishClass.thick, start_finishClass.penColor, start_finishClass.num);
                        npencil++;
                        drawTypes[num] = 4;
                        num++;
                    }
                }
            }
            catch
            {
                
            }
            panel1.Invalidate(true);
            panel1.Update();
        }
        //====================================================
        private bool line;
        private bool rect;
        private bool circle;
        private bool pencil;
        private bool hand;
        private Point start;
        private Point finish;
        private Point nowPoint;
        private Pen pen;
        private SolidBrush brush;
        private int nline;
        private int nrect;
        private int ncircle;
        private int npencil;
        private int i;
        private int num;
        private int thick;
        private bool isFilled;
        private MyLines[] mylines;
        private MyRect[] myrect;
        private MyCircle[] mycircle;
        private MyPencil[] mypencil;
        private int[] drawTypes;
       

        private void SetupVar()
        {
            num = 0;
            i = 0;
            thick = 1;
            isFilled = false;
            line = false;
            rect = false;
            circle = false;
            pencil = false;
            hand = false;
            start = new Point(0, 0);
            finish = new Point(0, 0);
            nowPoint = new Point(0, 0);
            pen = new Pen(Color.Black);
            brush = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
            mylines = new MyLines[100];
            myrect = new MyRect[100];
            mycircle = new MyCircle[100];
            mypencil = new MyPencil[1024 * 4];
            drawTypes = new int[1024 * 10];
            
            
            nline = 0;
            nrect = 0;
            ncircle = 0;
            npencil = 0;
            SetupMine();
        }
        private void SetupMine()
        {
            for (i = 0; i < 100; i++)
            {
                mylines[i] = new MyLines();
            }
            for (i = 0; i < 100; i++)
            {
                myrect[i] = new MyRect();
            }
            for (i = 0; i < 100; i++)
            {
                mycircle[i] = new MyCircle();
            }
            for (i = 0; i < 1024*4; i++)
            {
                mypencil[i] = new MyPencil();
            }
        }

        //====================================================
        private NetworkStream m_networkstream;
        private TcpListener m_listener;
        private byte[] sendBuffer = new byte[1024 * 4];
        private byte[] readBuffer = new byte[1024 * 4];
        private byte[] testBuffer = new byte[1024 * 4];

        private bool m_bClientOn = false;

        private Thread m_thread;
        private Thread client_thread;

        public Initialize m_initializeClass;
        public Start_Finish m_start_finishClass;
        public List<TcpClient> clients = new List<TcpClient>();         //클라이언트들 담을 배열
        public string PATH;
        

        private byte[] StringToByte(string str)
        {
            byte[] StrByte = Encoding.UTF8.GetBytes(str); return StrByte;
        }

        public void SendAll(int num)                       //clients의 index가 num인 클라이언트 빼고 나머지에 전송
        {
            int nRead = 0;
            NetworkStream networkStream;
            for (int j = 0; j < count; j++)                           
            {
                if (j == num)
                {
                    continue;
                }
                try
                {
                    networkStream = clients[j].GetStream();
                    networkStream.Write(this.sendBuffer, 0, this.sendBuffer.Length);
                    networkStream.Flush();
                }
                catch
                {
                    continue;
                }
            }
            for (int i = 0; i < 1024 * 4; i++)
            {
                this.sendBuffer[i] = 0;
            }
        }

        public void Client_Connect()                        //클라이언트의 접속을 기다리는 메소드
        {
            //                  접속을 기다리는거랑 메시지를 받는거를 쓰레드로 각각
            Thread.Sleep(500);
            while (true)
            {
                clients.Add(this.m_listener.AcceptTcpClient());
                if (clients[count].Connected)
                    {
                        NetworkStream networkStream;
                        this.m_bClientOn = true;
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            this.textBox1.AppendText("클라이언트 접속\r\n");
                        }));
                        
                    //==========접속한 클라이언트에 서버 그림 정보 전송
                    if (num != 0)
                    {
                        int n1 = 0, n2 = 0, n3 = 0, n4 = 0;
                        for (int i = 0; i <= num; i++)
                        {
                            Start_Finish point = new Start_Finish();
                            point.Type = (int)PacketType.그림;
                            if (drawTypes[i] == 1)
                            {
                                point.start = mylines[n1].getPoint1();
                                point.finish = mylines[n1].getPoint2();
                                point.thick = mylines[n1].getThick();
                                point.penColor = mylines[n1].penColor;
                                point.insideColor = mylines[n1].insideColor;
                                point.drawType = 1;
                                point.num = i;
                                n1++;
                            }
                            else if (drawTypes[i] == 2)
                            {
                                point.start.X = myrect[n2].getRect().X;
                                point.start.Y = myrect[n2].getRect().Y;
                                point.finish.X = myrect[n2].getRect().X + myrect[n2].getRect().Width;
                                point.finish.Y = myrect[n2].getRect().Y + myrect[n2].getRect().Height;
                                point.thick = myrect[n2].getThick();
                                point.penColor = myrect[n2].penColor;
                                point.insideColor = myrect[n2].insideColor;
                                point.isFilled = myrect[n2].getFilled();
                                point.drawType = 2;
                                point.num = i;
                                n2++;
                            }
                            else if (drawTypes[i] == 3)
                            {                                                     //========사각형이랑 원 start,finish 좌표 다시 보기!!!!
                                point.start.X = mycircle[n3].getRectC().X;
                                point.start.Y = mycircle[n3].getRectC().Y;
                                point.finish.X = mycircle[n3].getRectC().X + mycircle[n3].getRectC().Width;
                                point.finish.Y = mycircle[n3].getRectC().Y + mycircle[n3].getRectC().Height;
                                point.thick = mycircle[n3].getThick();
                                point.penColor = mycircle[n3].penColor;
                                point.insideColor = mycircle[n3].insideColor;
                                point.isFilled = mycircle[n3].getFilled();
                                point.drawType = 3;
                                point.num = i;
                                n3++;
                            }
                            else
                            {
                                point.start = mypencil[n4].getPoint1();
                                point.finish = mypencil[n4].getPoint2();
                                point.thick = mypencil[n4].getThick();
                                point.penColor = mypencil[n4].penColor;
                                point.insideColor = mypencil[n4].insideColor;
                                point.drawType = 4;
                                point.num = i;
                                n4++;
                            }
                            Packet.Serialize(point).CopyTo(this.sendBuffer, 0);
                            networkStream = clients[count].GetStream();
                            networkStream.Write(this.sendBuffer, 0, this.sendBuffer.Length);
                            networkStream.Flush();
                        }

                        for (int j = 0; j < 1024 * 4; j++)
                        {
                            this.sendBuffer[j] = 0;
                        }
                    }
                    //=================================================


                    count++;
                }
            }
        }

        public void RUN()
        {
            this.m_listener = new TcpListener(PORT);
            this.m_listener.Start();

            int nRead = 0;

            while (true)
                {                   
                    if (this.m_bClientOn)
                    {
                        for (int i = 0; i < count; i++)
                        {
                        try
                        {
                            m_networkstream = clients[i].GetStream();
                        }
                        catch
                        {
                            continue;
                        }
                        if (!m_networkstream.DataAvailable)               //보낸 것이 없는 클라이언트는 넘어감
                            continue;

                            try
                            {
                                    nRead = 0;
                                    nRead = this.m_networkstream.Read(readBuffer, 0, 1024 * 4);
                            }
                            catch
                            {
                                    this.m_bClientOn = false;
                                    this.m_networkstream = null;
                            }

                        Packet packet = (Packet)Packet.Desserialize(this.readBuffer);
                                switch ((int)packet.Type)
                                {

                                    case (int)PacketType.채팅:
                                        {
                                            this.m_initializeClass =
                                                (Initialize)Packet.Desserialize(this.readBuffer);
                                            this.Invoke(new MethodInvoker(delegate ()
                                            {
                                                this.textBox1.AppendText(this.m_initializeClass.ID + ":" + this.m_initializeClass.Message + "\r\n");
                                            }));
                                            Send(i,1);                 //i빼고 전송
                                            break;
                                        }
                                    case (int)PacketType.그림:
                                        {
                                            this.m_start_finishClass =
                                                (Start_Finish)Packet.Desserialize(this.readBuffer);
                                            if (m_start_finishClass.start == m_start_finishClass.finish)
                                                break;
                                            if (m_start_finishClass.drawType == 1)
                                            {
                                                mylines[nline].setPoint(m_start_finishClass.start, m_start_finishClass.finish, m_start_finishClass.thick, m_start_finishClass.penColor, m_start_finishClass.num);
                                                nline++;
                                                drawTypes[num] = 1;
                                                num++;
                                            }
                                            if (m_start_finishClass.drawType == 2)
                                            {
                                                myrect[nrect].setRect(m_start_finishClass.start, m_start_finishClass.finish, m_start_finishClass.thick,m_start_finishClass.isFilled , m_start_finishClass.penColor, m_start_finishClass.insideColor, m_start_finishClass.num);
                                                nrect++;
                                                drawTypes[num] = 2;
                                                num++;
                                            }
                                            if (m_start_finishClass.drawType == 3)
                                            {
                                                mycircle[ncircle].setRectC(m_start_finishClass.start, m_start_finishClass.finish, m_start_finishClass.thick, m_start_finishClass.isFilled, m_start_finishClass.penColor, m_start_finishClass.insideColor, m_start_finishClass.num);
                                                ncircle++;                      //start, finish, thick, isFilled, pencolor, insidecolor
                                                drawTypes[num] = 3;
                                                num++;
                                            }
                                            if (m_start_finishClass.drawType == 4)
                                            {
                                                mypencil[npencil].setPoint(m_start_finishClass.start, m_start_finishClass.finish, m_start_finishClass.thick, m_start_finishClass.penColor, m_start_finishClass.num);
                                                npencil++;                      
                                                drawTypes[num] = 4;
                                                num++;
                                            }
                                    //=============받는 정보들 txt로 저장==============
                                        string[]lines=
                                    { m_start_finishClass.start.ToString(), m_start_finishClass.finish.ToString(),
                                    m_start_finishClass.thick.ToString(), m_start_finishClass.isFilled.ToString(),
                                    m_start_finishClass.penColor.ToString(),m_start_finishClass.insideColor.ToString(),
                                    m_start_finishClass.drawType.ToString(), m_start_finishClass.num.ToString()};

                                    using (StreamWriter outputFile = new StreamWriter(@"./server_shapes.txt",true))
                                            {
                                                foreach (string line in lines)
                                                {
                                                    outputFile.WriteLine(line);
                                                }
                                            }
                                    //=================================================


                                            panel1.Invalidate(true);
                                            panel1.Update();
                                            Send(i, 2);

                                    break;
                                        }
                                }
                            
                        }
                    }
                }
        }

        public void Send(int i, int num)                //i빼고 나머지에 전송,num이 1이면 채팅 2면 그림    switch문으로 채팅, 그림판 전송 선택
        {
            switch (num)
            {
                case 1:
                    {
                        Initialize initialize = new Initialize();
                        initialize.Type = (int)PacketType.채팅;
                        initialize.ID = this.m_initializeClass.ID;
                        initialize.Message = this.m_initializeClass.Message;
                        Packet.Serialize(initialize).CopyTo(this.sendBuffer, 0);
                        break;
                    }
                case 2:
                    {
                        Start_Finish point = new Start_Finish();
                        point.Type = (int)PacketType.그림;
                        point.start = m_start_finishClass.start;
                        point.finish = m_start_finishClass.finish;
                        point.thick = m_start_finishClass.thick;
                        point.penColor = m_start_finishClass.penColor;
                        point.insideColor = m_start_finishClass.insideColor;
                        point.isFilled = m_start_finishClass.isFilled;
                        point.drawType = m_start_finishClass.drawType;
                        point.num = m_start_finishClass.num;
                        Packet.Serialize(point).CopyTo(this.sendBuffer, 0);                     //이 부분에서 보내야 함
                        break;
                    }
            }
            
            this.SendAll(i);
        }

        private void ServerForm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_listener.Stop();
            this.m_networkstream.Close();
            this.m_thread.Abort();
            this.client_thread.Abort();
        }

        private void ServerForm2_Load(object sender, EventArgs e)
        {
            this.client_thread = new Thread(new ThreadStart(Client_Connect));
            this.m_thread = new Thread(new ThreadStart(RUN));
            this.client_thread.Start();
            this.m_thread.Start();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int n1 = 0;
            int n2 = 0;
            int n3 = 0;
            int n4 = 0;
            for (int i = 0; i <= nline + nrect + ncircle + npencil; i++)
            {
                if (drawTypes[i] == 1)
                {
                    pen.Color = mylines[n1].penColor;
                    pen.Width = mylines[n1].getThick();
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                    e.Graphics.DrawLine(pen, mylines[n1].getPoint1(), mylines[n1].getPoint2());
                    n1++;
                }
                else if (drawTypes[i] == 2)
                {
                    pen.Color = myrect[n2].penColor;
                    brush.Color = myrect[n2].insideColor;
                    pen.Width = myrect[n2].getThick();
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                    e.Graphics.DrawRectangle(pen, myrect[n2].getRect());
                    if (myrect[n2].getFilled())
                        e.Graphics.FillRectangle(brush, myrect[n2].getRect());
                    n2++;
                }
                else if (drawTypes[i] == 3)
                {
                    pen.Color = mycircle[n3].penColor;
                    pen.Width = mycircle[n3].getThick();
                    brush.Color = mycircle[n3].insideColor;
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                    e.Graphics.DrawEllipse(pen, mycircle[n3].getRectC());
                    if (mycircle[n3].getFilled())
                        e.Graphics.FillEllipse(brush, mycircle[n3].getRectC());
                    n3++;
                }
                else
                {
                    pen.Color = mypencil[n4].penColor;
                    pen.Width = mypencil[n4].getThick();
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                    e.Graphics.DrawLine(pen, mypencil[n4].getPoint1(), mypencil[n4].getPoint2());
                    n4++;
                }
            }
        }

        //====================================================


    }
}

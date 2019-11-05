using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1;
using System.Threading;
using System.Collections;

namespace 응소실_HW3_Client
{
    
    public partial class ClientForm2 : Form
    {
        
        ClientForm1 clientform1;
        string IP;
        int PORT = 0;
        string ID;

        public ClientForm2()
        {
            InitializeComponent();
            SetupVar();
        }
        public ClientForm2(ClientForm1 form, string ip, int port, string id)
        {
            InitializeComponent();
            clientform1 = form;
            IP = ip;
            PORT = port;
            ID = id;
            SetupVar();
            this.MouseWheel += new MouseEventHandler(Mouse_Wheel);
        }
        public int wheel_count;
        public double ratio;
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
        public ArrayList paints;

        private void SetupVar()
        {
            wheel_count = 0;
            ratio = 1.0;
            i = 0;
            num = 0;
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
            paints = new ArrayList();
            

            nline = 0;
            nrect = 0;
            ncircle = 0;
            npencil = 0;
            toolStripMenuItem1.Checked = false;
            toolStripMenuItem2.Checked = true;
            toolStripMenuItem3.Checked = false;
            toolStripMenuItem4.Checked = false;

            SetupMine();
        }
        private void SetupMine()
        {
            for (i=0; i<100; i++)
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
            for (i = 0; i < 1024 * 4; i++)
            {
                mypencil[i] = new MyPencil();
            }
        }

        //================첫 번째 ToolStripMenu================

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            line = true;
            rect = false;
            circle = false;
            pencil = false;
            hand = false;
            this.lineToolStripMenuItem.Checked= true;
            this.rectToolStripMenuItem.Checked = false;
            this.circleToolStripMenuItem.Checked = false;
            this.pencilToolStripMenuItem.Checked = false;
            this.handToolStripMenuItem.Checked = false;
            toolStripDropDownButton1.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/직선.jpg");
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            line = false;
            rect = false;
            circle = true;
            pencil = false;
            hand = false;

            this.lineToolStripMenuItem.Checked = false;
            this.rectToolStripMenuItem.Checked = false;
            this.circleToolStripMenuItem.Checked = true;
            this.pencilToolStripMenuItem.Checked = false;
            this.handToolStripMenuItem.Checked = false;
            toolStripDropDownButton1.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/원.jpg");
        }

        private void rectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            line = false;
            rect = true;
            circle = false;
            pencil = false;
            hand = false;

            this.lineToolStripMenuItem.Checked = false;
            this.rectToolStripMenuItem.Checked = true;
            this.circleToolStripMenuItem.Checked = false;
            this.pencilToolStripMenuItem.Checked = false;
            this.handToolStripMenuItem.Checked = false;
            toolStripDropDownButton1.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/사각형.jpg");
        }
        private void pencilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            line = false;
            rect = false;
            circle = false;
            pencil = true;
            hand = false;

            this.lineToolStripMenuItem.Checked = false;
            this.rectToolStripMenuItem.Checked = false;
            this.circleToolStripMenuItem.Checked = false;
            this.pencilToolStripMenuItem.Checked = true;
            this.handToolStripMenuItem.Checked = false;
            toolStripDropDownButton1.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/연필.jpg");
        }

        private void handToolStripMenuItem_Click(object sender, EventArgs e)
        {
            line = false;
            rect = false;
            circle = false;
            pencil = false;
            hand = true;

            this.lineToolStripMenuItem.Checked = false;
            this.rectToolStripMenuItem.Checked = false;
            this.circleToolStripMenuItem.Checked = false;
            this.pencilToolStripMenuItem.Checked = false;
            this.handToolStripMenuItem.Checked = true;
            toolStripDropDownButton1.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/손.png");
        }



        //=========================두 번째 ToolStripMenu==================

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            thick = 1;

            this.toolStripMenuItem1.Checked = true;
            this.toolStripMenuItem2.Checked = false;
            this.toolStripMenuItem3.Checked = false;
            this.toolStripMenuItem4.Checked = false;
            this.toolStripMenuItem5.Checked = false;
            toolStripDropDownButton2.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/line1Button.JPG");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            thick = 2;

            this.toolStripMenuItem1.Checked = false;
            this.toolStripMenuItem2.Checked = true;
            this.toolStripMenuItem3.Checked = false;
            this.toolStripMenuItem4.Checked = false;
            this.toolStripMenuItem5.Checked = false;
            toolStripDropDownButton2.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/line2Button.JPG");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            thick = 3;

            this.toolStripMenuItem1.Checked = false;
            this.toolStripMenuItem2.Checked = false;
            this.toolStripMenuItem3.Checked = true;
            this.toolStripMenuItem4.Checked = false;
            this.toolStripMenuItem5.Checked = false;
            toolStripDropDownButton2.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/line3Button.JPG");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            thick = 4;

            this.toolStripMenuItem1.Checked = false;
            this.toolStripMenuItem2.Checked = false;
            this.toolStripMenuItem3.Checked = false;
            this.toolStripMenuItem4.Checked = true;
            this.toolStripMenuItem5.Checked = false;
            toolStripDropDownButton2.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/line4Button.JPG");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            thick = 5;

            this.toolStripMenuItem1.Checked = false;
            this.toolStripMenuItem2.Checked = false;
            this.toolStripMenuItem3.Checked = false;
            this.toolStripMenuItem4.Checked = false;
            this.toolStripMenuItem5.Checked = true;
            toolStripDropDownButton2.Image = System.Drawing.Image.FromFile("../../../응소실_HW3_Client/Resources/line5Button.JPG");
        }

        //=======================panel 이벤트=========================

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (hand == true)
            {
                Graphics g = e.Graphics;
                g.ScaleTransform((float)ratio, (float)ratio);

            }
            int n1 = 0;
            int n2 = 0;
            int n3 = 0;
            int n4 = 0;
            for(int i = 0; i <= num; i++)
            {
                if(drawTypes[i] == 1)
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
            
            pen.Width = thick;
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }

        private void panel1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            start.X = e.X;
            start.Y = e.Y;
            finish.X = e.X;
            finish.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
            if ((start.X == 0) && (start.Y == 0)) return;
            if (pencil == true || hand == true)
                start = finish;
            finish.X = e.X;
            finish.Y = e.Y;
            
            if (line == true)
            {
                mylines[nline].setPoint(start, finish, thick, colorDialog1.Color, num);
                drawTypes[num] = 1;
            }
            if (rect == true)
            {
                myrect[nrect].setRect(start, finish, thick, isFilled, colorDialog1.Color, colorDialog2.Color, num);
                drawTypes[num] = 2;
            }
            if (circle == true)
            {
                mycircle[ncircle].setRectC(start, finish, thick, isFilled, colorDialog1.Color, colorDialog2.Color, num);
                drawTypes[num] = 3;
            }
            if (pencil == true)
            {
                mypencil[npencil].setPoint(start, finish, thick, colorDialog1.Color, num);
                drawTypes[num] = 4;
                Start_Finish point = new Start_Finish();
                point.Type = (int)PacketType.그림;
                point.start = start;                                    //시작 좌표
                point.finish = finish;                                  //끝 좌표
                point.thick = thick;                                    //펜 두께
                point.isFilled = isFilled;                              //채우기 여부
                point.penColor = colorDialog1.Color;                    //펜 색
                point.insideColor = colorDialog2.Color;                 //내부 색
                point.drawType = 4;                                     //선 or 원 or 사각형 or pencil
                point.num = num;                                        //고유 번호
                Packet.Serialize(point).CopyTo(this.sendBuffer, 0);
                this.Send();
                npencil++;
                num++;
            }
            if(hand == true)
            {
                
                if (start.X - e.X > 0)   //왼쪽으로 드래그, 그림들 왼쪽으로 이동
                {
                    int n1 = 0;
                    int n2 = 0;
                    int n3 = 0;
                    int n4 = 0;
                    for (int i = 0; i <= num; i++)
                    {
                        if (drawTypes[i] == 1)
                        {
                            mylines[n1].point[0].X -= start.X - e.X;
                            mylines[n1].point[1].X -= start.X - e.X;
                            n1++;
                        }
                        else if (drawTypes[i] == 2)
                        {
                            myrect[n2].rect.X -= start.X - e.X;
                            n2++;
                        }
                        else if (drawTypes[i] == 3)
                        {
                            mycircle[n3].rectC.X -= start.X - e.X;
                            n3++;
                        }
                        else
                        {
                            mypencil[n4].point[0].X -= start.X - e.X;
                            mypencil[n4].point[1].X -= start.X - e.X;
                            n4++;
                        }
                    }
                }
                else
                {
                    int n1 = 0;
                    int n2 = 0;
                    int n3 = 0;
                    int n4 = 0;
                    for (int i = 0; i <= num; i++)
                    {
                        if (drawTypes[i] == 1)
                        {
                            mylines[n1].point[0].X += e.X - start.X;
                            mylines[n1].point[1].X += e.X - start.X;
                            n1++;
                        }
                        else if (drawTypes[i] == 2)
                        {
                            myrect[n2].rect.X += e.X - start.X;
                            n2++;
                        }
                        else if (drawTypes[i] == 3)
                        {
                            mycircle[n3].rectC.X += e.X - start.X;
                            n3++;
                        }
                        else
                        {
                            mypencil[n4].point[0].X += e.X - start.X;
                            mypencil[n4].point[1].X += e.X - start.X;
                            n4++;
                        }
                    }
                }
                if (start.Y - e.Y > 0)              //마우스를 위로 드래그, 그림들 위로 올라감
                {
                    int n1 = 0;
                    int n2 = 0;
                    int n3 = 0;
                    int n4 = 0;
                    for (int i = 0; i <= num; i++)
                    {
                        if (drawTypes[i] == 1)
                        {
                            mylines[n1].point[0].Y -= start.Y - e.Y;
                            mylines[n1].point[1].Y -= start.Y - e.Y;
                            n1++;
                        }
                        else if (drawTypes[i] == 2)
                        {
                            myrect[n2].rect.Y -= start.Y - e.Y;
                            n2++;
                        }
                        else if (drawTypes[i] == 3)
                        {
                            mycircle[n3].rectC.Y -= start.Y - e.Y;
                            n3++;
                        }
                        else
                        {
                            mypencil[n4].point[0].Y -= start.Y - e.Y;
                            mypencil[n4].point[1].Y -= start.Y - e.Y;
                            n4++;
                        }
                    }
                }
                else
                {
                    int n1 = 0;
                    int n2 = 0;
                    int n3 = 0;
                    int n4 = 0;
                    for (int i = 0; i <= num; i++)
                    {
                        if (drawTypes[i] == 1)
                        {
                            mylines[n1].point[0].Y += e.Y - start.Y;
                            mylines[n1].point[1].Y += e.Y - start.Y;
                            n1++;
                        }
                        else if (drawTypes[i] == 2)
                        {
                            myrect[n2].rect.Y += e.Y - start.Y;
                            n2++;
                        }
                        else if (drawTypes[i] == 3)
                        {
                            mycircle[n3].rectC.Y += e.Y - start.Y;
                            n3++;
                        }
                        else
                        {
                            mypencil[n4].point[0].Y += e.Y - start.Y;
                            mypencil[n4].point[1].Y += e.Y - start.Y;
                            n4++;
                        }
                    }
                }
            }
            
            panel1.Invalidate(true);
            panel1.Update();
        }

        private void panel1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int type = 0;
            if (line == true)
            {
                type = 1;
                nline++;
            }
            if (rect == true)
            {
                type = 2;
                nrect++;
            }
            if (circle == true)
            {
                type = 3;
                ncircle++;
            }
            if (pencil == true)
            {
                start.X = 0;
                start.Y = 0;
                finish.X = 0;
                finish.Y = 0;
                return;
            }

            //==================mouse up 이벤트 발생했을 때 Start_Finish 클래스를 서버로 전송
            Start_Finish point = new Start_Finish();
            point.Type = (int)PacketType.그림;
            point.start = start;                                    //시작 좌표
            point.finish = finish;                                  //끝 좌표
            point.thick = thick;                                    //펜 두께
            point.isFilled = isFilled;                              //채우기 여부
            point.penColor = colorDialog1.Color;                    //펜 색
            point.insideColor = colorDialog2.Color;                 //내부 색
            point.drawType = type;                                  //선 or 원 or 사각형 or pencil
            point.num = num;                                        //고유 번호
            Packet.Serialize(point).CopyTo(this.sendBuffer, 0);                     
            this.Send();
            //===============================================================================
            if(line || rect || circle || pencil)
                num++;
            start.X = 0;
            start.Y = 0;
            finish.X = 0;
            finish.Y = 0;
        }
        

        private void toolStripButton2_Click(object sender, EventArgs e)     //선색 버튼 클릭
        {
            colorDialog1.ShowDialog();
            toolStripButton2.BackColor = colorDialog1.Color;
            pen.Color = colorDialog1.Color;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)     //배경색 버튼 클릭
        {
            colorDialog2.ShowDialog();
            toolStripButton3.BackColor = colorDialog2.Color;
            brush.Color = colorDialog2.Color;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)      //채우기 버튼 클릭
        {
            if (toolStripButton1.Checked)
            {
                toolStripButton1.Checked = false;
            }
            else
                toolStripButton1.Checked = true;
            
        }

        private void toolStripButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripButton1.Checked)       //채우기 버튼 ON
            {
                isFilled = true;
            }
            else                                //채우기 버튼 OFF
            {
                isFilled = false;
            }
        }


        private void Mouse_Wheel(object sender, MouseEventArgs e)
        {
            if ((e.Delta / 120) > 0)
            {
                if (wheel_count > -10)
                {
                    wheel_count--;
                    ratio -= 0.1;
                }
            }
            else
            {
                
                if (wheel_count < 10)
                {
                    wheel_count++;
                    ratio += 0.1;
                }
            }
            panel1.Invalidate(true);
            panel1.Update();
        }


        //=========================통신========================

        private NetworkStream m_networkstream;
        private TcpClient m_client;

        private byte[] sendBuffer = new byte[1024 * 4];
        private byte[] readBuffer = new byte[1024 * 4];

        private bool m_bConnect = false;
        private bool m_bClientOn = false;

        public Initialize m_initializeClass;
        public Start_Finish m_start_finishClass;

        Thread m_thread;

        public void Send()
        {
            this.m_networkstream.Write(this.sendBuffer, 0, this.sendBuffer.Length);
            this.m_networkstream.Flush();

            for(int i = 0; i < 1024 * 4; i++)
            {
                this.sendBuffer[i] = 0;
            }
        }

        public void RUN()                       //데이터 수신 메소드
        {
            int nRead = 0;
            m_bClientOn = true;

            while (this.m_bClientOn)
            {
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
                            break;
                        }
                    case (int)PacketType.그림:
                        {
                            this.m_start_finishClass=
                                (Start_Finish)Packet.Desserialize(this.readBuffer);
                            if (m_start_finishClass.start == m_start_finishClass.finish)
                                break;
                            if (m_start_finishClass.drawType == 1)
                            {
                                mylines[nline].setPoint(m_start_finishClass.start, m_start_finishClass.finish, m_start_finishClass.thick, m_start_finishClass.penColor, m_start_finishClass.num);
                                nline++;
                                drawTypes[num] = 1;
                            }
                            if (m_start_finishClass.drawType == 2)
                            {
                                myrect[nrect].setRect(m_start_finishClass.start, m_start_finishClass.finish, m_start_finishClass.thick, m_start_finishClass.isFilled, m_start_finishClass.penColor, m_start_finishClass.insideColor, m_start_finishClass.num);
                                nrect++;
                                drawTypes[num] = 2;
                            }
                            if (m_start_finishClass.drawType == 3)
                            {
                                mycircle[ncircle].setRectC(m_start_finishClass.start, m_start_finishClass.finish, m_start_finishClass.thick, m_start_finishClass.isFilled, m_start_finishClass.penColor, m_start_finishClass.insideColor, m_start_finishClass.num);
                                ncircle++;                      //start, finish, thick, isFilled, pencolor, insidecolor
                                drawTypes[num] = 3;
                            }
                            if (m_start_finishClass.drawType == 4)
                            {
                                mypencil[npencil].setPoint(m_start_finishClass.start, m_start_finishClass.finish, m_start_finishClass.thick, m_start_finishClass.penColor, m_start_finishClass.num);
                                npencil++;
                                drawTypes[num] = 4;
                            }
                            num++;
                            panel1.Invalidate(true);
                            panel1.Update();
                            break;
                        }
                }
            }

        }




        private void ClientForm2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.m_client.Close();
            this.m_networkstream.Close();
            this.m_thread.Abort();
        }

        private void ClientForm2_Load(object sender, EventArgs e)
        {
            this.m_client = new TcpClient();
            try
            {
                this.m_client.Connect(IP, PORT);
            }
            catch
            {
                this.Close();
                return;
            }
            this.m_bConnect = true;
            this.m_networkstream = this.m_client.GetStream();
            this.textBox2.Select();
            this.m_thread = new Thread(new ThreadStart(RUN));
            this.m_thread.Start();

        }

        private void button1_Click(object sender, EventArgs e)           //메시지 전송 버튼
        {
            if (!this.m_bConnect)
            {
                return;
            }
            this.textBox1.AppendText(ID + ":" + this.textBox2.Text + "\r\n");
            Initialize initialize = new Initialize();
            initialize.Type = (int)PacketType.채팅;
            initialize.ID = ID;
            initialize.Message = this.textBox2.Text;

            Packet.Serialize(initialize).CopyTo(this.sendBuffer, 0);
            this.Send();
            this.textBox2.ResetText();
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!this.m_bConnect)
                {
                    return;
                }
                this.textBox1.AppendText(ID + ":" + this.textBox2.Text + "\r\n");
                Initialize initialize = new Initialize();
                initialize.Type = (int)PacketType.채팅;
                initialize.ID = ID;
                initialize.Message = this.textBox2.Text;

                Packet.Serialize(initialize).CopyTo(this.sendBuffer, 0);
                this.Send();
                this.textBox2.ResetText();
            }
            
        }

        //=====================================================

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace ClassLibrary1
{
    public enum PacketType
    {
        채팅 = 0,
        그림,
        
    }
    public enum PacketSendERROR
    {
        정상 = 0,
        에러
    }
    [Serializable]
    public class Packet
    {
        public int Length;
        public int Type;

        public Packet()
        {
            this.Length = 0;
            this.Type = 0;
        }
        public static byte[] Serialize(Object o)
        {
            MemoryStream ms = new MemoryStream(1024 * 4);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);                        

            return ms.ToArray();
        }
        public static Object Desserialize(byte[] bt)
        {
            MemoryStream ms = new MemoryStream(1024 * 4);
            foreach (byte b in bt)
            {
                ms.WriteByte(b);
            }

            ms.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            Object obj = bf.Deserialize(ms);                 
            ms.Close();
            return obj;

        }

    }
    [Serializable]
    public class Initialize : Packet
    {
        public string Message;
        public string ID;

        public Initialize()
        {
            this.Message = null;
            this.ID = null;
        }
    }

    [Serializable]
    public class Start_Finish : Packet
    {
        public Point start;
        public Point finish;
        public int drawType = 0;
        public int thick = 0;
        public int num = 0;
        public bool isFilled = false;
        public Color penColor;
        public Color insideColor;

        public Start_Finish()
        {
            this.start = new Point(0, 0);
            this.finish = new Point(0, 0);
        }
    }
    
    public class MyLines
    {
        public Point[] point = new Point[2];
        private int thick;
        private bool isSolid;
        public int num=0;

        public Color penColor;
        public Color insideColor;

        public MyLines()
        {
            point[0] = new Point();
            point[1] = new Point();
            thick = 1;
            isSolid = true;
        }
        public void setPoint(Point start, Point finish, int thick, Color penColor, int num)
        {
            point[0] = start;
            point[1] = finish;
            this.thick = thick;
            this.num = num;
            this.penColor = penColor;
        }
        public Point getPoint1()
        {
            return point[0];
        }
        public Point getPoint2()
        {
            return point[1];
        }
        public int getThick()
        {
            return thick;
        }
    }
    
    public class MyCircle
    {
        public Rectangle rectC;
        private int thick;
        private bool isFilled;
        public int num;

        public Color penColor;
        public Color insideColor;

        public MyCircle()
        {
            num = 0;
            rectC = new Rectangle();
            thick = 1;
            isFilled = false;
        }
        public void setRectC(Point start, Point finish, int thick, bool isFilled, Color penColor, Color insideColor, int num)
        {
            rectC.X = Math.Min(start.X, finish.X);
            rectC.Y = Math.Min(start.Y, finish.Y);
            rectC.Width = Math.Abs(start.X - finish.X);
            rectC.Height = Math.Abs(start.Y - finish.Y);
            this.thick = thick;
            this.num = num;
            this.isFilled = isFilled;
            this.penColor = penColor;
            this.insideColor = insideColor;
        }

        public Rectangle getRectC()
        {
            return rectC;
        }

        public int getThick()
        {
            return thick;
        }

        public bool getFilled()
        {
            return isFilled;
        }
    }
    
    public class MyRect
    {
        public Rectangle rect;
        private int thick;
        private bool isFilled;
        public int num;

        public Color penColor;
        public Color insideColor;

        public MyRect()
        {
            rect = new Rectangle();
            thick = 1;
            num = 0;
            isFilled = false;
        }

        public void setRect(Point start, Point finish, int thick, bool isFilled, Color penColor, Color insideColor, int num)
        {
            rect.X = Math.Min(start.X, finish.X);
            rect.Y = Math.Min(start.Y, finish.Y);
            rect.Width = Math.Abs(start.X - finish.X);
            rect.Height = Math.Abs(start.Y - finish.Y);
            this.thick = thick;
            this.num = num;
            this.isFilled = isFilled;
            this.penColor = penColor;
            this.insideColor = insideColor;
        }
        public Rectangle getRect()
        {
            return rect;
        }
        public int getThick()
        {
            return thick;
        }
        public bool getFilled()
        {
            return isFilled;
        }
    }
    
    public class MyPencil
    {
        public Point[] point = new Point[2];
        private int thick;
        public int num;

        public Color penColor;
        public Color insideColor;

        public MyPencil()
        {
            num = 0;
            point[0] = new Point();
            point[1] = new Point();
            thick = 1;
        }
        public void setPoint(Point start, Point finish, int thick, Color penColor, int num)
        {
            point[0] = start;
            point[1] = finish;
            this.thick = thick;
            this.penColor = penColor;
            this.num = num;
        }
        public Point getPoint1()
        {
            return point[0];
        }
        public Point getPoint2()
        {
            return point[1];
        }
        public int getThick()
        {
            return thick;
        }
    }

}

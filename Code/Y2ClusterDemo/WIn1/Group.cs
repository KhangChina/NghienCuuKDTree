/*
 * Created by SharpDevelop.
 * User: Windown Office-Tuan
 * Date: 4/12/2011
 * Time: 11:46 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace WIn1
{
    public class XPoint : IComparable
    {
        public string Name;
        public int Left=-1;
        public int Top = -1;
        public XPoint() { }
        public XPoint(string name, int x, int y)
        {
            this.Name = name;
            this.Left = x;
            this.Top = y;
        }
        public virtual Rectangle ToRectangle()
        {
            return new Rectangle(Left, Top, 3, 3);
        }
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            XPoint p = obj as XPoint;
            if (p == null)
                return false;

            return this.Left == p.Left && this.Top == p.Top;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public virtual float GetDistance(XPoint p)
        {
            return (float)Math.Round(Math.Sqrt(Math.Pow(this.Left - p.Left, 2) + Math.Pow(this.Top - p.Top, 2))/10, 1);
        }
        public virtual float GetMaxDistance(XPoint p)
        {
            return (float)(Math.Sqrt(Math.Pow(this.Left - p.Left, 2) + Math.Pow(this.Top - p.Top, 2)));
        }
        public int CompareTo(object obj)
        {
            XPoint p = obj as XPoint;
            if (p == null)
                return -1;
            return this.Name.CompareTo(p.Name);

        }
    }
    public class Group : XPoint, IEnumerable<XPoint>
    {
        List<XPoint> _list = new List<XPoint>();
        public int Right=-1;
        public int Bottom = -1;
        public Color Color { get; private set; }

        private int XCenter, YCenter;
        private int Radius;

        public Group()
        {
            Random rnd = new Random();
            this.Color = Color.FromArgb(rnd.Next(100),
                   rnd.Next(255), rnd.Next(255), rnd.Next(255));
        }
        public void Add(XPoint p)
        {
            if (_list.Contains(p))
                return;
            if (this.Left == -1 || this.Left > p.Left)
                this.Left = p.Left;
            if (this.Right == -1 || this.Right < p.Left)
                this.Right = p.Left;
            if (this.Top == -1 || this.Top > p.Top)
                this.Top = p.Top;
            if (this.Bottom == -1 || this.Bottom < p.Top)
                this.Bottom = p.Top;

            if (p is Group)
            {
                Group g = p as Group;
                foreach (XPoint g1 in g)
                {
                    Add(g1);
                }
            }
            _list.Add(p);

            StringBuilder str = new StringBuilder();
            foreach (XPoint xp in this._list)
            {
                if (!(xp is Group))
                    str.Append(xp.Name).Append(",");
            }
            this.Name = str.ToString().TrimEnd(',');
        }
        public int Count
        {
            get
            {

                return _list.Count;
            }
        }
        public override float GetDistance(XPoint p)
        {
            float min = 5000;
            if (p is Group)
            {
                Group g = p as Group;
                foreach (XPoint p1 in this._list)
                {
                    foreach (XPoint p2 in g._list)
                    {
                        float d = p1.GetDistance(p2);
                        if (d < min)
                            min = d;
                    }
                }
            }

            return min;
        }
        public override float GetMaxDistance(XPoint p)
        {
            float m = 0;
            if (p is Group)
            {
                Group g = p as Group;
                foreach (XPoint p1 in this._list)
                {
                    foreach (XPoint p2 in g._list)
                    {
                        float d = p1.GetMaxDistance(p2);
                        if (d > m)
                            m = d;
                    }
                }
            }

            return m;
        }
        public Rectangle ToSquare()
        {

            if (Radius == 0)
            {
                float max = 0;
                int index1 = 0, index2 = 0;
                for (int i = 0; i < _list.Count - 1; i++)
                {
                    for (int j = i + 1; j < _list.Count; j++)
                    {

                        float d = _list[i].GetMaxDistance(_list[j]);
                        if (d > max)
                        {
                            max = d;
                            index1 = i;
                            index2 = j;
                        }
                    }
                }
                if (index1 == index2)
                {
                    max = 2;
                    this.XCenter = (int)_list[index1].Left;
                    this.YCenter = (int)_list[index1].Top;
                }
                else
                {
                    this.XCenter = Math.Abs((int)_list[index1].Left + (int)_list[index2].Left) / 2;
                    this.YCenter = Math.Abs((int)_list[index1].Top + (int)_list[index2].Top) / 2;
                }
                
                this.Radius = (int)max / 2+_list.Count*2;
            }

            int x = XCenter - Radius;
            int y = YCenter - Radius;
            int dd = (int)Radius * 2;
            return new Rectangle(x, y, dd, dd);

        }
        public override Rectangle ToRectangle()
        {
            if (_list.Count == 0)
                return Rectangle.Empty;
            int offset = _list.Count * 3;

            return new Rectangle((int)this.Left - offset, (int)Top - offset, (int)Right - (int)Left + offset * 2, (int)Bottom - (int)Top + offset * 2);
        }
        public override string ToString()
        {
            return this.Name;
        }
        IEnumerator<XPoint> IEnumerable<XPoint>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
    public class Matrix : IEnumerable<Group>
    {
        List<Group> _list = new List<Group>();

        public Matrix() { }


        public void Add(Group g)
        {
            _list.Add(g);
            _list.Sort();
        }
        public Group this[int index]
        {
            get { return _list[index]; }
        }
        public int Count
        {
            get
            {

                return _list.Count;
            }
        }
        public void Clear()
        {
            _list.Clear();
        }
        public Group GetNearestGroup()
        {
            float min = 1000;
            int index1 = 0, index2 = 0;

            for (int i = 0; i < _list.Count - 1; i++)
            {
                for (int j = i + 1; j < _list.Count; j++)
                {

                    float d = _list[i].GetDistance(_list[j]);
                    if (d < min)
                    {
                        min = d;
                        index1 = i;
                        index2 = j;
                    }
                }
            }

            Group g = new Group();
            Group g1 = _list[index1];
            Group g2 = _list[index2];
            g.Add(g1);
            g.Add(g2);
            _list.Remove(g1);
            _list.Remove(g2);
            return g;
        }

        public IEnumerator<Group> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}

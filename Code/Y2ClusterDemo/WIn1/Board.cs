using System;
using System.Drawing;
using System.Windows.Forms;

namespace WIn1
{

    /// <summary>
    /// Description of Board.
    /// </summary>
    public class Board : Control
    {

        private const int POINT_SIZE = 5;
        Matrix _matrix = new Matrix();
        int _clickCount = 0;
        private bool _isRectangleBound;
        public bool IsRectangleBound
        {
            get { return _isRectangleBound; }
            set { _isRectangleBound = value;
            Invalidate();
            }
        }
        public int GroupCount
        {
            get { return _matrix.Count; }
        }
        public Board()
        {
            this.DoubleBuffered = true;
        }
        public void Clear()
        {
            _matrix.Clear();
            _clickCount = 0;
            Invalidate();
        }
        public void CreateNewGroup()
        {
            if (_matrix.Count > 1)
            {
                Group g = _matrix.GetNearestGroup();
                _matrix.Add(g);

                Invalidate();
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (_clickCount >= 27)
                return;
            string name = ((char)(_clickCount + 'A')).ToString();
            _clickCount++;
            Group g = new Group();
            g.Add(new XPoint(name, e.X, e.Y));
            _matrix.Add(g);

            Invalidate();

            base.OnMouseClick(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            foreach (Group group in _matrix)
            {
                DrawGroup(group, e.Graphics);
                foreach (XPoint p in group)
                {

                    if (p is Group)
                    {

                        Group g = p as Group;

                        DrawGroup(g, e.Graphics);
                    }
                    else
                    {

                        e.Graphics.DrawString(p.Name, this.Font, Brushes.Red, (int)p.Left + 3, (int)p.Top + 3);
                    }
                }
            }
            DrawMatrix(e.Graphics);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
            base.OnPaint(e);
        }
        void DrawMatrix(Graphics graphisc)
        {
            if (_matrix.Count == 0)
                return;
            int OFFSET = 40;
            int cellSize = 60 - _matrix.Count*3;
            if (cellSize < 20)
                cellSize = 20;

            int size = cellSize * _matrix.Count + OFFSET;
            for (int i = 0; i < _matrix.Count; i++)
            {
                int x = cellSize * i + OFFSET;
                graphisc.DrawLine(Pens.DarkGray,
                                  x, 0, x, size);
                graphisc.DrawLine(Pens.DarkGray,
                                  0, x, size, x);
                string s = _matrix[i].Name;
                if (s.Length > 3)
                    s = s.Substring(0, 2) + Environment.NewLine + s.Substring(3);

                
                RectangleF rc = new RectangleF(x, 0, OFFSET, OFFSET);
                graphisc.DrawString(s, this.Font, Brushes.Red, rc);
                
                
                rc = new RectangleF(0, x, OFFSET, OFFSET);
                graphisc.DrawString(s, this.Font, Brushes.Red, rc);

                for (int j = 0; j < _matrix.Count; j++)
                {
                    float d = _matrix[i].GetDistance(_matrix[j]);

                    SizeF sf = graphisc.MeasureString(d.ToString(), Font);
                    float l = x + (cellSize - sf.Width) / 2;
                    float t = j * cellSize + OFFSET+ (cellSize- sf.Height) / 2;
                    graphisc.DrawString(d.ToString(),
                                        this.Font, Brushes.Blue,
                                        l, t);
                }
            }
            graphisc.DrawRectangle(Pens.Black, 0, 0, size, size);

        }
        void DrawGroup(Group group, Graphics graphic)
        {
            foreach (XPoint p in group)
            {
                
                if (p is Group)
                {
                                        
                    Group g = p as Group;

                    DrawGroup(g, graphic);
                }
                else
                    graphic.DrawString(p.Name, this.Font, Brushes.Red, (int)p.Left + 3, (int)p.Top + 3);
            }
            Rectangle rect1=IsRectangleBound?group.ToRectangle():group.ToSquare();                        
            Brush b = new SolidBrush(group.Color);
            if (IsRectangleBound)
            {
                graphic.DrawRectangle(Pens.Blue, rect1);
                graphic.FillRectangle(b, rect1);
            }
            else
            {
                graphic.DrawEllipse(Pens.Blue, rect1);
                graphic.FillEllipse(b, rect1);
            }
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletHell.Gfx;
using System.Drawing;
using BulletHell.MathLib;

namespace BulletHell.Physics.ShapeLib
{
    public class Ellipse : PhysicsShape
    {
        private Particle r;
        private Drawable myDraw;

        public Ellipse(double r, int dim=2)
        {
            Vector<double> rad = new Vector<double>(dim);
            for (int i = 0; i < dim; i++)
                rad[i] = r;
            this.r = rad;
            myDraw = DrawableFactory.MakeEllipse(this.r, new GraphicsStyle(null, Pens.Red));
        }

        public Ellipse(Particle rad)
        {
            r = rad;
            myDraw = DrawableFactory.MakeEllipse(r, new GraphicsStyle(null, Pens.Red));
        }


        protected override void Draw(Particle p, Graphics g, GraphicsStyle sty = null)
        {
            myDraw(p, g, sty);
        }

        protected override bool containsPoint(Particle pos, Particle p)
        {
            if(p.Dimension!=r.Dimension || p.Dimension!=pos.Dimension)
                return false;
            Vector<double> cp = p.CurrentPosition;
            Vector<double> mcp = pos.CurrentPosition;
            Vector<double> diff = cp-mcp;
            Vector<double> rp = r.CurrentPosition;
            for (int i = 0; i < pos.Dimension; i++)
            {
                if (Math.Abs(diff[i]) > Math.Abs(rp[i]))
                    return false;
            }
            double sum=0;
            Func<double,double> sqr = t=>t*t;
            for (int i = 0; i < pos.Dimension; i++)
            {
                sum += sqr(diff[i] / rp[i]);
            }
            return sum < 1;
        }

        protected override bool meets(Particle p, PhysicsShape o, Particle oPos)
        {
            if(o.GetType()==typeof(Point))
            {
                return ContainsPoint(p,oPos);
            }
            if(o.GetType()==typeof(Ellipse))
            {
                Ellipse oth = o as Ellipse;
                return new Ellipse(r + oth.r).ContainsPoint(p, oPos);
            }
            return o.Meets(oPos, this, p);
        }

        public Particle Radius
        {
            get
            { return r; }
        }

        public override Box BoundingBox
        {
            get
            {
                return new Box(r);
            }
        }

        protected override void UpdateTime()
        {
            r.Time = Time;
        }
    }
}

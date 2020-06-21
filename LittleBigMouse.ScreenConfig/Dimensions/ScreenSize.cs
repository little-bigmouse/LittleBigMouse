/*
  LittleBigMouse.Screen.Config
  Copyright (c) 2017 Mathieu GRENET.  All right reserved.

  This file is part of LittleBigMouse.Screen.Config.

    LittleBigMouse.Screen.Config is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LittleBigMouse.Screen.Config is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MouseControl.  If not, see <http://www.gnu.org/licenses/>.

	  mailto:mathieu@mgth.fr
	  http://www.mgth.fr
*/

using System;
using System.Runtime.Serialization;
using System.Windows;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using Newtonsoft.Json;

namespace LittleBigMouse.ScreenConfig.Dimensions
{
    public interface IScreenSize : IEquatable<IScreenSize>
    {
        double Width { get; set; }
        double Height { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double TopBorder { get; set; }
        double BottomBorder { get; set; }
        double LeftBorder { get; set; }
        double RightBorder { get; set; }

        Rect Bounds { get; }
        Point Center { get; }

        Rect OutsideBounds { get; }
        double OutsideWidth { get; }
        double OutsideHeight { get; }
        double OutsideX { get; }
        double OutsideY { get; }

        Point Location { get; }
    }

    public static class ScreenSizeExtensions
    {
        public static Point GetPoint(this IScreenSize sz, IScreenSize source, Point point)
        {
            var x = (point.X - source.X) / source.Width;
            var y = (point.Y - source.Y) / source.Height;

            return new Point(sz.X + x * sz.Width, sz.Y + y * sz.Height);
        }
        public static Point Inside(this IScreenSize sz, Point p)
        {
            var x = p.X < sz.X ? sz.X : (p.X > sz.Bounds.Right - 1) ? (sz.Bounds.Right - 1) : p.X;
            var y = p.Y < sz.Y ? sz.Y : (p.Y > sz.Bounds.Bottom - 1) ? (sz.Bounds.Bottom - 1) : p.Y;

            return new Point(x, y);
        }
    }

    [DataContract]
    public abstract class ScreenSize<TClass> : N<TClass>, IScreenSize
    where TClass : ScreenSize<TClass>
    {
        protected ScreenSize(IScreenSize source):base(false)
        {
            Source = source;
        }

        [JsonIgnore]
        public IScreenSize Source { get; }

        //[DataMember]
        public abstract double Width { get; set; }
        //[DataMember]
        public abstract double Height { get; set; }
        //[DataMember]
        public abstract double X { get; set; }
        //[DataMember]
        public abstract double Y { get; set; }
        //[DataMember]
        public abstract double TopBorder { get; set; }
        //[DataMember]
        public abstract double BottomBorder { get; set; }
        //[DataMember]
        public abstract double LeftBorder { get; set; }

        //[DataMember]
        public abstract double RightBorder { get; set; }


        [DataMember]
        public Thickness Borders => _borders.Get();
        private readonly IProperty<Thickness> _borders = H.Property<Thickness>(c => c
            .Set(e => new Thickness(e.LeftBorder, e.TopBorder, e.RightBorder, e.BottomBorder))
            .On(nameof(LeftBorder))
            .On(nameof(RightBorder))
            .On(nameof(TopBorder))
            .On(nameof(BottomBorder))
            .Update()
        );
        //[DataMember]
        public Point Location
        {
            get => _location.Get();
            set
            {
                    X = value.X;
                    Y = value.Y;
            }
        }
        private readonly IProperty<Point> _location = H.Property<Point>(c=>c
            .Set(e => new Point(e.X, e.Y))
            .On(nameof(X))
            .On(nameof(Y))
            .Update()
        );

        public Size Size
        {
            get => _size.Get();
            set
            {
                    Height = value.Height;
                    Width = value.Width;
            }
        }
        private readonly IProperty<Size> _size = H.Property<Size>(c => c
            .Set(e => new Size(e.Width,e.Height))            
            .On(nameof(Width))
            .On(nameof(Height))
            .Update()
        );

        public Point Center => _center.Get();
        private readonly IProperty<Point> _center = H.Property<Point>(c => c
            .Set(e => e.Location + new Vector(e.Width / 2, e.Height / 2) )            
            .On(e => e.Location)
            .On(e => e.Size)
            .Update()
        );


        //[DataMember]
        [DataMember] public Rect Bounds => _bounds.Get();
        private readonly IProperty<Rect> _bounds = H.Property<Rect>(c => c
            .Set(e => new Rect(e.Location,e.Size) )            
            .On(e => e.Location)
            .On(e => e.Size)
            .Update()
        );

        //[DataMember]
        public double OutsideX
        {
            get => X - LeftBorder;
            set => X = value + LeftBorder;
        }
        private readonly IProperty<double> _outsideX = H.Property<double>(c => c
            .Set(e => e.X - e.LeftBorder )            
            .On(e => e.X)
            .On(e => e.LeftBorder)
            .Update()
        );

        //[DataMember]
       public double OutsideY
        {
            get => Y - TopBorder;
            set => Y = value + TopBorder;
        }
        private readonly IProperty<double> _outsideY = H.Property<double>(c => c
            .Set(e => e.Y - e.TopBorder )            
            .On(e => e.Y)
            .On(e => e.TopBorder)
            .Update()
        );

       public double OutsideWidth
       {
           get => _outsideWidth.Get();
           set => throw new NotImplementedException();
       }
       private readonly IProperty<double> _outsideWidth = H.Property<double>(c => c
           .Set(e => e.Width + e.LeftBorder + e.RightBorder)
           .On(e => e.Width)
           .On(e=>e.LeftBorder)
           .On(e => e.RightBorder)
           .Update()
       );


       public double OutsideHeight
       {
           get => _outsideHeight.Get();
           set => throw new NotImplementedException();
       }
       private readonly IProperty<double> _outsideHeight = H.Property<double>(c => c
           .Set(e => e.Height + e.TopBorder + e.BottomBorder)
           .On(e => e.Height)
           .On(e => e.TopBorder)
           .On(e => e.BottomBorder)
           .Update()
       );



        [DataMember]
        public Rect OutsideBounds => _outsideBounds.Get();
        private readonly IProperty<Rect> _outsideBounds = H.Property<Rect>(c => c
           .Set(e => new Rect(new Point(e.OutsideX, e.OutsideY), new Size(e.OutsideWidth, e.OutsideHeight)))
           .On(e => e.OutsideX)
           .On(e => e.OutsideY)
           .On(e => e.OutsideWidth)
           .Update()
       );
        public bool Equals(IScreenSize other)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using Unity.UIWidgets.ui.txt;
using UnityEngine;

namespace Unity.UIWidgets.ui {
    public abstract class DrawCmd {
    }

    public class DrawSave : DrawCmd {
    }

    public class DrawSaveLayer : DrawCmd {
        public Rect rect;
        public Paint paint;
    }

    public class DrawRestore : DrawCmd {
    }

    public class DrawTranslate : DrawCmd {
        public double dx;
        public double dy;
    }

    public class DrawScale : DrawCmd {
        public double sx;
        public double? sy;
    }

    public class DrawRotate : DrawCmd {
        public double radians;
        public Offset offset;
    }

    public class DrawSkew : DrawCmd {
        public double sx;
        public double sy;
    }

    public class DrawConcat : DrawCmd {
        public Matrix3 matrix;
    }

    public class DrawResetMatrix : DrawCmd {
    }

    public class DrawSetMatrix : DrawCmd {
        public Matrix3 matrix;
    }

    public class DrawClipRect : DrawCmd {
        public Rect rect;
    }

    public class DrawClipRRect : DrawCmd {
        public RRect rrect;
    }

    public class DrawClipPath : DrawCmd {
        public Path path;
    }

    public class DrawPath : DrawCmd {
        public Path path;
        public Paint paint;
    }

    public class DrawImage : DrawCmd {
        public Texture image;
        public Offset offset;
        public Paint paint;
    }

    public class DrawImageRect : DrawCmd {
        public Texture image;
        public Rect src;
        public Rect dst;
        public Paint paint;
    }

    public class DrawImageNine : DrawCmd {
        public Texture image;
        public Rect src;
        public Rect center;
        public Rect dst;
        public Paint paint;
    }

    public class DrawPicture : DrawCmd {
        public Picture picture;
    }

    public class DrawTextBlob : DrawCmd {
        public TextBlob textBlob;
        public Offset offset;
        public Paint paint;
    }
}
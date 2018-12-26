﻿using Unity.UIWidgets.ui;
using Rect = Unity.UIWidgets.ui.Rect;

namespace Unity.UIWidgets.flow {
    public class ClipRectLayer : ContainerLayer {
        private Rect _clipRect;

        public Rect clipRect {
            set { this._clipRect = value; }
        }

        public override void preroll(PrerollContext context, Matrix3 matrix) {
            var childPaintBounds = Rect.zero;
            this.prerollChildren(context, matrix, ref childPaintBounds);
            childPaintBounds = childPaintBounds.intersect(this._clipRect);

            if (!childPaintBounds.isEmpty) {
                this.paintBounds = childPaintBounds;
            }
        }

        public override void paint(PaintContext context) {
            var canvas = context.canvas;

            canvas.save();
            canvas.clipRect(this.paintBounds);

            try {
                this.paintChildren(context);
            }
            finally {
                canvas.restore();
            }
        }
    }
}
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.UIWidgets.widgets {
    public class DefaultTextStyle : InheritedWidget {
        public DefaultTextStyle(
            Key key = null,
            TextStyle style = null,
            TextAlign? textAlign = null,
            bool softWrap = true,
            TextOverflow overflow = TextOverflow.clip,
            int? maxLines = null,
            Widget child = null
        ) : base(key, child) {
            D.assert(style != null);
            D.assert(maxLines == null || maxLines > 0);
            D.assert(child != null);
            
            this.style = style;
            this.textAlign = textAlign;
            this.softWrap = softWrap;
            this.overflow = overflow;
            this.maxLines = maxLines;
        }

        private DefaultTextStyle() {
            this.style = new TextStyle();
            this.textAlign = null;
            this.softWrap = true;
            this.overflow = TextOverflow.clip;
            this.maxLines = null;
        }

        public static DefaultTextStyle fallback() {
            return _fallback;
        }
        
        static readonly DefaultTextStyle _fallback = new DefaultTextStyle();

        public static Widget merge(
            Key key = null,
            TextStyle style = null,
            TextAlign? textAlign = null,
            bool? softWrap = null,
            TextOverflow? overflow = null,
            int? maxLines = null,
            Widget child = null
        ) {
            D.assert(child != null);
            return new Builder(builder: (context => {
                var parent = DefaultTextStyle.of(context);
                return new DefaultTextStyle(
                    key: key,
                    style: parent.style.merge(style),
                    textAlign: textAlign ?? parent.textAlign,
                    softWrap: softWrap ?? parent.softWrap,
                    overflow: overflow ?? parent.overflow,
                    maxLines: maxLines ?? parent.maxLines,
                    child: child
                );
            }));
        }

        public readonly TextStyle style;
        public readonly TextAlign? textAlign;
        public readonly bool softWrap;
        public readonly TextOverflow overflow;
        public readonly int? maxLines;

        public static DefaultTextStyle of(BuildContext context) {
            var inherit = (DefaultTextStyle) context.inheritFromWidgetOfExactType(typeof(DefaultTextStyle));
            return inherit ?? DefaultTextStyle.fallback();
        }

        public override bool updateShouldNotify(InheritedWidget w) {
            var oldWidget = (DefaultTextStyle) w;
            return this.style != oldWidget.style ||
                   this.textAlign != oldWidget.textAlign ||
                   this.softWrap != oldWidget.softWrap ||
                   this.overflow != oldWidget.overflow ||
                   this.maxLines != oldWidget.maxLines;
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            if (this.style != null) {
                this.style.debugFillProperties(properties);
            }

            properties.add(new EnumProperty<TextAlign?>("textAlign", this.textAlign,
                defaultValue: Diagnostics.kNullDefaultValue));
            properties.add(new FlagProperty("softWrap", value: this.softWrap, ifTrue: "wrapping at box width",
                ifFalse: "no wrapping except at line break characters", showName: true));
            properties.add(new EnumProperty<TextOverflow>("overflow", this.overflow,
                defaultValue: Diagnostics.kNullDefaultValue));
            properties.add(new IntProperty("maxLines", this.maxLines,
                defaultValue: Diagnostics.kNullDefaultValue));
        }
    }

    public class Text : StatelessWidget {
        public Text(string data,
            Key key = null, 
            TextStyle style = null,
            TextAlign? textAlign = null, 
            bool? softWrap = null,
            TextOverflow? overflow = null,
            double? textScaleFactor = null,
            int? maxLines = null) : base(key) {
            D.assert(data != null);
            this.textSpan = null;
            this.data = data;
            this.style = style;
            this.textAlign = textAlign;
            this.softWrap = softWrap;
            this.overflow = overflow;
            this.textScaleFactor = textScaleFactor;
            this.maxLines = maxLines;
        }

        private Text(TextSpan textSpan, 
            Key key = null, 
            TextStyle style = null,
            TextAlign? textAlign = null,
            bool? softWrap = null,
            TextOverflow? overflow = null,
            double? textScaleFactor = null, 
            int? maxLines = null) : base(key) {
            D.assert(textSpan != null);
            this.textSpan = textSpan;
            this.data = null;
            this.style = style;
            this.textAlign = textAlign;
            this.softWrap = softWrap;
            this.overflow = overflow;
            this.textScaleFactor = textScaleFactor;
            this.maxLines = maxLines;
        }

        public static Text rich(TextSpan textSpan,
            Key key = null,
            TextStyle style = null,
            TextAlign? textAlign = null,
            bool? softWrap = null,
            TextOverflow? overflow = null,
            double? textScaleFactor = null,
            int? maxLines = null) {
            return new Text(
                textSpan, key,
                style,
                textAlign,
                softWrap,
                overflow,
                textScaleFactor,
                maxLines);
        }

        public readonly string data;

        public readonly TextSpan textSpan;

        public readonly TextStyle style;

        public readonly TextAlign? textAlign;

        public readonly bool? softWrap;

        public readonly TextOverflow? overflow;

        public readonly double? textScaleFactor;

        public readonly int? maxLines;

        public override Widget build(BuildContext context) {
            DefaultTextStyle defaultTextStyle = DefaultTextStyle.of(context);
            TextStyle effectiveTextStyle = this.style;
            if (this.style == null || this.style.inherit) {
                effectiveTextStyle = defaultTextStyle.style.merge(this.style);
            }

            return new RichText(
                textAlign: this.textAlign ?? defaultTextStyle.textAlign ??  TextAlign.left,
                softWrap: this.softWrap ?? defaultTextStyle.softWrap,
                overflow: this.overflow ?? defaultTextStyle.overflow,
                textScaleFactor: this.textScaleFactor ?? 1.0, // MediaQuery.textScaleFactorOf(context), todo
                maxLines: this.maxLines ?? defaultTextStyle.maxLines,
                text: new TextSpan(
                    style: effectiveTextStyle,
                    text: this.data,
                    children: this.textSpan != null ? new List<TextSpan> {this.textSpan} : null
                )
            );
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new StringProperty("data", this.data, showName: false));
            if (this.textSpan != null) {
                properties.add(this.textSpan.toDiagnosticsNode(name: "textSpan", style: DiagnosticsTreeStyle.transition));
            }

            if (this.style != null) {
                this.style.debugFillProperties(properties);
            }

            properties.add(new EnumProperty<TextAlign?>("textAlign", this.textAlign,
                defaultValue: Diagnostics.kNullDefaultValue));
            properties.add(new FlagProperty("softWrap", value: this.softWrap, ifTrue: "wrapping at box width",
                ifFalse: "no wrapping except at line break characters", showName: true));
            properties.add(new EnumProperty<TextOverflow?>("overflow", this.overflow,
                defaultValue: Diagnostics.kNullDefaultValue));
            properties.add(new DoubleProperty("textScaleFactor", this.textScaleFactor,
                defaultValue: Diagnostics.kNullDefaultValue));
            properties.add(new IntProperty("maxLines", this.maxLines, defaultValue: Diagnostics.kNullDefaultValue));
        }
    }
}
using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Crosswall
{
	[Register("crosswall.IOSButton")]
	public class IOSButton : TextView
	{
		GradientDrawable _gradientDrawable;
		Color _unPressColor;
		Color _pressColor;
		Color _strokeColor;
		int _strokeWidth = 2;
		int _cornerRadius = 12;
		Resources _resources;
		const int DefaultStrokeWidth = 2;
		const int DefailtCornerRadius = 12;
		Color textUnPressColor;
		Color textPressColor;

		public IOSButton(Context context)
			: this(context, null, 0, 0)
		{
		}

		public IOSButton(Context context, IAttributeSet attrs)
			: this(context, attrs, 0, 0)
		{
		}

		public IOSButton(Context context, IAttributeSet attrs, int defStyleAttr)
			: this(context, attrs, defStyleAttr, 0)
		{
		}

		public IOSButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
			:base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs, defStyleAttr, defStyleRes);
		}

		void Init(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
		{
			_resources = Resources;
			if (_gradientDrawable == null)
			{
				_gradientDrawable = new GradientDrawable();
			}

			TypedArray typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.IOSButton, defStyleAttr, defStyleRes);
			textUnPressColor = typedArray.GetColor(Resource.Styleable.IOSButton_btn_text_unpressColor, Color.Gray);
			textPressColor = typedArray.GetColor(Resource.Styleable.IOSButton_btn_text_pressColor, Color.White);
			_unPressColor = typedArray.GetColor(Resource.Styleable.IOSButton_btn_unpressColor, Color.White);
			_pressColor = typedArray.GetColor(Resource.Styleable.IOSButton_btn_pressColor, Color.Gray);
			_strokeColor = typedArray.GetColor(Resource.Styleable.IOSButton_btn_strokeColor, Color.Gray);
			_strokeWidth = typedArray.GetDimensionPixelSize(Resource.Styleable.IOSButton_btn_strokeWidth, DefaultStrokeWidth);
			_cornerRadius = typedArray.GetDimensionPixelSize(Resource.Styleable.IOSButton_btn_cornerRadius, DefailtCornerRadius);
			_gradientDrawable.SetShape(ShapeType.Rectangle);
			_gradientDrawable.SetColor(_unPressColor);
			_gradientDrawable.SetStroke(_strokeWidth, _strokeColor, 0, 0);
			_gradientDrawable.SetCornerRadius(_cornerRadius);

			SetButtonBackgroud();
			typedArray.Recycle();

			Gravity = GravityFlags.Center;
			SetTextColor(textUnPressColor);

			Clickable = true;
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			switch (e.Action)
			{
				case MotionEventActions.Down:
					SetPressStatus(true);
					break;
				case MotionEventActions.Move:
				case MotionEventActions.Cancel:
				case MotionEventActions.Up:
					SetPressStatus(false);
					break;
			}
			return base.OnTouchEvent(e);
		}

		void SetButtonBackgroud()
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
				Background = _gradientDrawable;
			else
				SetBackgroundDrawable(_gradientDrawable);
		}

		public void SetPressStatus(bool isPress)
		{
			if (isPress)
			{
				SetTextColor(textPressColor);
				_gradientDrawable.SetColor(ColorBurn(_pressColor));
			}
			else 
			{
				SetTextColor(textUnPressColor);
				_gradientDrawable.SetColor(_unPressColor);
			}
			SetButtonBackgroud();
		}


		public void SetButtonStatus(bool isEnable)
		{
			if (isEnable)
			{
				SetTextColor(textPressColor);
				_gradientDrawable.SetColor(_pressColor);
			}
			else {
				SetTextColor(textUnPressColor);
				_gradientDrawable.SetColor(_unPressColor);
			}

			SetButtonBackgroud();
		}

		public void SetDrawableRightText(int text, int imgResId)
		{
			SetDrawableRightText(_resources.GetString(text), imgResId);
		}

		public void SetDrawableRightText(string text, int imgResId)
		{
			Drawable drawable = ContextCompat.GetDrawable(Context, imgResId);
			SetDrawableRightText(text, drawable);
		}

		public void SetDrawableRightText(string text, Drawable drawable)
		{
			Text = "";
			SpannableString ss = new SpannableString("pics");

			drawable.SetBounds(0, 0, drawable.IntrinsicWidth, drawable.IntrinsicHeight);
			var span = new ImageSpan(drawable, SpanAlign.Baseline);
			ss.SetSpan(span, 0, ss.Length(), SpanTypes.InclusiveExclusive);
			Append(text);
			Append(" ");
			Append(ss);
		}

		public void SetDrawableLeftText(int text, int imgResId)
		{
			SetDrawableLeftText(_resources.GetString(text), imgResId);
		}

		public void SetDrawableLeftText(string text, int imgResId)
		{
			Drawable drawable = ContextCompat.GetDrawable(Context, imgResId);
			SetDrawableLeftText(text, drawable);
		}

		public void SetDrawableLeftText(string text, Drawable drawable)
		{
			Text = "";
			var ss = new SpannableString("pics");
			drawable.SetBounds(0, 0, drawable.IntrinsicWidth, drawable.IntrinsicHeight);
			ImageSpan span = new ImageSpan(drawable, SpanAlign.Baseline);
			ss.SetSpan(span, 0, ss.Length(), SpanTypes.InclusiveExclusive);
			Append(ss);
			Append(" ");
			Append(text);
		}

		/**
		 * 颜色加深处理
		 *
		 * @param RGBValues RGB的值，由alpha（透明度）、red（红）、green（绿）、blue（蓝）构成，
		 *                  Android中我们一般使用它的16进制，
		 *                  例如："#FFAABBCC",最左边到最右每两个字母就是代表alpha（透明度）、
		 *                  red（红）、green（绿）、blue（蓝）。每种颜色值占一个字节(8位)，值域0~255
		 *                  所以下面使用移位的方法可以得到每种颜色的值，然后每种颜色值减小一下，在合成RGB颜色，颜色就会看起来深一些了
		 * @return
		 */
		int ColorBurn(int RGBValues)
		{
			int red = RGBValues >> 16 & 0xFF;
			int green = RGBValues >> 8 & 0xFF;
			int blue = RGBValues & 0xFF;
			red = (int)Math.Floor(red * (1 - 0.1));
			green = (int)Math.Floor(green * (1 - 0.1));
			blue = (int)Math.Floor(blue * (1 - 0.1));
			return Color.Rgb(red, green, blue);
		}

	}
}

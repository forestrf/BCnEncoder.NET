﻿using System;
using System.Numerics;
using SixLabors.ImageSharp.PixelFormats;

namespace BCnEnc.Net.Shared
{
	internal struct ColorRgb565 : IEquatable<ColorRgb565>
	{
		public bool Equals(ColorRgb565 other)
		{
			return data == other.data;
		}

		public override bool Equals(object obj)
		{
			return obj is ColorRgb565 other && Equals(other);
		}

		public override int GetHashCode()
		{
			return data.GetHashCode();
		}

		public static bool operator ==(ColorRgb565 left, ColorRgb565 right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ColorRgb565 left, ColorRgb565 right)
		{
			return !left.Equals(right);
		}

		private const ushort RedMask = 0b11111_000000_00000;
		private const int RedShift = 11;
		private const ushort GreenMask = 0b00000_111111_00000;
		private const int GreenShift = 5;
		private const ushort BlueMask = 0b00000_000000_11111;

		public ushort data;

		public byte R {
			readonly get {
				double rd = ((data & RedMask) >> RedShift) / (double)((1 << 5) - 1);
				return (byte)(rd * 255);
			}
			set {
				double rd = value / 255.0;
				byte r5 = (byte)(rd * ((1 << 5) - 1));
				data = (ushort)(data & ~RedMask);
				data = (ushort)(data | ((r5) << RedShift));
			}
		}

		public byte G {
			readonly get {
				double gd = ((data & GreenMask) >> GreenShift) / (double)((1 << 6) - 1);
				return (byte)(gd * 255);
			}
			set {
				double gd = value / 255.0;
				byte g6 = (byte)(gd * ((1 << 6) - 1));
				data = (ushort)(data & ~GreenMask);
				data = (ushort)(data | ((g6) << GreenShift));
			}
		}

		public byte B {
			readonly get {
				double bd = ((data & BlueMask)) / (double)((1 << 5) - 1);
				return (byte)(bd * 255);
			}
			set {
				double bd = value / 255.0;
				byte b5 = (byte)(bd * ((1 << 5) - 1));
				data = (ushort)(data & ~BlueMask);
				data = (ushort)(data | b5);
			}
		}

		public int RawR {
			readonly get { return ((data & RedMask) >> RedShift); }
			set {
				if (value > 31) value = 31;
				if (value < 0) value = 0;
				data = (ushort)(data & ~RedMask);
				data = (ushort)(data | ((value) << RedShift));
			}
		}

		public int RawG {
			readonly get { return ((data & GreenMask) >> GreenShift); }
			set {
				if (value > 63) value = 63;
				if (value < 0) value = 0;
				data = (ushort)(data & ~GreenMask);
				data = (ushort)(data | ((value) << GreenShift));
			}
		}

		public int RawB {
			readonly get { return (data & BlueMask); }
			set {
				if (value > 31) value = 31;
				if (value < 0) value = 0;
				data = (ushort)(data & ~BlueMask);
				data = (ushort)(data | value);
			}
		}

		public ColorRgb565(byte r, byte g, byte b)
		{
			data = 0;
			R = r;
			G = g;
			B = b;
		}

		public ColorRgb565(Vector3 colorVector) {
			data = 0;
			R = ByteHelper.ClampToByte(colorVector.X * 255);
			G = ByteHelper.ClampToByte(colorVector.Y * 255);
			B = ByteHelper.ClampToByte(colorVector.Z * 255);
		}

		public readonly ColorRgb24 ToColorRgb24()
		{
			return new ColorRgb24(R, G, B);
		}

		public override string ToString() {
			return $"r : {R} g : {G} b : {B}";
		}

		public ColorRgba32 ToColorRgba32() {
			return new ColorRgba32(R, G, B, 255);
		}
	}

	internal struct ColorRgba32 : IEquatable<ColorRgba32>
	{
		public byte r, g, b, a;
		public ColorRgba32(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public bool Equals(ColorRgba32 other)
		{
			return r == other.r && g == other.g && b == other.b && a == other.a;
		}

		public override bool Equals(object obj)
		{
			return obj is ColorRgba32 other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = r.GetHashCode();
				hashCode = (hashCode * 397) ^ g.GetHashCode();
				hashCode = (hashCode * 397) ^ b.GetHashCode();
				hashCode = (hashCode * 397) ^ a.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(ColorRgba32 left, ColorRgba32 right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ColorRgba32 left, ColorRgba32 right)
		{
			return !left.Equals(right);
		}

		public static ColorRgba32 operator +(ColorRgba32 left, ColorRgba32 right)
		{
			return new ColorRgba32(
				ByteHelper.ClampToByte(left.r + right.r),
				ByteHelper.ClampToByte(left.g + right.g),
				ByteHelper.ClampToByte(left.b + right.b),
				ByteHelper.ClampToByte(left.a + right.a));
		}

		public static ColorRgba32 operator -(ColorRgba32 left, ColorRgba32 right)
		{
			return new ColorRgba32(
				ByteHelper.ClampToByte(left.r - right.r),
				ByteHelper.ClampToByte(left.g - right.g),
				ByteHelper.ClampToByte(left.b - right.b),
				ByteHelper.ClampToByte(left.a - right.a));
		}

		public static ColorRgba32 operator /(ColorRgba32 left, double right)
		{
			return new ColorRgba32(
				ByteHelper.ClampToByte((int)(left.r / right)),
				ByteHelper.ClampToByte((int)(left.g / right)),
				ByteHelper.ClampToByte((int)(left.b / right)),
				ByteHelper.ClampToByte((int)(left.a / right))
			);
		}

		public static ColorRgba32 operator *(ColorRgba32 left, double right)
		{
			return new ColorRgba32(
				ByteHelper.ClampToByte((int)(left.r * right)),
				ByteHelper.ClampToByte((int)(left.g * right)),
				ByteHelper.ClampToByte((int)(left.b * right)),
				ByteHelper.ClampToByte((int)(left.a * right))
			);
		}

		public override string ToString() {
			return $"r : {r} g : {g} b : {b} a : {a}";
		}
	}

	internal struct ColorRgb24 : IEquatable<ColorRgb24>
	{
		public byte r, g, b;
		public ColorRgb24(byte r, byte g, byte b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
		}

		public ColorRgb24(ColorRgb565 color) : this() {
			this.r = color.R;
			this.g = color.G;
			this.b = color.B;
		}

		public bool Equals(ColorRgb24 other)
		{
			return r == other.r && g == other.g && b == other.b;
		}

		public override bool Equals(object obj)
		{
			return obj is ColorRgb24 other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = r.GetHashCode();
				hashCode = (hashCode * 397) ^ g.GetHashCode();
				hashCode = (hashCode * 397) ^ b.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(ColorRgb24 left, ColorRgb24 right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ColorRgb24 left, ColorRgb24 right)
		{
			return !left.Equals(right);
		}

		public static ColorRgb24 operator +(ColorRgb24 left, ColorRgb24 right)
		{
			return new ColorRgb24(
				ByteHelper.ClampToByte(left.r + right.r),
				ByteHelper.ClampToByte(left.g + right.g),
				ByteHelper.ClampToByte(left.b + right.b));
		}

		public static ColorRgb24 operator -(ColorRgb24 left, ColorRgb24 right)
		{
			return new ColorRgb24(
				ByteHelper.ClampToByte(left.r - right.r),
				ByteHelper.ClampToByte(left.g - right.g),
				ByteHelper.ClampToByte(left.b - right.b));
		}

		public static ColorRgb24 operator /(ColorRgb24 left, double right)
		{
			return new ColorRgb24(
				ByteHelper.ClampToByte((int)(left.r / right)),
				ByteHelper.ClampToByte((int)(left.g / right)),
				ByteHelper.ClampToByte((int)(left.b / right))
				);
		}

		public static ColorRgb24 operator *(ColorRgb24 left, double right)
		{
			return new ColorRgb24(
				ByteHelper.ClampToByte((int)(left.r * right)),
				ByteHelper.ClampToByte((int)(left.g * right)),
				ByteHelper.ClampToByte((int)(left.b * right))
			);
		}

		public override string ToString() {
			return $"r : {r} g : {g} b : {b}";
		}
	}

	internal struct ColorYCbCr
	{
		public float y;
		public float cb;
		public float cr;

		public ColorYCbCr(float y, float cb, float cr)
		{
			this.y = y;
			this.cb = cb;
			this.cr = cr;
		}

		public ColorYCbCr(ColorRgb24 rgb)
		{
			float fr = (float)rgb.r / 255;
			float fg = (float)rgb.g / 255;
			float fb = (float)rgb.b / 255;

			y = (0.2989f * fr + 0.5866f * fg + 0.1145f * fb);
			cb = (-0.1687f * fr - 0.3313f * fg + 0.5000f * fb);
			cr = (0.5000f * fr - 0.4184f * fg - 0.0816f * fb);
		}

		public ColorYCbCr(ColorRgb565 rgb)
		{
			float fr = (float)rgb.R / 255;
			float fg = (float)rgb.G / 255;
			float fb = (float)rgb.B / 255;

			y = (0.2989f * fr + 0.5866f * fg + 0.1145f * fb);
			cb = (-0.1687f * fr - 0.3313f * fg + 0.5000f * fb);
			cr = (0.5000f * fr - 0.4184f * fg - 0.0816f * fb);
		}

		public ColorYCbCr(ColorRgba32 rgba)
		{
			float fr = (float)rgba.r / 255;
			float fg = (float)rgba.g / 255;
			float fb = (float)rgba.b / 255;

			y = (0.2989f * fr + 0.5866f * fg + 0.1145f * fb);
			cb = (-0.1687f * fr - 0.3313f * fg + 0.5000f * fb);
			cr = (0.5000f * fr - 0.4184f * fg - 0.0816f * fb);
		}

		public ColorYCbCr(Rgba32 rgb)
		{
			float fr = (float)rgb.R / 255;
			float fg = (float)rgb.G / 255;
			float fb = (float)rgb.B / 255;

			y = (0.2989f * fr + 0.5866f * fg + 0.1145f * fb);
			cb = (-0.1687f * fr - 0.3313f * fg + 0.5000f * fb);
			cr = (0.5000f * fr - 0.4184f * fg - 0.0816f * fb);
		}

		public ColorYCbCr(Vector3 vec) {
			float fr = (float) vec.X;
			float fg = (float) vec.Y;
			float fb = (float) vec.Z;

			y = (0.2989f * fr + 0.5866f * fg + 0.1145f * fb);
			cb = (-0.1687f * fr - 0.3313f * fg + 0.5000f * fb);
			cr = (0.5000f * fr - 0.4184f * fg - 0.0816f * fb);
		}

		public ColorRgb565 ToColorRgb565() {
			float r = Math.Max(0.0f, Math.Min(1.0f, (float)(y + 0.0000 * cb + 1.4022 * cr)));
			float g = Math.Max(0.0f, Math.Min(1.0f, (float)(y - 0.3456 * cb - 0.7145 * cr)));
			float b = Math.Max(0.0f, Math.Min(1.0f, (float)(y + 1.7710 * cb + 0.0000 * cr)));

			return new ColorRgb565((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
		}

		public override string ToString() {
			float r = Math.Max(0.0f, Math.Min(1.0f, (float)(y + 0.0000 * cb + 1.4022 * cr)));
			float g = Math.Max(0.0f, Math.Min(1.0f, (float)(y - 0.3456 * cb - 0.7145 * cr)));
			float b = Math.Max(0.0f, Math.Min(1.0f, (float)(y + 1.7710 * cb + 0.0000 * cr)));

			return $"r : {r * 255} g : {g * 255} b : {b * 255}";
		}

		public float CalcDistWeighted(ColorYCbCr other, float yWeight = 4) {
			float dy = (y - other.y) * (y - other.y) * yWeight;
			float dcb = (cb - other.cb) * (cb - other.cb);
			float dcr = (cr - other.cr) * (cr - other.cr);

			return MathF.Sqrt(dy + dcb + dcr);
		}
	}
}
